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
    }
}
