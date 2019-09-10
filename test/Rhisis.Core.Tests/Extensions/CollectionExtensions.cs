using Bogus;
using Rhisis.Core.Extensions;
using System.Collections;
using System.Linq;
using Xunit;

namespace Rhisis.Core.Test.Extensions
{
    public class CollectionExtensions
    {
        private readonly int[] _data;
        private readonly ICollection _collection;

        public CollectionExtensions()
        {
            var faker = new Faker();

            this._data = Enumerable.Range(0, faker.Random.Byte() + 1).Select(x => faker.Random.Int()).ToArray();
            this._collection = new ArrayList(this._data);
        }

        [Fact]
        public void ConvertCollectionToArrayTest()
        {
            int[] arrayOfData = this._collection.ToArray<int>();

            Assert.NotNull(arrayOfData);
            Assert.Equal(this._data.Length, arrayOfData.Length);
            Assert.Equal(this._data, arrayOfData);
        }
    }
}
