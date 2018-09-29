using Rhisis.Database.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Rhisis.Database.Test.Repositories
{
    public class UserRepositoryTest : IDisposable
    {
        private readonly IDatabase _database;

        public UserRepositoryTest()
        {
            this._database = new Database(DatabaseTestConstants.GenerateNewDatabaseOptions());
        }

        [Fact]
        public void CreateUser()
        {
            int oldUserCount = this._database.Users.Count();
            var user = this._database.Users.Create(this.GenerateUser());
            
            Assert.NotNull(user);
            Assert.Equal(DatabaseTestConstants.TestUsername, user.Username);
            Assert.Equal(DatabaseTestConstants.TestPassword, user.Password);
            Assert.Equal(DatabaseTestConstants.TestAuthority, user.Authority);
            Assert.Equal(0, oldUserCount);
            Assert.Equal(0, this._database.Users.Count());
            this._database.Complete(); // Complete pending operations
            Assert.Equal(1, this._database.Users.Count());
        }

        [Fact]
        public async Task CreateUserAsynchronously()
        {
            int oldUserCount = this._database.Users.Count();
            var user = await this._database.Users.CreateAsync(this.GenerateUser());

            Assert.NotNull(user);
            Assert.Equal(DatabaseTestConstants.TestUsername, user.Username);
            Assert.Equal(DatabaseTestConstants.TestPassword, user.Password);
            Assert.Equal(DatabaseTestConstants.TestAuthority, user.Authority);
            Assert.Equal(0, oldUserCount);
            Assert.Equal(0, this._database.Users.Count());
            await this._database.CompleteAsync();
            Assert.Equal(1, this._database.Users.Count());
        }

        [Fact]
        public void UpdateUser()
        {
            var user = this._database.Users.Create(this.GenerateUser());
            this._database.Complete();
            Assert.NotNull(user);

            user.Username = DatabaseTestConstants.TestUsername2;
            user.Password = DatabaseTestConstants.TestPassword2;
            user.Authority = DatabaseTestConstants.TestAuthority2;

            user = this._database.Users.Update(user);
            this._database.Complete();

            Assert.NotNull(user);
            Assert.Equal(DatabaseTestConstants.TestUsername2, user.Username);
            Assert.Equal(DatabaseTestConstants.TestPassword2, user.Password);
            Assert.Equal(DatabaseTestConstants.TestAuthority2, user.Authority);
            Assert.Equal(1, this._database.Users.Count());
        }

        [Fact]
        public void DeleteUser()
        {
            var user = this._database.Users.Create(this.GenerateUser());
            this._database.Complete();
            Assert.NotNull(user);
            Assert.Equal(1, this._database.Users.Count());

            this._database.Users.Delete(user);
            this._database.Complete();
            Assert.Equal(0, this._database.Users.Count());
        }

        [Fact]
        public void GetUserById()
        {
            var user = this._database.Users.Create(this.GenerateUser());
            this._database.Complete();
            Assert.NotNull(user);

            var sameUser = this._database.Users.Get(user.Id);

            Assert.Equal(user.Id, sameUser.Id);
            Assert.Equal(user.Username, sameUser.Username);
            Assert.Equal(user.Password, sameUser.Password);
            Assert.Equal(user.Authority, sameUser.Authority);
        }

        [Fact]
        public void GetAllUsers()
        {
            var users = this.GenerateUsers(10);

            foreach (var user in users)
                this._database.Users.Create(user);

            this._database.Complete();
            var dbUsers = this._database.Users.GetAll();

            Assert.Equal(users.Count(), dbUsers.Count());
            for (int i = 0; i < dbUsers.Count(); i++)
            {
                Assert.Equal(users[i].Username, dbUsers.ElementAt(i).Username);
                Assert.Equal(users[i].Password, dbUsers.ElementAt(i).Password);
                Assert.Equal(users[i].Authority, dbUsers.ElementAt(i).Authority);
            }
        }

        [Fact]
        public void GetUserWithPredicate()
        {
            this._database.Users.Create(new DbUser
            {
                Username = DatabaseTestConstants.TestUsername,
                Password = DatabaseTestConstants.TestPassword,
                Authority = DatabaseTestConstants.TestAuthority
            });
            this._database.Users.Create(new DbUser
            {
                Username = DatabaseTestConstants.TestUsername2,
                Password = DatabaseTestConstants.TestPassword2,
                Authority = DatabaseTestConstants.TestAuthority2
            });
            this._database.Complete();

            var user = this._database.Users.Get(x => x.Username == DatabaseTestConstants.TestUsername2);

            Assert.NotNull(user);
            Assert.Equal(DatabaseTestConstants.TestUsername2, user.Username);
            Assert.Equal(DatabaseTestConstants.TestPassword2, user.Password);
            Assert.Equal(DatabaseTestConstants.TestAuthority2, user.Authority);
        }

        [Fact]
        public void GetUsersWithPredicate()
        {
            var users = this.GenerateUsers(10);

            foreach (var user in users)
                this._database.Users.Create(user);

            this._database.Users.Create(new DbUser
            {
                Username = DatabaseTestConstants.TestUsername,
                Password = DatabaseTestConstants.TestPassword,
                Authority = DatabaseTestConstants.TestAuthority2
            });
            this._database.Users.Create(new DbUser
            {
                Username = DatabaseTestConstants.TestUsername2,
                Password = DatabaseTestConstants.TestPassword2,
                Authority = DatabaseTestConstants.TestAuthority2
            });
            this._database.Complete();

            var queryUsers = this._database.Users.GetAll(x => x.Authority == DatabaseTestConstants.TestAuthority2);

            Assert.NotEmpty(queryUsers);
            Assert.Equal(2, queryUsers.Count());

            for (int i = 0; i < queryUsers.Count(); i++)
            {
                Assert.Equal(DatabaseTestConstants.TestAuthority2, queryUsers.ElementAt(i).Authority);
            }
        }

        private DbUser GenerateUser()
        {
            return new DbUser
            {
                Username = DatabaseTestConstants.TestUsername,
                Password = DatabaseTestConstants.TestPassword,
                Authority = DatabaseTestConstants.TestAuthority
            };
        }

        private DbUser[] GenerateUsers(int count)
        {
            var users = new DbUser[count];

            for (int i = 0; i < count; i++)
            {
                users[i] = new DbUser
                {
                    Username = Guid.NewGuid().ToString(),
                    Password = Guid.NewGuid().ToString(),
                    Authority = DatabaseTestConstants.TestAuthority
                };
            }

            return users;
        }

        public void Dispose()
        {
            this._database.Dispose();
        }
    }
}
