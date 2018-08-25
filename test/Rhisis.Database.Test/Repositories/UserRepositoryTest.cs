using Rhisis.Database.Entities;
using Rhisis.Database.Interfaces;
using Rhisis.Database.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Rhisis.Database.Test.Repositories
{
    public class UserRepositoryTest : IDisposable
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IUserRepository _userRepository;
        

        public UserRepositoryTest()
        {
            this._databaseContext = new DatabaseContext(DatabaseTestConstants.GenerateNewDatabaseOptions());
            this._userRepository = new UserRepository(this._databaseContext);
        }

        [Fact]
        public void CreateUser()
        {
            int oldUserCount = this._userRepository.Count();
            var user = this._userRepository.Create(this.GenerateUser());
            
            Assert.NotNull(user);
            Assert.Equal(DatabaseTestConstants.TestUsername, user.Username);
            Assert.Equal(DatabaseTestConstants.TestPassword, user.Password);
            Assert.Equal(DatabaseTestConstants.TestAuthority, user.Authority);
            Assert.Equal(0, oldUserCount);
            Assert.Equal(1, this._userRepository.Count());
        }

        [Fact]
        public async Task CreateUserAsynchronously()
        {
            int oldUserCount = this._userRepository.Count();
            var user = await this._userRepository.CreateAsync(this.GenerateUser());

            Assert.NotNull(user);
            Assert.Equal(DatabaseTestConstants.TestUsername, user.Username);
            Assert.Equal(DatabaseTestConstants.TestPassword, user.Password);
            Assert.Equal(DatabaseTestConstants.TestAuthority, user.Authority);
            Assert.Equal(0, oldUserCount);
            Assert.Equal(1, this._userRepository.Count());
        }

        [Fact]
        public void UpdateUser()
        {
            var user = this._userRepository.Create(this.GenerateUser());
            Assert.NotNull(user);

            user.Username = DatabaseTestConstants.TestUsername2;
            user.Password = DatabaseTestConstants.TestPassword2;
            user.Authority = DatabaseTestConstants.TestAuthority2;

            user = this._userRepository.Update(user);

            Assert.NotNull(user);
            Assert.Equal(DatabaseTestConstants.TestUsername2, user.Username);
            Assert.Equal(DatabaseTestConstants.TestPassword2, user.Password);
            Assert.Equal(DatabaseTestConstants.TestAuthority2, user.Authority);
            Assert.Equal(1, this._userRepository.Count());
        }

        [Fact]
        public async Task UpdateUserAsynchronously()
        {
            var user = await this._userRepository.CreateAsync(this.GenerateUser());
            Assert.NotNull(user);

            user.Username = DatabaseTestConstants.TestUsername2;
            user.Password = DatabaseTestConstants.TestPassword2;
            user.Authority = DatabaseTestConstants.TestAuthority2;

            user = await this._userRepository.UpdateAsync(user);

            Assert.NotNull(user);
            Assert.Equal(DatabaseTestConstants.TestUsername2, user.Username);
            Assert.Equal(DatabaseTestConstants.TestPassword2, user.Password);
            Assert.Equal(DatabaseTestConstants.TestAuthority2, user.Authority);
            Assert.Equal(1, this._userRepository.Count());
        }

        [Fact]
        public void DeleteUser()
        {
            var user = this._userRepository.Create(this.GenerateUser());
            Assert.NotNull(user);
            Assert.Equal(1, this._userRepository.Count());

            this._userRepository.Delete(user);
            Assert.Equal(0, this._userRepository.Count());
        }

        [Fact]
        public async Task DeleteUserAsynchronously()
        {
            var user = await this._userRepository.CreateAsync(this.GenerateUser());
            Assert.NotNull(user);
            Assert.Equal(1, this._userRepository.Count());

            await this._userRepository.DeleteAsync(user);
            Assert.Equal(0, this._userRepository.Count());
        }

        [Fact]
        public void GetUserById()
        {
            var user = this._userRepository.Create(this.GenerateUser());
            Assert.NotNull(user);

            var sameUser = this._userRepository.Get(user.Id);

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
                this._userRepository.Create(user);

            var dbUsers = this._userRepository.GetAll();

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
            this._userRepository.Create(new DbUser
            {
                Username = DatabaseTestConstants.TestUsername,
                Password = DatabaseTestConstants.TestPassword,
                Authority = DatabaseTestConstants.TestAuthority
            });
            this._userRepository.Create(new DbUser
            {
                Username = DatabaseTestConstants.TestUsername2,
                Password = DatabaseTestConstants.TestPassword2,
                Authority = DatabaseTestConstants.TestAuthority2
            });

            var user = this._userRepository.Get(x => x.Username == DatabaseTestConstants.TestUsername2);

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
                this._userRepository.Create(user);

            this._userRepository.Create(new DbUser
            {
                Username = DatabaseTestConstants.TestUsername,
                Password = DatabaseTestConstants.TestPassword,
                Authority = DatabaseTestConstants.TestAuthority2
            });
            this._userRepository.Create(new DbUser
            {
                Username = DatabaseTestConstants.TestUsername2,
                Password = DatabaseTestConstants.TestPassword2,
                Authority = DatabaseTestConstants.TestAuthority2
            });

            var queryUsers = this._userRepository.GetAll(x => x.Authority == DatabaseTestConstants.TestAuthority2);

            Assert.NotEmpty(queryUsers);
            Assert.Equal(2, queryUsers.Count());

            for (int i = 0; i < queryUsers.Count(); i++)
            {
                Assert.Equal(DatabaseTestConstants.TestAuthority2, queryUsers.ElementAt(i).Authority);
            }
        }

        public DbUser GenerateUser()
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
            this._databaseContext.Dispose();
        }
    }
}
