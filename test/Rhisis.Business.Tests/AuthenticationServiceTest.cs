using Moq;
using Rhisis.Business.Services;
using Rhisis.Core.Common;
using Rhisis.Core.Services;
using Rhisis.Database.Entities;
using Rhisis.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Rhisis.Business.Tests
{
    public class AuthenticationServiceTest : ServiceTestBase<AuthenticationService>
    {
        private readonly IList<DbUser> _users;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly IAuthenticationService _service;

        public AuthenticationServiceTest()
        {
            this._users = this.LoadCollectionFromJson<DbUser>("Resources/Users/UsersDatabase.json");
            this._userRepositoryMock = new Mock<IUserRepository>();
            this._userRepositoryMock
                .Setup(x => x.Get(It.IsAny<Func<DbUser, bool>>()))
                .Returns<Func<DbUser, bool>>(x => this._users.FirstOrDefault(x));

            this.DatabaseMock.SetupProperty(x => x.Users, this._userRepositoryMock.Object);

            this._service = new AuthenticationService(this.DatabaseMock.Object);
        }

        [Theory]
        [InlineData("Michael", "a833300c-be76-4b52-9367-46bd77bec6db", AuthenticationResult.Success)]
        [InlineData("mayer", "c31f44c8-fc9c-4cea-872c-66f91be16589", AuthenticationResult.Success)]
        [InlineData("Eastrall", "a833300qsdnisqd45646djfdec6db", AuthenticationResult.BadUsername)]
        [InlineData("Nita", "a833300c-be76367-46bd77bec6db", AuthenticationResult.BadPassword)]
        [InlineData("Armstrong", "daf059bc-e76c-4b8f-b431-e049617ebb31", AuthenticationResult.AccountDeleted)]
        [InlineData("Stacey", "c96f0d63-f16c-48b2-a1c9-0b7b2af3dee5", AuthenticationResult.AccountDeleted)]
        [InlineData("Weber", "8a946c3f-7261-4871-8ab9-a7cf02a18149", AuthenticationResult.AccountDeleted)]
        public void AuthenticationTest(string username, string password, AuthenticationResult expectedResult)
        {
            AuthenticationResult result = this._service.Authenticate(username, password);

            Assert.Equal(expectedResult, result);
        }
    }
}
