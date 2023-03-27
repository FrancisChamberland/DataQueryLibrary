using System.Reflection;

namespace DataQueryLibrary
{
    internal enum Operator
    {
        Equal = ':',
        Greater = '>',
        Less = '<'
    }

    public class DataQueryService<T> where T : class
    {

        // name:pipo,age>5
        public IEnumerable<T> ApplyFilter(IEnumerable<T> entities, string filter)
        {
            string[] filters = filter.Split(',');

            foreach (T entity in entities)
            {
                bool isMatch = false;

                for (int i = 0; i < filters.Length; i++)
                {
                    char? comparator = null;

                    foreach (Operator @operator in Enum.GetValues(typeof(Operator)))
                    {
                        if (!filters[i].Contains((char)@operator)) continue;
                        comparator = (char)@operator;
                        break;
                    }

                    if (comparator is null) throw new ArgumentException("InvalidComparisonOperator");

                    string[] filterArray = filters[i].Trim().Split(comparator!.Value);

                    if (filterArray.Length != 2) throw new ArgumentException("InvalidFilterValueOrProperty");

                    string propertyName = filterArray[0].TrimEnd(), filterValue = filterArray[1].TrimStart();
                    PropertyInfo property = typeof(T).GetProperty(propertyName) ?? throw new ArgumentException("PropertyNotFound");

                    object? propertValue = property.GetValue(entity);

                    if (propertValue is null) continue;

                    switch (comparator!.Value)
                    {
                        case (char)Operator.Equal:
                            isMatch = propertValue!.ToString() == filterValue;
                            break;
                        case (char)Operator.Greater:
                            if (!IsNumericType(propertValue)) continue;
                            break;
                        case (char)Operator.Less:
                            break;
                    }
                }
            }

            throw new NotImplementedException();
        }

        private bool IsNumericType(object value)
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