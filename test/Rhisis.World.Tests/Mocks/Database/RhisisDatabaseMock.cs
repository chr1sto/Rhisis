using Moq;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Rhisis.World.Tests.Mocks.Database
{
    internal class RhisisDatabaseMock : Mock<IDatabase>
    {
        public RhisisDatabaseMock()
        {
        }
    }

    internal class ItemRepositoryMock : Mock<IItemRepository>
    {
        private readonly IEnumerable<DbItem> _items;

        public ItemRepositoryMock(IEnumerable<DbItem> items)
        {
            _items = items;
            Setup(x => x.GetAll()).Returns(_items);
            Setup(x => x.GetAll(It.IsAny<Expression<Func<DbItem, bool>>>())).Returns<Expression<Func<DbItem, bool>>>(x => GetAll(x));
        }

        private IEnumerable<DbItem> GetAll(Expression<Func<DbItem, bool>> predicate) => _items.AsQueryable().Where(predicate);
    }
}
