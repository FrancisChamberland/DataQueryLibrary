﻿using DataQueryLibrary;
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
            new Entity { Name = "John", Age = 25, BirthDate = new(1999, 02, 20), MonthlySalary = 2000, PhoneNumber = "450 939-8234", Gender = Gender.Male },
            new Entity { Name = "Sam", Age = 25, BirthDate = new(2010, 03, 10), MonthlySalary = 2000, PhoneNumber = "450 838-1234", Gender = Gender.Male },
            new Entity { Name = "Jane", Age = 30, BirthDate = new(2001, 06, 14), MonthlySalary = 4000, PhoneNumber = "450 677-5499", Gender = Gender.Male },
            new Entity { Name = "Jane", Age = 20, BirthDate = new(1999, 12, 15), MonthlySalary = 5000, PhoneNumber = null, Gender = Gender.Female }, 
            new Entity { Name = "Bob", Age = 0, BirthDate = new(2005, 10, 3), MonthlySalary = 6000, PhoneNumber = "514 233-666", Gender = Gender.Female },
            new Entity { Name = "Carl", Age = 15, BirthDate = new(2007, 03, 12), MonthlySalary = 100, PhoneNumber = null, Gender = Gender.Female },
            new Entity { Name = "Frank", Age = 88, BirthDate = new(1895, 02, 20), MonthlySalary = 15000, PhoneNumber = null, Gender = Gender.Male },
            new Entity { Name = "Zack", Age = 52, BirthDate = new(1925, 01, 27), MonthlySalary = 2400, PhoneNumber = "514 444-4444", Gender = Gender.Female }
        };


        [Fact]
        public void FilterEqualStringTest()
        {
            DataQueryService queryService = new();

            IEnumerable<Entity> expectedResult = new List<Entity>
            {
                new Entity { Name = "John", Age = 25, BirthDate = new(1999, 02, 20), MonthlySalary = 2000, PhoneNumber = "450 939-8234", Gender = Gender.Male },
            };

            string filter = "name:John";

            IEnumerable<Entity> result = queryService.ApplyFilter(Entities, filter);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void FilterEqualIntTest()
        {
            DataQueryService queryService = new();

            IEnumerable<Entity> expectedResult = new List<Entity>
            {
                new Entity { Name = "Jane", Age = 20, BirthDate = new(1999, 12, 15), MonthlySalary = 5000, PhoneNumber = null, Gender = Gender.Female },
            };

            string filter = "age:20";

            IEnumerable<Entity> result = queryService.ApplyFilter(Entities, filter);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void FilterEqualDateTest()
        {
            DataQueryService queryService = new();

            IEnumerable<Entity> expectedResult = new List<Entity>
            {
                new Entity { Name = "Bob", Age = 0, BirthDate = new(2005, 10, 3), MonthlySalary = 6000, PhoneNumber = "514 233-666", Gender = Gender.Female },
            };

            string filter = "birthdate:2005/10/3";

            IEnumerable<Entity> result = queryService.ApplyFilter(Entities, filter);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }


        [Fact]
        public void FilterEqualEnumTest()
        {
            DataQueryService queryService = new();

            IEnumerable<Entity> expectedResult = new List<Entity>
            {
                new Entity { Name = "Jane", Age = 20, BirthDate = new(1999, 12, 15), MonthlySalary = 5000, PhoneNumber = null, Gender = Gender.Female },
                new Entity { Name = "Bob", Age = 0, BirthDate = new(2005, 10, 3), MonthlySalary = 6000, PhoneNumber = "514 233-666", Gender = Gender.Female },
                new Entity { Name = "Carl", Age = 15, BirthDate = new(2007, 03, 12), MonthlySalary = 100, PhoneNumber = null, Gender = Gender.Female },
                new Entity { Name = "Zack", Age = 52, BirthDate = new(1925, 01, 27), MonthlySalary = 2400, PhoneNumber = "514 444-4444", Gender = Gender.Female }
            };

            string filter = "gender:1";

            IEnumerable<Entity> result = queryService.ApplyFilter(Entities, filter);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void FilterEqualNullTest()
        {
            DataQueryService queryService = new();

            IEnumerable<Entity> expectedResult = new List<Entity>
            {
                new Entity { Name = "Jane", Age = 20, BirthDate = new(1999, 12, 15), MonthlySalary = 5000, PhoneNumber = null, Gender = Gender.Female },
                new Entity { Name = "Carl", Age = 15, BirthDate = new(2007, 03, 12), MonthlySalary = 100, PhoneNumber = null, Gender = Gender.Female },
                new Entity { Name = "Frank", Age = 88, BirthDate = new(1895, 02, 20), MonthlySalary = 15000, PhoneNumber = null, Gender = Gender.Male },
            };

            string filter = "phonenumber:null";

            IEnumerable<Entity> result = queryService.ApplyFilter(Entities, filter);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void FilterContainsStringTest()
        {
            DataQueryService queryService = new();

            IEnumerable<Entity> expectedResult = new List<Entity>
            {
                new Entity { Name = "John", Age = 25, BirthDate = new(1999, 02, 20), MonthlySalary = 2000, PhoneNumber = "450 939-8234", Gender = Gender.Male },
                new Entity { Name = "Jane", Age = 30, BirthDate = new(2001, 06, 14), MonthlySalary = 4000, PhoneNumber = "450 677-5499", Gender = Gender.Male },
                new Entity { Name = "Jane", Age = 20, BirthDate = new(1999, 12, 15), MonthlySalary = 5000, PhoneNumber = null, Gender = Gender.Female },
            };

            string filter = "name;j";

            IEnumerable<Entity> result = queryService.ApplyFilter(Entities, filter);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void FilterGreaterIntTest()
        {
            DataQueryService queryService = new();

            IEnumerable<Entity> expectedResult = new List<Entity>
            {
                new Entity { Name = "John", Age = 25, BirthDate = new(1999, 02, 20), MonthlySalary = 2000, PhoneNumber = "450 939-8234", Gender = Gender.Male },
                new Entity { Name = "Sam", Age = 25, BirthDate = new(2010, 03, 10), MonthlySalary = 2000, PhoneNumber = "450 838-1234", Gender = Gender.Male },
                new Entity { Name = "Jane", Age = 30, BirthDate = new(2001, 06, 14), MonthlySalary = 4000, PhoneNumber = "450 677-5499", Gender = Gender.Male },
                new Entity { Name = "Frank", Age = 88, BirthDate = new(1895, 02, 20), MonthlySalary = 15000, PhoneNumber = null, Gender = Gender.Male },
                new Entity { Name = "Zack", Age = 52, BirthDate = new(1925, 01, 27), MonthlySalary = 2400, PhoneNumber = "514 444-4444", Gender = Gender.Female }
            };

            string filter = "age>20";

            IEnumerable<Entity> result = queryService.ApplyFilter(Entities, filter);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void FilterGreaterNullTest()
        {
            DataQueryService queryService = new();

            string filter = "age>null";

            Assert.Throws<ArgumentException>(() =>  queryService.ApplyFilter(Entities, filter));
        }

        [Theory]
        [InlineData("birthdate>2000/01/01")]
        [InlineData("birthdate>01/01/2000")]
        [InlineData("birthdate>2000-01-01")]
        [InlineData("birthdate>01-01-2000")]
        public void FilterGreaterDateTest(string filter)
        {
            DataQueryService queryService = new();

            IEnumerable<Entity> expectedResult = new List<Entity>
            {
                new Entity { Name = "Sam", Age = 25, BirthDate = new(2010, 03, 10), MonthlySalary = 2000, PhoneNumber = "450 838-1234", Gender = Gender.Male },
                new Entity { Name = "Jane", Age = 30, BirthDate = new(2001, 06, 14), MonthlySalary = 4000, PhoneNumber = "450 677-5499", Gender = Gender.Male },
                new Entity { Name = "Bob", Age = 0, BirthDate = new(2005, 10, 3), MonthlySalary = 6000, PhoneNumber = "514 233-666", Gender = Gender.Female },
                new Entity { Name = "Carl", Age = 15, BirthDate = new(2007, 03, 12), MonthlySalary = 100, PhoneNumber = null, Gender = Gender.Female },
            };

            IEnumerable<Entity> result = queryService.ApplyFilter(Entities, filter);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void FilterEqualStringAndNumberAndLesserNumberTest()
        {
            DataQueryService queryService = new();

            IEnumerable<Entity> expectedResult = new List<Entity>
            {
                new Entity { Name = "John", Age = 25, BirthDate = new(1999, 02, 20), MonthlySalary = 2000, PhoneNumber = "450 939-8234", Gender = Gender.Male },
                new Entity { Name = "Bob", Age = 0, BirthDate = new(2005, 10, 3), MonthlySalary = 6000, PhoneNumber = "514 233-666", Gender = Gender.Female },
            };

            string filter = "name:John|age:40|age<5";

            IEnumerable<Entity> result = queryService.ApplyFilter(Entities, filter);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void FilterOrAndTest()
        {
            DataQueryService queryService = new();

            IEnumerable<Entity> expectedResult = new List<Entity>
            {
                new Entity { Name = "Jane", Age = 30, BirthDate = new(2001, 06, 14), MonthlySalary = 4000, PhoneNumber = "450 677-5499", Gender = Gender.Male },
                new Entity { Name = "Frank", Age = 88, BirthDate = new(1895, 02, 20), MonthlySalary = 15000, PhoneNumber = null, Gender = Gender.Male },
            };

            string filter = "name;j,age>25|name;k,age>60";

            IEnumerable<Entity> result = queryService.ApplyFilter(Entities, filter);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void FilterWhiteSpaceTest()
        {
            DataQueryService queryService = new();

            IEnumerable<Entity> expectedResult = new List<Entity>
            {
                new Entity { Name = "Jane", Age = 30, BirthDate = new(2001, 06, 14), MonthlySalary = 4000, PhoneNumber = "450 677-5499", Gender = Gender.Male },
                new Entity { Name = "Frank", Age = 88, BirthDate = new(1895, 02, 20), MonthlySalary = 15000, PhoneNumber = null, Gender = Gender.Male },
            };

            string filter = " name ; j , age > 25 | name ; k , age > 60 ";

            IEnumerable<Entity> result = queryService.ApplyFilter(Entities, filter);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void OrderAscendingIntTest()
        {
            DataQueryService queryService = new();

            IEnumerable<Entity> expectedResult = new List<Entity>
            {
                new Entity { Name = "Bob", Age = 0, BirthDate = new(2005, 10, 3), MonthlySalary = 6000, PhoneNumber = "514 233-666", Gender = Gender.Female },
                new Entity { Name = "Carl", Age = 15, BirthDate = new(2007, 03, 12), MonthlySalary = 100, PhoneNumber = null, Gender = Gender.Female },
                new Entity { Name = "Jane", Age = 20, BirthDate = new(1999, 12, 15), MonthlySalary = 5000, PhoneNumber = null, Gender = Gender.Female },
                new Entity { Name = "John", Age = 25, BirthDate = new(1999, 02, 20), MonthlySalary = 2000, PhoneNumber = "450 939-8234", Gender = Gender.Male },
                new Entity { Name = "Sam", Age = 25, BirthDate = new(2010, 03, 10), MonthlySalary = 2000, PhoneNumber = "450 838-1234", Gender = Gender.Male },
                new Entity { Name = "Jane", Age = 30, BirthDate = new(2001, 06, 14), MonthlySalary = 4000, PhoneNumber = "450 677-5499", Gender = Gender.Male },
                new Entity { Name = "Zack", Age = 52, BirthDate = new(1925, 01, 27), MonthlySalary = 2400, PhoneNumber = "514 444-4444", Gender = Gender.Female },
                new Entity { Name = "Frank", Age = 88, BirthDate = new(1895, 02, 20), MonthlySalary = 15000, PhoneNumber = null, Gender = Gender.Male },
            };

            string order = "age:asc";

            IEnumerable<Entity> result = queryService.ApplyOrder(Entities, order);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void OrderDescendingDateTest()
        {
            DataQueryService queryService = new();

            IEnumerable<Entity> expectedResult = new List<Entity>
            {
                new Entity { Name = "Sam", Age = 25, BirthDate = new(2010, 03, 10), MonthlySalary = 2000, PhoneNumber = "450 838-1234", Gender = Gender.Male },
                new Entity { Name = "Carl", Age = 15, BirthDate = new(2007, 03, 12), MonthlySalary = 100, PhoneNumber = null, Gender = Gender.Female },
                new Entity { Name = "Bob", Age = 0, BirthDate = new(2005, 10, 3), MonthlySalary = 6000, PhoneNumber = "514 233-666", Gender = Gender.Female },
                new Entity { Name = "Jane", Age = 30, BirthDate = new(2001, 06, 14), MonthlySalary = 4000, PhoneNumber = "450 677-5499", Gender = Gender.Male },
                new Entity { Name = "Jane", Age = 20, BirthDate = new(1999, 12, 15), MonthlySalary = 5000, PhoneNumber = null, Gender = Gender.Female },
                new Entity { Name = "John", Age = 25, BirthDate = new(1999, 02, 20), MonthlySalary = 2000, PhoneNumber = "450 939-8234", Gender = Gender.Male },
                new Entity { Name = "Zack", Age = 52, BirthDate = new(1925, 01, 27), MonthlySalary = 2400, PhoneNumber = "514 444-4444", Gender = Gender.Female },
                new Entity { Name = "Frank", Age = 88, BirthDate = new(1895, 02, 20), MonthlySalary = 15000, PhoneNumber = null, Gender = Gender.Male },
            };

            string order = "birthdate:desc";

            IEnumerable<Entity> result = queryService.ApplyOrder(Entities, order);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void OrderDescendingIntThenDescendingDateTest()
        {
            DataQueryService queryService = new();

            IEnumerable<Entity> expectedResult = new List<Entity>
            {
                new Entity { Name = "Frank", Age = 88, BirthDate = new(1895, 02, 20), MonthlySalary = 15000, PhoneNumber = null, Gender = Gender.Male },
                new Entity { Name = "Zack", Age = 52, BirthDate = new(1925, 01, 27), MonthlySalary = 2400, PhoneNumber = "514 444-4444", Gender = Gender.Female },
                new Entity { Name = "Jane", Age = 30, BirthDate = new(2001, 06, 14), MonthlySalary = 4000, PhoneNumber = "450 677-5499", Gender = Gender.Male },
                new Entity { Name = "Sam", Age = 25, BirthDate = new(2010, 03, 10), MonthlySalary = 2000, PhoneNumber = "450 838-1234", Gender = Gender.Male },
                new Entity { Name = "John", Age = 25, BirthDate = new(1999, 02, 20), MonthlySalary = 2000, PhoneNumber = "450 939-8234", Gender = Gender.Male },
                new Entity { Name = "Jane", Age = 20, BirthDate = new(1999, 12, 15), MonthlySalary = 5000, PhoneNumber = null, Gender = Gender.Female },
                new Entity { Name = "Carl", Age = 15, BirthDate = new(2007, 03, 12), MonthlySalary = 100, PhoneNumber = null, Gender = Gender.Female },
                new Entity { Name = "Bob", Age = 0, BirthDate = new(2005, 10, 3), MonthlySalary = 6000, PhoneNumber = "514 233-666", Gender = Gender.Female },
            };

            string order = "age:desc,birthdate:desc";

            IEnumerable<Entity> result = queryService.ApplyOrder(Entities, order);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }
    }

    public enum Gender
    {
        Male,
        Female
    }

    public class Entity
    {
        public string Name { get; set; } = null!;
        public int Age { get; set; }
        public DateTime BirthDate { get; set; }
        public decimal MonthlySalary { get; set; }
        public string? PhoneNumber { get; set; }
        public Gender Gender { get; set; }
    }
}
