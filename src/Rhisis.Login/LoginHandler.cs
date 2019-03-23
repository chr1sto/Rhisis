using Ether.Network.Packets;
using Microsoft.Extensions.Logging;
using Rhisis.Core.Common;
using Rhisis.Core.Cryptography;
using Rhisis.Core.DependencyInjection;
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
    public class LoginHandler
    {
        private static readonly ILogger<LoginHandler> Logger = DependencyContainer.Instance.Resolve<ILogger<LoginHandler>>();

        [PacketHandler(PacketType.PING)]
        public static void OnPing(LoginClient client, INetPacketStream packet)
        {
            var pak = new PingPacket(packet);

            if (!pak.IsTimeOut)
                CommonPacketFactory.SendPong(client, pak.Time);
        }

        [PacketHandler(PacketType.CERTIFY)]
        public static void OnLogin(LoginClient client, INetPacketStream packet)
        {
            var loginServer = DependencyContainer.Instance.Resolve<ILoginServer>();
            var configuration = DependencyContainer.Instance.Resolve<LoginConfiguration>();
            var certifyPacket = new CertifyPacket(packet, configuration.PasswordEncryption);
            string password = null;

            if (certifyPacket.BuildVersion != configuration.BuildVersion)
            {
                AuthenticationFailed(client, ErrorType.CERT_GENERAL, "bad client build version");
                return;
            }

            if (configuration.PasswordEncryption)
            {
                byte[] encryptionKey = Aes.BuildEncryptionKeyFromString(configuration.EncryptionKey, 16);

                password = Aes.DecryptByteArray(certifyPacket.EncryptedPassword, encryptionKey);
            }
            else
            {
                password = packet.Read<string>();
            }

            var authenticationService = DependencyContainer.Instance.Resolve<IAuthenticationService>();
            var authenticationResult = authenticationService.Authenticate(certifyPacket.Username, password);

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
                    if (loginServer.IsClientConnected(certifyPacket.Username))
                    {
                        AuthenticationFailed(client, ErrorType.DUPLICATE_ACCOUNT, "client already connected", disconnectClient: false);
                        return;
                    }
                    
                    using (var database = DependencyContainer.Instance.Resolve<IDatabase>())
                    {
                        var user = database.Users.Get(x => x.Username.Equals(certifyPacket.Username, StringComparison.OrdinalIgnoreCase));

                        if (user == null)
                        {
                            AuthenticationFailed(client, ErrorType.ILLEGAL_ACCESS, "Cannot find user in database");
                            Logger.LogCritical($"Cannot find user '{certifyPacket.Username}' in database to update last connection time.");
                            return;
                        }

                        user.LastConnectionTime = DateTime.UtcNow;
                        database.Users.Update(user);
                        database.Complete();
                    }

                    LoginPacketFactory.SendServerList(client, certifyPacket.Username, loginServer.GetConnectedClusters());
                    client.SetClientUsername(certifyPacket.Username);
                    Logger.LogInformation($"User '{client.Username}' logged succesfully from {client.RemoteEndPoint}.");
                    break;
                default:
                    break;
            }
        }

        [PacketHandler(PacketType.CLOSE_EXISTING_CONNECTION)]
        public static void OnCloseExistingConnection(LoginClient client, INetPacketStream packet)
        {
            var loginServer = DependencyContainer.Instance.Resolve<ILoginServer>();
            var closeConnectionPacket = new CloseConnectionPacket(packet);
            var otherConnectedClient = loginServer.GetClientByUsername(closeConnectionPacket.Username);

            if (otherConnectedClient == null)
            {
                Logger.LogWarning($"Cannot find user with username '{closeConnectionPacket.Username}'.");
                return;
            }

            // TODO: disconnect client from server and ISC.
        }

        private static void AuthenticationFailed(LoginClient client, ErrorType error, string reason, bool disconnectClient = true)
        {
            Logger.LogWarning($"Unable to authenticate user from {client.RemoteEndPoint}. Reason: {reason}");
            LoginPacketFactory.SendLoginError(client, error);

            if (disconnectClient)
                client.Disconnect();
        }
    }
}