using Ether.Network.Packets;
using NLog;
using Rhisis.Core.Common;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Services;
using Rhisis.Login.Packets;
using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.Network.Packets.Login;

namespace Rhisis.Login
{
    public static class LoginHandler
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

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
            var cryptographyService = DependencyContainer.Instance.Resolve<ICryptographyService>();
            var certifyPacket = new CertifyPacket(packet, client.ServerConfiguration.PasswordEncryption);
            string password = null;

            if (certifyPacket.BuildVersion != client.ServerConfiguration.BuildVersion)
            {
                AuthenticationFailed(client, ErrorType.CERT_GENERAL, "bad client build version");
                return;
            }

            if (client.ServerConfiguration.PasswordEncryption)
            {
                byte[] encryptionKey = cryptographyService.BuildEncryptionKeyFromString(client.ServerConfiguration.EncryptionKey, 16);

                password = cryptographyService.Decrypt(certifyPacket.EncryptedPassword, encryptionKey);
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
                case AuthenticationResult.Success:
                    LoginPacketFactory.SendServerList(client, certifyPacket.Username, client.ClustersConnected);
                    client.SetClientUsername(certifyPacket.Username);
                    Logger.Info($"User '{client.Username}' logged succesfully from {client.RemoteEndPoint}.");
                    break;
                default:
                    break;
            }
        }

        private static void AuthenticationFailed(LoginClient client, ErrorType error, string reason)
        {
            Logger.Warn($"Unable to authenticate user from {client.RemoteEndPoint}. Reason: {reason}");
            LoginPacketFactory.SendLoginError(client, error);
            client.Disconnect();
        }
    }
}