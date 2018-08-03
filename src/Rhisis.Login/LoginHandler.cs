using Ether.Network.Packets;
using NLog;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.Core.Network.Packets.Login;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.Login.Packets;
using System;

namespace Rhisis.Login
{
    public static class LoginHandler
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        [PacketHandler(PacketType.PING)]
        public static void OnPing(LoginClient client, INetPacketStream packet)
        {
            var pingPacket = new PingPacket(packet);

            if (!pingPacket.IsTimeOut)
                CommonPacketFactory.SendPong(client, pingPacket.Time);
        }

        [PacketHandler(PacketType.CERTIFY)]
        public static void OnLogin(LoginClient client, INetPacketStream packet)
        {
            var certify = new CertifyPacket(packet, client.Configuration.PasswordEncryption, client.Configuration.EncryptionKey);

            if (certify.BuildVersion != client.Configuration.BuildVersion)
            {
                Logger.Warn($"User '{certify.Username}' logged in with bad build version.");
                LoginPacketFactory.SendLoginError(client, ErrorType.CERT_GENERAL);
                client.Disconnect();
                return;
            }

            using (DatabaseContext db = DatabaseService.GetContext())
            {
                User user = db.Users.Get(x => x.Username.Equals(certify.Username, StringComparison.OrdinalIgnoreCase));

                if (user == null)
                {
                    Logger.Warn($"User '{certify.Username}' logged in with bad credentials. (Bad username)");
                    LoginPacketFactory.SendLoginError(client, ErrorType.FLYFF_ACCOUNT);
                    client.Disconnect();
                    return;
                }

                if (!user.Password.Equals(certify.Password, StringComparison.OrdinalIgnoreCase))
                {
                    Logger.Warn($"User '{certify.Username}' logged in with bad credentials. (Bad password)");
                    LoginPacketFactory.SendLoginError(client, ErrorType.FLYFF_PASSWORD);
                    client.Disconnect();
                    return;
                }

                LoginPacketFactory.SendServerList(client, certify.Username, client.ClustersConnected);
            }
        }
    }
}