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
    }

    public class SomeData
    {
        public int Id { get; set; }

        public string Value => $"Value {this.Id}";

        public SomeData(int id)
        {
            this.Id = id;
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
