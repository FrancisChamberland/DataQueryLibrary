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
        And = '&',
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
                            ?? throw new ArgumentException("PropertyNotFound");

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

                    if (propertyValue is DateTime && DateTime.TryParse(filterValue, out DateTime dateValue))
                        return (DateTime)propertyValue == dateValue;

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