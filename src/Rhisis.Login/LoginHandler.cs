using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Core.Common;
using Rhisis.Core.Handlers.Attributes;
using Rhisis.Core.Services;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Database;
using Rhisis.Login.Core;
using Rhisis.Login.Packets;
using Rhisis.Network.Packets;
using Rhisis.Network.Packets.Login;
using System;

namespace Rhisis.Login
{
    [Handler]
    public class LoginHandler
    {
        private readonly ILogger<LoginHandler> _logger;
        private readonly LoginConfiguration _loginConfiguration;
        private readonly ILoginServer _loginServer;
        private readonly IDatabase _database;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICoreServer _coreServer;

        /// <summary>
        /// Creates a new <see cref="LoginHandler"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="loginConfiguration">Login server configuration.</param>
        /// <param name="loginServer">Login server instance.</param>
        /// <param name="database">Database service.</param>
        /// <param name="authenticationService">Authentication service.</param>
        /// <param name="coreServer">Core server.</param>
        public LoginHandler(ILogger<LoginHandler> logger, IOptions<LoginConfiguration> loginConfiguration, ILoginServer loginServer, IDatabase database, IAuthenticationService authenticationService, ICoreServer coreServer)
        {
            this._logger = logger;
            this._loginConfiguration = loginConfiguration.Value;
            this._loginServer = loginServer;
            this._database = database;
            this._authenticationService = authenticationService;
            this._coreServer = coreServer;
        }

        /// <summary>
        /// Handles the PING packet.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <param name="pingPacket">Ping packet.</param>
        [HandlerAction(PacketType.PING)]
        public void OnPing(LoginClient client, PingPacket pingPacket)
        {
            if (!pingPacket.IsTimeOut)
            {
                CommonPacketFactory.SendPong(client, pingPacket.Time);
            }
        }

        /// <summary>
        /// Certifies and authenticates a user.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <param name="certifyPacket">Certify packet.</param>
        [HandlerAction(PacketType.CERTIFY)]
        public void OnCertify(LoginClient client, CertifyPacket certifyPacket)
        {
            if (certifyPacket.BuildVersion != this._loginConfiguration.BuildVersion)
            {
                AuthenticationFailed(client, ErrorType.CERT_GENERAL, "bad client build version");
                return;
            }

            AuthenticationResult authenticationResult = this._authenticationService.Authenticate(certifyPacket.Username, certifyPacket.Password);

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
                    if (this._loginServer.IsClientConnected(certifyPacket.Username))
                    {
                        AuthenticationFailed(client, ErrorType.DUPLICATE_ACCOUNT, "client already connected", disconnectClient: false);
                        return;
                    }

                    var user = this._database.Users.Get(x => x.Username.Equals(certifyPacket.Username, StringComparison.OrdinalIgnoreCase));

                    if (user == null)
                    {
                        AuthenticationFailed(client, ErrorType.ILLEGAL_ACCESS, "Cannot find user in database");
                        this._logger.LogCritical($"Cannot find user '{certifyPacket.Username}' in database to update last connection time.");
                        return;
                    }

                    user.LastConnectionTime = DateTime.UtcNow;
                    this._database.Users.Update(user);
                    this._database.Complete();

                    LoginPacketFactory.SendServerList(client, certifyPacket.Username, this._coreServer.GetConnectedClusters());
                    client.SetClientUsername(certifyPacket.Username);
                    this._logger.LogInformation($"User '{client.Username}' logged succesfully from {client.RemoteEndPoint}.");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Closes an existing connection.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <param name="closeConnectionPacket">Close connection packet.</param>
        [HandlerAction(PacketType.CLOSE_EXISTING_CONNECTION)]
        public void OnCloseExistingConnection(LoginClient client, CloseConnectionPacket closeConnectionPacket)
        {
            var otherConnectedClient = this._loginServer.GetClientByUsername(closeConnectionPacket.Username);

            if (otherConnectedClient == null)
            {
                this._logger.LogWarning($"Cannot find user with username '{closeConnectionPacket.Username}'.");
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
        /// <param name="disconnectClient">A boolean value that indicates if we disconnect the client or not.</param>
        private void AuthenticationFailed(LoginClient client, ErrorType error, string reason, bool disconnectClient = true)
        {
            this._logger.LogWarning($"Unable to authenticate user from {client.RemoteEndPoint}. Reason: {reason}");
            LoginPacketFactory.SendLoginError(client, error);

            if (disconnectClient)
                client.Disconnect();
        }
    }
}