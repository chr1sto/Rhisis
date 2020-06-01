using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Core.Common;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.Login.Client;
using Rhisis.Login.Core;
using Rhisis.Login.Packets;
using Rhisis.Network.Packets;
using Rhisis.Network.Packets.Login;
using Sylver.HandlerInvoker.Attributes;
using Sylver.Network.Data;
using System;
using System.Linq;

namespace Rhisis.Login.Handlers
{
    [Handler]
    public class LoginHandler
    {
        private readonly ILogger<LoginHandler> _logger;
        private readonly LoginConfiguration _loginConfiguration;
        private readonly ILoginServer _loginServer;
        private readonly IRhisisDatabase _database;
        private readonly ICoreServer _coreServer;
        private readonly ILoginPacketFactory _loginPacketFactory;

        /// <summary>
        /// Creates a new <see cref="LoginHandler"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="loginConfiguration">Login server configuration.</param>
        /// <param name="loginServer">Login server instance.</param>
        /// <param name="database">Database service.</param>
        /// <param name="coreServer">Core server.</param>
        /// <param name="loginPacketFactory">Login server packet factory.</param>
        public LoginHandler(ILogger<LoginHandler> logger, IOptions<LoginConfiguration> loginConfiguration, ILoginServer loginServer, IRhisisDatabase database, ICoreServer coreServer, ILoginPacketFactory loginPacketFactory)
        {
            _logger = logger;
            _loginConfiguration = loginConfiguration.Value;
            _loginServer = loginServer;
            _database = database;
            _coreServer = coreServer;
            _loginPacketFactory = loginPacketFactory;
        }

        /// <summary>
        /// Handles the PING packet.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <param name="pingPacket">Ping packet.</param>
        [HandlerAction(PacketType.PING)]
        public void OnPing(ILoginClient client, PingPacket pingPacket)
        {
            if (!pingPacket.IsTimeOut)
            {
                _loginPacketFactory.SendPong(client, pingPacket.Time);
            }
        }

        /// <summary>
        /// Certifies and authenticates a user.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <param name="certifyPacket">Certify packet.</param>
        [HandlerAction(PacketType.CERTIFY)]
        public void OnCertify(ILoginClient client, CertifyPacket certifyPacket)
        {
            if (certifyPacket.BuildVersion != _loginConfiguration.BuildVersion)
            {
                AuthenticationFailed(client, ErrorType.ILLEGAL_VER, "bad client build version");
                return;
            }

            DbUser user = _database.Users.FirstOrDefault(x => x.Username.Equals(certifyPacket.Username));
            AuthenticationResult authenticationResult = Authenticate(user, certifyPacket.Password);

            switch (authenticationResult)
            {
                case AuthenticationResult.BadUsername:
                    AuthenticationFailed(client, ErrorType.FLYFF_ACCOUNT, "bad username");
                    break;
                case AuthenticationResult.BadPassword:
                    AuthenticationFailed(client, ErrorType.FLYFF_PASSWORD, "bad password");
                    break;
                case AuthenticationResult.AccountSuspended:
                    // TODO
                    break;
                case AuthenticationResult.AccountTemporarySuspended:
                    // TODO
                    break;
                case AuthenticationResult.AccountDeleted:
                    AuthenticationFailed(client, ErrorType.ILLEGAL_ACCESS, "logged in with deleted account");
                    break;
                case AuthenticationResult.Success:
                    if (_loginServer.IsClientConnected(certifyPacket.Username))
                    {
                        AuthenticationFailed(client, ErrorType.DUPLICATE_ACCOUNT, "client already connected");
                        return;
                    }

                    user.LastConnectionTime = DateTime.UtcNow;
                    _database.Users.Update(user);
                    _database.SaveChanges();

                    _loginPacketFactory.SendServerList(client, certifyPacket.Username, _coreServer.GetConnectedClusters());
                    client.SetClientUsername(certifyPacket.Username);
                    _logger.LogInformation($"User '{client.Username}' logged succesfully from {client.Socket.RemoteEndPoint}.");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Disconnects the client when it receives an error packet.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="_"></param>
        [HandlerAction(PacketType.ERROR)]
        public void OnError(ILoginClient client, INetPacketStream _)
        {
            client.Disconnect();
        }

        /// <summary>
        /// Closes an existing connection.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <param name="closeConnectionPacket">Close connection packet.</param>
        [HandlerAction(PacketType.CLOSE_EXISTING_CONNECTION)]
        public void OnCloseExistingConnection(ILoginClient _, CloseConnectionPacket closeConnectionPacket)
        {
            var otherConnectedClient = _loginServer.GetClientByUsername(closeConnectionPacket.Username);

            if (otherConnectedClient == null)
            {
                _logger.LogWarning($"Cannot find user with username '{closeConnectionPacket.Username}'.");
                return;
            }

            // TODO: disconnect client from server and ISC.
        }

        /// <summary>
        /// Sends an authentication failed packet to the client.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <param name="error">Authentication error type.</param>
        /// <param name="reason">Authentication error reason.</param>
        private void AuthenticationFailed(ILoginClient client, ErrorType error, string reason)
        {
            _logger.LogWarning($"Unable to authenticate user from {client.Socket.RemoteEndPoint}. Reason: {reason}");
            _loginPacketFactory.SendLoginError(client, error);
        }

        /// <summary>
        /// Authenticates a user.
        /// </summary>
        /// <param name="user">Database user.</param>
        /// <param name="password">Password</param>
        /// <returns>Authentication result</returns>
        private AuthenticationResult Authenticate(DbUser user, string password)
        {
            if (user == null)
            {
                return AuthenticationResult.BadUsername;
            }

            if (user.IsDeleted)
            {
                return AuthenticationResult.AccountDeleted;
            }

            if (!user.Password.Equals(password, StringComparison.OrdinalIgnoreCase))
            {
                return AuthenticationResult.BadPassword;
            }

            return AuthenticationResult.Success;
        }
    }
}