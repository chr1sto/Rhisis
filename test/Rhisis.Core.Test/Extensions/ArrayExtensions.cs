using Rhisis.Core.Extensions;
using System.Collections.Generic;
using Xunit;

namespace Rhisis.Core.Test.Extensions
{
    public class ArrayExtensions
    {
        private readonly int[] integerArray = new[] { 2, 4, 5, 2, 5, 0, 7, 1 };
        private readonly IList<int> integerList = new List<int>() { 2, 4, 5, 2, 5, 0, 7, 1 };

        [Theory]
        [InlineData(2, 6, 7, 5)]
        [InlineData(6, 7, 1, 7)]
        [InlineData(0, 4, 5, 2)]
        public void SwapArrayElementTest(int source, int dest, int sourceValueAfterSwap, int destValueAfterSwap)
        {
            integerArray.Swap(source, dest);

            Assert.Equal(sourceValueAfterSwap, integerArray[source]);
            Assert.Equal(destValueAfterSwap, integerArray[dest]);
        }

        [Theory]
        [InlineData(2, 6, 7, 5)]
        [InlineData(6, 7, 1, 7)]
        [InlineData(0, 4, 5, 2)]
        public void SwapListElementTest(int source, int dest, int sourceValueAfterSwap, int destValueAfterSwap)
        {
            integerList.Swap(source, dest);

            Assert.Equal(sourceValueAfterSwap, integerList[source]);
            Assert.Equal(destValueAfterSwap, integerList[dest]);
        }
    }
}
