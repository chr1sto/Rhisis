using Microsoft.EntityFrameworkCore;

namespace Rhisis.Database.Test
{
    public class DatabaseTestConstants
    {
        public const string DatabaseName = "Rhisis";
        public const string TestUsername = "eastrall";
        public const string TestPassword = "HelloWorld!";
        public const int TestAuthority = 100;

        public static readonly DbContextOptions<DatabaseContext> DatabaseOptions = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: DatabaseName)
            .Options;
    }
}
