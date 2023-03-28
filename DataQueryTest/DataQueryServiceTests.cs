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
            new Entity { Name = "John", Age = 25 },
            new Entity { Name = "Jane", Age = 30 },
            new Entity { Name = "Jane", Age = 20 },
            new Entity { Name = "Bob", Age = 0 },
            new Entity { Name = "Sam", Age = 40 }
        };


        [Fact]
        public void EqualStringTest()
        {
            DataQueryService queryService = new();

            IEnumerable<Entity> expectedResult = new List<Entity>
            {
                new Entity { Name = "John", Age = 25 },
            };

            string filter = "name:john";

            IEnumerable<Entity> result = queryService.ApplyFilter(Entities, filter);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void ContainsStringTest()
        {
            DataQueryService queryService = new();

            IEnumerable<Entity> expectedResult = new List<Entity>
            {
                new Entity { Name = "John", Age = 25 },
                new Entity { Name = "Jane", Age = 30 },
                new Entity { Name = "Jane", Age = 20 },
            };

            string filter = "name;j";

            IEnumerable<Entity> result = queryService.ApplyFilter(Entities, filter);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void EqualIntTest()
        {
            DataQueryService queryService = new();

            IEnumerable<Entity> expectedResult = new List<Entity>
            {
                new Entity { Name = "Jane", Age = 20 },
            };

            string filter = "age:20";

            IEnumerable<Entity> result = queryService.ApplyFilter(Entities, filter);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void GreaterIntTest()
        {
            DataQueryService queryService = new();

            IEnumerable<Entity> expectedResult = new List<Entity>
            {
                new Entity { Name = "John", Age = 25 },
                new Entity { Name = "Jane", Age = 30 },
                new Entity { Name = "Sam", Age = 40 }
            };

            string filter = "age>20";

            IEnumerable<Entity> result = queryService.ApplyFilter(Entities, filter);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void EqualStringAndNumberAndLesserNumberTest()
        {
            DataQueryService queryService = new();

            IEnumerable<Entity> expectedResult = new List<Entity>
            {
                new Entity { Name = "John", Age = 25 },
                new Entity { Name = "Bob", Age = 0 },
                new Entity { Name = "Sam", Age = 40 }
            };

            string filter = "name:john|age:40|age<5";

            IEnumerable<Entity> result = queryService.ApplyFilter(Entities, filter);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void OrAndTest()
        {
            DataQueryService queryService = new();

            IEnumerable<Entity> expectedResult = new List<Entity>
            {
                new Entity { Name = "Sam", Age = 40 }
            };

            string filter = "name:john&age>25|name;sa";

            IEnumerable<Entity> result = queryService.ApplyFilter(Entities, filter);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }
    }

    public class Entity
    {
        public string Name { get; set; } = null!;
        public int Age { get; set; }
    }
}
