using Microsoft.Extensions.Configuration;
using Moq;
using Rhisis.Core.Business.Services;
using Rhisis.Core.Common;
using Rhisis.Core.Models;
using Rhisis.Core.Services;
using Rhisis.Database.Entities;
using Rhisis.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Rhisis.Business.Tests
{
    public class UserServiceTest : ServiceTestBase<UserService>
    {
        private readonly IList<DbUser> _users;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IConfigurationSection> _configurationSectionMock;
        private readonly IUserService _service;

        public UserServiceTest()
        {
            this._users = this.LoadCollectionFromJson<DbUser>("Resources/Users/UsersDatabase.json");

            this._userRepositoryMock = new Mock<IUserRepository>();
            this._userRepositoryMock
                .Setup(x => x.HasAny(It.IsAny<Func<DbUser, bool>>()))
                .Returns<Func<DbUser, bool>>(x => this._users.Any(x));
            this._userRepositoryMock
                .Setup(x => x.Create(It.IsAny<DbUser>()))
                .Returns<DbUser>(x =>
                {
                    x.Id = this._users.Max(y => y.Id) + 1;
                    this._users.Add(x);
                    return x;
                });

            this._configurationSectionMock = new Mock<IConfigurationSection>();
            this._configurationSectionMock
                .SetupGet(x => x.Value)
                .Returns("some_salt");

            this._configurationMock = new Mock<IConfiguration>();
            this._configurationMock
                .Setup(x => x.GetSection(It.IsAny<string>()))
                .Returns<string>(x => this._configurationSectionMock.Object);

            this.DatabaseMock.SetupProperty(x => x.Users, this._userRepositoryMock.Object);
            
            this._service = new UserService(this.Logger.Object, this.DatabaseMock.Object, 
                this._configurationMock.Object, new CryptographyService());
        }

        [Theory]
        [InlineData(2, true)]
        [InlineData(5, true)]
        [InlineData(-1, false)]
        [InlineData(13, false)]
        public void HasUserWithIdTest(int userId, bool exists)
        {
            bool userExists = this._service.HasUser(userId);

            Assert.Equal(exists, userExists);
        }

        [Theory]
        [InlineData("Nita", true)]
        [InlineData("Reeves", true)]
        [InlineData("Eastrall", false)]
        [InlineData(null, false)]
        public void HasUserWithUsername(string username, bool exists)
        {
            bool userExists = this._service.HasUser(username);

            Assert.Equal(exists, userExists);
        }

        [Fact]
        public void CreateUserTest()
        {
            var userToCreate = new UserRegisterModel
            {
                Username = "eastrall",
                Password = "hello_world_test",
                PasswordConfirmation = "hello_world_test"
            };

            this._service.CreateUser(userToCreate);

            Assert.Equal(11, this._users.Count);

            var user = this._users.LastOrDefault();

            Assert.NotNull(user);
            Assert.Equal(userToCreate.Username, user.Username);
            Assert.Equal("cbfa1820c9e58e4cf886f22775672a7f", user.Password);
            Assert.Equal((int)AuthorityType.Player, user.Authority);
            Assert.Equal(10, user.Id);
        }

        [Fact]
        public void CreateUserWithInvalidUsernameTest()
        {
            var userToCreate = new UserRegisterModel
            {
                Username = "Twila",
                Password = "hello_world_test",
                PasswordConfirmation = "hello_world_test"
            };

            var exception = Assert.Throws<InvalidOperationException>(() => this._service.CreateUser(userToCreate));
            Assert.NotNull(exception);
            Assert.Equal("username already taken", exception.Message);
        }

        [Fact]
        public void CreateUserWithInvalidPasswords()
        {
            var userToCreate = new UserRegisterModel
            {
                Username = "eastrall",
                Password = "hello_world_test_not_match",
                PasswordConfirmation = "hello_world_test"
            };

            var exception = Assert.Throws<InvalidOperationException>(() => this._service.CreateUser(userToCreate));
            Assert.NotNull(exception);
            Assert.Equal("passwords doesn't match", exception.Message);
        }

        [Fact]
        public void CreateUserWithInvalidInputsTest()
        {
            var userToCreate = new UserRegisterModel();

            Assert.Throws<ArgumentNullException>(() => this._service.CreateUser(userToCreate));
        }
    }
}
