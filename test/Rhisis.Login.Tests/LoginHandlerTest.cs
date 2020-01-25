using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Rhisis.Core.Common;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.Database.Repositories;
using Rhisis.Login.Client;
using Rhisis.Login.Core;
using Rhisis.Login.Packets;
using Rhisis.Network.Packets;
using Rhisis.Network.Packets.Login;
using Sylver.HandlerInvoker;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Xunit;

namespace Rhisis.Login.Tests
{
    public class LoginHandlerTest : IDisposable
    {
        private const string BuildVersion = "20072019";
        private const string IncorrectBuildVersion = "23462378";
        private readonly LoginConfiguration _loginConfiguration = new LoginConfiguration
        {
            BuildVersion = BuildVersion
        };
        private readonly Socket _socket;
        private readonly LoginClient _loginClient;

        private readonly Mock<ILogger<LoginHandler>> _loggerMock;
        private readonly Mock<ILoginServer> _loginServerMock;
        private readonly Mock<ICoreServer> _coreServerMock;
        private readonly Mock<IDatabase> _database;
        private readonly Mock<IUserRepository> _databaseUserRepository;
        private readonly Mock<ILoginPacketFactory> _loginPacketFactoryMock;
        private readonly Mock<IOptions<LoginConfiguration>> _loginConfigurationMock;
        private readonly LoginHandler _loginHandler;

        private readonly List<DbUser> _users;

        public LoginHandlerTest()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _loginClient = new LoginClient(_socket);

            _users = new UserGenerator().Generate(100);

            _loggerMock = new Mock<ILogger<LoginHandler>>();
            _loginServerMock = new Mock<ILoginServer>();
            _coreServerMock = new Mock<ICoreServer>();
            _loginPacketFactoryMock = new Mock<ILoginPacketFactory>();

            _loginConfigurationMock = new Mock<IOptions<LoginConfiguration>>();
            _loginConfigurationMock.Setup(x => x.Value).Returns(_loginConfiguration);

            _databaseUserRepository = new Mock<IUserRepository>();
            _databaseUserRepository.Setup(x => x.GetUser(It.IsAny<string>()))
                .Returns<string>(x => _users.AsQueryable().FirstOrDefault(y => y.Username == x));

            _database = new Mock<IDatabase>();
            _database.SetupGet(x => x.Users).Returns(_databaseUserRepository.Object);

            _loginHandler = new LoginHandler(
                _loggerMock.Object,
                _loginConfigurationMock.Object,
                _loginServerMock.Object,
                _database.Object,
                _coreServerMock.Object,
                _loginPacketFactoryMock.Object);

            _loginClient.Initialize(_loginServerMock.Object,
                new Mock<ILogger<LoginClient>>().Object,
                new Mock<IHandlerInvoker>().Object,
                _loginPacketFactoryMock.Object);
        }

        [Fact]
        public void OnCertifyFailedWithIncorrectBuildVersionTest()
        {
            var certifyPacket = new Mock<CertifyPacket>(_loginConfigurationMock.Object);
            certifyPacket.SetupGet(x => x.BuildVersion).Returns(IncorrectBuildVersion);

            _loginHandler.OnCertify(_loginClient, certifyPacket.Object);

            _loginPacketFactoryMock.Verify(x => x.SendLoginError(_loginClient, ErrorType.ILLEGAL_VER), Times.Once());
        }

        public static IEnumerable<object[]> UserData => new List<object[]>
        {
            new object[] { "Rhisis", "CorrectPassword", AuthenticationResult.Success, ErrorType.DEFAULT },
            new object[] { "Shade", "password", AuthenticationResult.AccountDeleted, ErrorType.ILLEGAL_ACCESS },
            new object[] { "Ivillis", "randompassword", AuthenticationResult.BadUsername, ErrorType.FLYFF_ACCOUNT },
            new object[] { "Bubble", "incorrect password", AuthenticationResult.BadPassword, ErrorType.FLYFF_PASSWORD },
        };

        [Theory]
        [MemberData(nameof(UserData))]
        public void OnCertifyTest(string username, string password, AuthenticationResult authenticationResult, ErrorType errorType)
        {
            var certifyPacket = new Mock<CertifyPacket>(_loginConfigurationMock.Object);
            certifyPacket.SetupGet(x => x.BuildVersion).Returns(BuildVersion);
            certifyPacket.SetupGet(x => x.Username).Returns(username);
            certifyPacket.SetupGet(x => x.Password).Returns(password);

            _loginHandler.OnCertify(_loginClient, certifyPacket.Object);

            if (authenticationResult == AuthenticationResult.Success)
            {
                DbUser user = _users.FirstOrDefault(x => x.Username == username && x.Password == password);

                _databaseUserRepository.Verify(x => x.Update(user), Times.Once());
                _database.Verify(x => x.Complete());

                Assert.NotNull(_loginClient.Username);
                Assert.Equal(username, _loginClient.Username);
            }
            else
            {
                Assert.Null(_loginClient.Username);
                _loginPacketFactoryMock.Verify(x => x.SendLoginError(_loginClient, errorType), Times.Once());
            }
        }

        public static IEnumerable<object[]> PingPacketData => new List<object[]>
        {
            new object[] { 42, false },
            new object[] { 24, true }
        };

        [Theory]
        [MemberData(nameof(PingPacketData))]
        public void OnPingTest(int time, bool timeout)
        {
            var pingPacketMock = new Mock<PingPacket>();
            pingPacketMock.SetupGet(x => x.Time).Returns(time);
            pingPacketMock.SetupGet(x => x.IsTimeOut).Returns(timeout);

            _loginHandler.OnPing(_loginClient, pingPacketMock.Object);

            if (!timeout)
            {
                _loginPacketFactoryMock.Verify(x => x.SendPong(_loginClient, pingPacketMock.Object.Time));
            }
        }

        public void Dispose()
        {
            _socket?.Dispose();
            _loginClient?.Dispose();
        }
    }
}
