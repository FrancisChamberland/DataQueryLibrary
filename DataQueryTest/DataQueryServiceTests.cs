using DataQueryLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataQueryTest.DataQueryServiceTests;

namespace DataQueryTest
{
    public class DataQueryServiceTests
    {
        public ICollection<Entity> Entities { get; } = new List<Entity>
        {
            new Entity { Name = "John", Age = 25, BirthDate = new(1999, 02, 20), MonthlySalary = 2000, PhoneNumber = "450 939-8234" },
            new Entity { Name = "Jane", Age = 30, BirthDate = new(2001, 06, 14), MonthlySalary = 4000, PhoneNumber = "450 677-5499" },
            new Entity { Name = "Jane", Age = 20, BirthDate = new(1999, 12, 15), MonthlySalary = 5000, PhoneNumber = null }, 
            new Entity { Name = "Bob", Age = 0, BirthDate = new(2005, 10, 3), MonthlySalary = 6000, PhoneNumber = "514 233-666" },
            new Entity { Name = "Carl", Age = 15, BirthDate = new(2007, 03, 12), MonthlySalary = 100, PhoneNumber = null },
            new Entity { Name = "Frank", Age = 88, BirthDate = new(1895, 02, 20), MonthlySalary = 15000, PhoneNumber = null },
            new Entity { Name = "Zack", Age = 52, BirthDate = new(1925, 01, 27), MonthlySalary = 2400, PhoneNumber = "514 444-4444" }
        };


        [Fact]
        public void EqualStringTest()
        {
            DataQueryService queryService = new();

            IEnumerable<Entity> expectedResult = new List<Entity>
            {
                new Entity { Name = "John", Age = 25, BirthDate = new(1999, 02, 20), MonthlySalary = 2000, PhoneNumber = "450 939-8234" },
            };

            string filter = "name:John";

            IEnumerable<Entity> result = queryService.ApplyFilter(Entities, filter);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void EqualIntTest()
        {
            DataQueryService queryService = new();

            IEnumerable<Entity> expectedResult = new List<Entity>
            {
                new Entity { Name = "Jane", Age = 20, BirthDate = new(1999, 12, 15), MonthlySalary = 5000, PhoneNumber = null },
            };

            string filter = "age:20";

            IEnumerable<Entity> result = queryService.ApplyFilter(Entities, filter);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void EqualDateTest()
        {
            DataQueryService queryService = new();

            IEnumerable<Entity> expectedResult = new List<Entity>
            {
                new Entity { Name = "Bob", Age = 0, BirthDate = new(2005, 10, 3), MonthlySalary = 6000, PhoneNumber = "514 233-666" },
            };

            string filter = "birthdate:2005/10/3";

            IEnumerable<Entity> result = queryService.ApplyFilter(Entities, filter);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void EqualNullTest()
        {
            DataQueryService queryService = new();

            IEnumerable<Entity> expectedResult = new List<Entity>
            {
                new Entity { Name = "Jane", Age = 20, BirthDate = new(1999, 12, 15), MonthlySalary = 5000, PhoneNumber = null },
                new Entity { Name = "Carl", Age = 15, BirthDate = new(2007, 03, 12), MonthlySalary = 100, PhoneNumber = null },
                new Entity { Name = "Frank", Age = 88, BirthDate = new(1895, 02, 20), MonthlySalary = 15000, PhoneNumber = null },
            };

            string filter = "phonenumber:null";

            IEnumerable<Entity> result = queryService.ApplyFilter(Entities, filter);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void ContainsStringTest()
        {
            DataQueryService queryService = new();

            IEnumerable<Entity> expectedResult = new List<Entity>
            {
                new Entity { Name = "John", Age = 25, BirthDate = new(1999, 02, 20), MonthlySalary = 2000, PhoneNumber = "450 939-8234" },
                new Entity { Name = "Jane", Age = 30, BirthDate = new(2001, 06, 14), MonthlySalary = 4000, PhoneNumber = "450 677-5499" },
                new Entity { Name = "Jane", Age = 20, BirthDate = new(1999, 12, 15), MonthlySalary = 5000, PhoneNumber = null },
            };

            string filter = "name;j";

            IEnumerable<Entity> result = queryService.ApplyFilter(Entities, filter);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void GreaterIntTest()
        {
            DataQueryService queryService = new();

            IEnumerable<Entity> expectedResult = new List<Entity>
            {
                new Entity { Name = "John", Age = 25, BirthDate = new(1999, 02, 20), MonthlySalary = 2000, PhoneNumber = "450 939-8234" },
                new Entity { Name = "Jane", Age = 30, BirthDate = new(2001, 06, 14), MonthlySalary = 4000, PhoneNumber = "450 677-5499" },
                new Entity { Name = "Frank", Age = 88, BirthDate = new(1895, 02, 20), MonthlySalary = 15000, PhoneNumber = null },
                new Entity { Name = "Zack", Age = 52, BirthDate = new(1925, 01, 27), MonthlySalary = 2400, PhoneNumber = "514 444-4444" }
            };

            string filter = "age>20";

            IEnumerable<Entity> result = queryService.ApplyFilter(Entities, filter);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void GreaterNullTest()
        {
            DataQueryService queryService = new();

            string filter = "age>null";

            Assert.Throws<ArgumentException>(() =>  queryService.ApplyFilter(Entities, filter));
        }

        [Fact]
        public void GreaterDateTest()
        {
            DataQueryService queryService = new();

            IEnumerable<Entity> expectedResult = new List<Entity>
            {
                new Entity { Name = "Jane", Age = 30, BirthDate = new(2001, 06, 14), MonthlySalary = 4000, PhoneNumber = "450 677-5499" },
                new Entity { Name = "Bob", Age = 0, BirthDate = new(2005, 10, 3), MonthlySalary = 6000, PhoneNumber = "514 233-666" },
                new Entity { Name = "Carl", Age = 15, BirthDate = new(2007, 03, 12), MonthlySalary = 100, PhoneNumber = null },
            };

            string filter = "birthdate>2000/01/01";

            IEnumerable<Entity> result = queryService.ApplyFilter(Entities, filter);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void EqualStringAndNumberAndLesserNumberTest()
        {
            DataQueryService queryService = new();

            IEnumerable<Entity> expectedResult = new List<Entity>
            {
                new Entity { Name = "John", Age = 25, BirthDate = new(1999, 02, 20), MonthlySalary = 2000, PhoneNumber = "450 939-8234" },
                new Entity { Name = "Bob", Age = 0, BirthDate = new(2005, 10, 3), MonthlySalary = 6000, PhoneNumber = "514 233-666" },
            };

            string filter = "name:John|age:40|age<5";

            IEnumerable<Entity> result = queryService.ApplyFilter(Entities, filter);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void OrAndTest()
        {
            DataQueryService queryService = new();

            IEnumerable<Entity> expectedResult = new List<Entity>
            {
                new Entity { Name = "Jane", Age = 30, BirthDate = new(2001, 06, 14), MonthlySalary = 4000, PhoneNumber = "450 677-5499" },
                new Entity { Name = "Frank", Age = 88, BirthDate = new(1895, 02, 20), MonthlySalary = 15000, PhoneNumber = null },
            };

            string filter = "name;j&age>25|name;k&age>60";

            IEnumerable<Entity> result = queryService.ApplyFilter(Entities, filter);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }
    }

    public class Entity
    {
        public string Name { get; set; } = null!;
        public int Age { get; set; }
        public DateTime BirthDate { get; set; }
        public decimal MonthlySalary { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
