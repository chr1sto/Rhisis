using Microsoft.EntityFrameworkCore;
using Rhisis.Core.Common;
using System;

namespace Rhisis.Database.Test
{
    public class DatabaseTestConstants
    {
        public const string DatabaseName = "Rhisis";
        public const string TestUsername = "rhisis";
        public const string TestUsername2 = "shade";
        public const string TestPassword = "HelloWorld!";
        public const string TestPassword2 = "HelloBubble!";
        public const int TestAuthority = (int)AuthorityType.Administrator;
        public const int TestAuthority2 = (int)AuthorityType.GameMaster;

        public static DbContextOptions<DatabaseContext> GenerateNewDatabaseOptions() 
            => new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
    }
}
