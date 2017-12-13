using Rhisis.Core.Resources;
using Xunit;

namespace Rhisis.Core.Test.Resources
{
    public class ResourceTableUnitTest
    {
        private static readonly string ResourceTablePath = "Common/resource.txt";
        private static readonly int ResourceTableCount = 3;
        private static readonly int ResourceTableHeaderIndex = 2;

        /// <summary>
        /// Open and read a <see cref="ResourceTable"/> file.
        /// </summary>
        [Fact]
        public void OpenResourceTable()
        {
            var resourceTable = new ResourceTable(ResourceTablePath, ResourceTableHeaderIndex);

            Assert.NotNull(resourceTable);
            Assert.Equal(ResourceTableCount, resourceTable.Count);

            resourceTable.Dispose();
        }
    }
}
