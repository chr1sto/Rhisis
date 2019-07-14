using Ether.Network.Packets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Core.Common;
using Rhisis.Core.Cryptography;
using Rhisis.Core.Handlers.Attributes;
using Rhisis.Core.Services;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Database;
using Rhisis.Login.Packets;
using Rhisis.Network;
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

        public LoginHandler(ILogger<LoginHandler> logger, IOptions<LoginConfiguration> loginConfiguration, ILoginServer loginServer, IDatabase database, IAuthenticationService authenticationService)
        {
            this._logger = logger;
            this._loginConfiguration = loginConfiguration.Value;
            this._loginServer = loginServer;
            this._database = database;
            this._authenticationService = authenticationService;
        }

        [HandlerAction(PacketType.PING)]
        public void OnPing(LoginClient client, INetPacketStream packet)
        {
            var pak = new PingPacket(packet);

            if (!pak.IsTimeOut)
                CommonPacketFactory.SendPong(client, pak.Time);
        }

        [HandlerAction(PacketType.CERTIFY)]
        public void OnCertify(LoginClient client, INetPacketStream packet)
        {
            var certifyPacket = new CertifyPacket(packet, this._loginConfiguration.PasswordEncryption);
            string password = null;

            if (certifyPacket.BuildVersion != this._loginConfiguration.BuildVersion)
            {
                AuthenticationFailed(client, ErrorType.CERT_GENERAL, "bad client build version");
                return;
            }

            if (this._loginConfiguration.PasswordEncryption)
            {
                byte[] encryptionKey = Aes.BuildEncryptionKeyFromString(this._loginConfiguration.EncryptionKey, 16);

                password = Aes.DecryptByteArray(certifyPacket.EncryptedPassword, encryptionKey);
            }
            else
            {
                password = packet.Read<string>();
            }

            var authenticationResult = this._authenticationService.Authenticate(certifyPacket.Username, password);

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

                    LoginPacketFactory.SendServerList(client, certifyPacket.Username, this._loginServer.GetConnectedClusters());
                    client.SetClientUsername(certifyPacket.Username);
                    this._logger.LogInformation($"User '{client.Username}' logged succesfully from {client.RemoteEndPoint}.");
                    break;
                default:
                    break;
            }
        }

        [PacketHandler(PacketType.CLOSE_EXISTING_CONNECTION)]
        public void OnCloseExistingConnection(LoginClient client, INetPacketStream packet)
        {
            var closeConnectionPacket = new CloseConnectionPacket(packet);
            var otherConnectedClient = this._loginServer.GetClientByUsername(closeConnectionPacket.Username);

            if (otherConnectedClient == null)
            {
                this._logger.LogWarning($"Cannot find user with username '{closeConnectionPacket.Username}'.");
                return;
            }

            // TODO: disconnect client from server and ISC.
        }

        private void AuthenticationFailed(LoginClient client, ErrorType error, string reason, bool disconnectClient = true)
        {
            this._logger.LogWarning($"Unable to authenticate user from {client.RemoteEndPoint}. Reason: {reason}");
            LoginPacketFactory.SendLoginError(client, error);

            if (disconnectClient)
                client.Disconnect();
        }
    }
}