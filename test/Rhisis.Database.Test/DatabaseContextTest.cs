using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace Rhisis.Database.Test
{
    public class DatabaseContextTest
    {
        [Fact(DisplayName = "Create In Memory context")]
        public void CreateInMemoryContext()
        {
            var databaseOptions = DatabaseTestConstants.GenerateNewDatabaseOptions();

            using (var context = new DatabaseContext(databaseOptions))
            {
                context.Users.Add(new Entities.DbUser
                {
                    Username = DatabaseTestConstants.TestUsername,
                    Password = DatabaseTestConstants.TestPassword,
                    Authority = DatabaseTestConstants.TestAuthority
                });
                context.SaveChanges();
            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new DatabaseContext(databaseOptions))
            {
                Assert.Equal(1, context.Users.Count());
                Assert.Equal(DatabaseTestConstants.TestUsername, context.Users.Single().Username);
                Assert.Equal(DatabaseTestConstants.TestPassword, context.Users.Single().Password);
                Assert.Equal(DatabaseTestConstants.TestAuthority, context.Users.Single().Authority);
            }
        }
    }
}
