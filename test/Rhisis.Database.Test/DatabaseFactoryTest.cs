using Xunit;

namespace Rhisis.Database.Test
{
    public class DatabaseFactoryTest
    {
        private readonly DatabaseFactory _dbFactory;

        public DatabaseFactoryTest()
        {
            this._dbFactory = new DatabaseFactory();
        }

        [Fact(DisplayName = "Create a DbContext from factory")]
        public void CreateDbContextFromFactory()
        {
            DatabaseContext context = this._dbFactory.CreateDbContext(DatabaseTestConstants.DatabaseOptions);

            Assert.NotNull(context);
            Assert.NotNull(context.Users);
            Assert.NotNull(context.Characters);
            Assert.NotNull(context.Items);
        }
    }
}
