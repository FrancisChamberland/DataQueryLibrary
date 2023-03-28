using System.Reflection;
using System.Text.RegularExpressions;

namespace DataQueryLibrary
{
    internal enum ComparisonOperator
    {
        Equals = ':',
        Contains = ';',
        Greater = '>',
        Less = '<',
    }

    internal enum ConditionalOperator
    {
        And = ',',
        Or = '|',
    }

    public class DataQueryService
    {
        public IEnumerable<T> ApplyFilter<T>(ICollection<T> entities, string filter) where T : class
        {
            string[] filters = filter.Split((char)ConditionalOperator.Or);

            for (int i = entities.Count - 1; i > -1; i--)
            {
                T entity = entities.ElementAt(i);
                bool isMatch = false;

                for (int j = 0; j < filters.Length; j++)
                {
                    string[] subFilters = filters[j].Split((char)ConditionalOperator.And);
                    for (int k = 0;  k < subFilters.Length; k++)
                    {
                        char comparator = GetFilterComparator(subFilters[k])
                            ?? throw new ArgumentException("InvalidComparisonOperator");

                        string[] filterArray = subFilters[k].Trim().Split(comparator);
                        if (filterArray.Length != 2) throw new ArgumentException("InvalidFilterValueOrProperty");

                        string propertyName = filterArray[0].TrimEnd(), filterValue = filterArray[1].TrimStart();

                        PropertyInfo property = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                            ?? throw new ArgumentException("PropertyNotFound", propertyName);

                        isMatch = ValueMatchFilter(comparator, property.GetValue(entity), filterValue);
                        if (!isMatch) break;
                    }
                    if (isMatch) break;
                }
                if (isMatch) continue;
                entities.Remove(entity);
            }

            return entities;
        }

        public IEnumerable<T> ApplyOrder<T>(IEnumerable<T> entities, string order) where T : class
        {
            IOrderedEnumerable<T>? orderedEntites = null;

            string[] orders = order.Split((char)ConditionalOperator.And);

            for (int i = 0; i < orders.Length; i++)
            {
                string[] orderArray = orders[i].Trim().Split((char)ComparisonOperator.Equals);
                if (orderArray.Length != 2) throw new ArgumentException("InvalidOrderValueOrProperty");

                string propertyName = orderArray[0].TrimEnd(), orderValue = orderArray[1].TrimStart();

                PropertyInfo property = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                            ?? throw new ArgumentException("PropertyNotFound", propertyName);

                switch (orderValue)
                {
                    case "asc":
                        orderedEntites = orderedEntites is null ? entities.OrderBy(e => property.GetValue(e)) : 
                            orderedEntites.ThenBy(e => property.GetValue(e));
                        break;
                    case "desc":
                        orderedEntites = orderedEntites is null ? entities.OrderByDescending(e => property.GetValue(e)) : 
                            orderedEntites.ThenByDescending(e => property.GetValue(e));
                        break;
                    default:
                        throw new ArgumentException("InvalidOrderValue", orderValue);
                }
            }

            return orderedEntites is null ? entities : orderedEntites;
        }

        private char? GetFilterComparator(string filter)
        {
            foreach (ComparisonOperator @operator in Enum.GetValues(typeof(ComparisonOperator)))
            {
                if (!filter.Contains((char)@operator)) continue;
                return (char)@operator;
            }
            return null;
        }

        private bool ValueMatchFilter(char comparator, object? propertyValue, string filterValue)
        {
            switch (comparator)
            {
                case (char)ComparisonOperator.Equals:
                    if (filterValue == "null")
                        return propertyValue is null;

                    if (propertyValue is null) return false;

                    if (propertyValue is DateTime && DateTime.TryParse(filterValue, out DateTime dateValue))
                        return (DateTime)propertyValue == dateValue;

                    if (propertyValue is Enum && int.TryParse(filterValue, out int enumValue))
                        return (int)propertyValue == enumValue;

                    return propertyValue is null ? false : propertyValue!.ToString() == filterValue;

                case (char)ComparisonOperator.Contains:
                    if (propertyValue is null) return false;
                    if (propertyValue is not string) 
                        throw new ArgumentException("ValueTypeDoesNotMatchOperator");

                    return ((string)propertyValue!).ToUpper().Contains(filterValue.ToUpper());

                case (char)ComparisonOperator.Greater:
                    if (propertyValue is null) return false;

                    if (propertyValue is DateTime && DateTime.TryParse(filterValue, out DateTime dateValueG))
                        return (DateTime)propertyValue > dateValueG;

                    if (!IsValidNumericType(propertyValue!) || !double.TryParse(filterValue, out double numberValueG))
                        throw new ArgumentException("ValueTypeDoesNotMatchOperator");

                    return Convert.ToDouble(propertyValue!) > numberValueG;

                case (char)ComparisonOperator.Less:
                    if (propertyValue is null) return false;

                    if (propertyValue is DateTime && DateTime.TryParse(filterValue, out DateTime dateValueL))
                        return (DateTime)propertyValue < dateValueL;

                    if (!IsValidNumericType(propertyValue!) || !double.TryParse(filterValue, out double numberValueL))
                        throw new ArgumentException("ValueTypeDoesNotMatchOperator");

                    return Convert.ToDouble(propertyValue!) < numberValueL;

                default:
                    return false;
            }
        }

        private bool IsValidNumericType(object value)
        {
            return value is byte
                    || value is int
                    || value is long
                    || value is float
                    || value is double
                    || value is decimal;
        }
    }
}