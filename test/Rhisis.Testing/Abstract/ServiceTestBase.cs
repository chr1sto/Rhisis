using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Rhisis.Database;
using System;

namespace Rhisis.Testing.Abstract
{
    public class ServiceTestBase<TService> : IDisposable
        where TService : class
    {
        protected Mock<ILogger<TService>> LoggerMock { get; }

        protected TService Service { get; set; }

        protected Faker Faker { get; }

        protected IRhisisDatabase Database { get; }

        /// <summary>
        /// Creates a new <see cref="ServiceTestBase{TService}"/>.
        /// </summary>
        protected ServiceTestBase()
        {
            LoggerMock = new Mock<ILogger<TService>>();
            Faker = new Faker();

            var options = new DbContextOptionsBuilder<RhisisDatabaseContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            Database = new RhisisDatabaseContext(options);
        }

        public virtual void Dispose()
        {
            Database.Dispose();
        }
    }
}
