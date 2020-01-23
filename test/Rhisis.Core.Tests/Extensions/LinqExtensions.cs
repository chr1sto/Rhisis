using Bogus;
using Rhisis.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Rhisis.Core.Test.Extensions
{
    public class LinqExtensions
    {
        private readonly IEnumerable<int> array = Enumerable.Repeat(new Random().Next(), 64);

        public static readonly IEnumerable<object[]> DuplicateTestData = new List<object[]>
        {
            new object[] { SomeData.Generate(10), false },
            new object[] { SomeData.Generate(10), false },
            new object[] { SomeData.Generate(40, true), true },
            new object[] { SomeData.Generate(20, true), true },
        };

        private readonly IDictionary<int, int> _dictionary;

        public LinqExtensions()
        {
            var faker = new Faker();

            _dictionary = Enumerable.Range(0, 10)
                .Select(x => new KeyValuePair<int, int>(faker.Random.Int(), faker.Random.Int()))
                .ToDictionary(x => x.Key, x => x.Value);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(10)]
        [InlineData(32)]
        [InlineData(64)]
        public void GroupByAmount(int itemsPerGroup)
        {
            int numberOfGroups = (array.Count() / itemsPerGroup);

            if (array.Count() % itemsPerGroup > 0)
                numberOfGroups++;

            var groups = array.GroupBy(itemsPerGroup);

            Assert.Equal(numberOfGroups, groups.Count());
            Assert.Equal(array.Count(), groups.Sum(x => x.Count()));
        }

        [Theory]
        [MemberData(nameof(DuplicateTestData))]
        public void HasDuplicatesTest(IEnumerable<SomeData> data, bool hasDuplicate)
        {
            bool containsDuplicate = data.HasDuplicates(x => x.Id);

            Assert.Equal(hasDuplicate, containsDuplicate);
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(5, false)]
        [InlineData(9, false)]
        [InlineData(-1, true)]
        [InlineData(10, true)]
        public void GetElementAtIndexOrDefault(int index, bool shouldBeDefault)
        {
            int value = _dictionary.GetValueAtIndexOrDefault(index);

            if (shouldBeDefault)
            {
                Assert.Equal(default, value);
            }
            else
            {
                Assert.Equal(_dictionary.ElementAt(index).Value, value);
            }
        }
    }

    public class SomeData
    {
        public int Id { get; set; }

        public string Value => $"Value {Id}";

        public SomeData(int id)
        {
            Id = id;
        }

        public static IEnumerable<SomeData> Generate(int count, bool hasDuplicates = false)
        {
            var data = Enumerable.Range(0, count).Select(x => new SomeData(x)).ToList();

            if (hasDuplicates)
                data.Add(data.ElementAt(0));

            return data;
        }
    }
}
