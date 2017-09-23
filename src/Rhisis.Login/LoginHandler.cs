using Ether.Network.Packets;
using Rhisis.Core.IO;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.Core.Network.Packets.Login;
using Rhisis.Database;
using Rhisis.Database.Structures;
using Rhisis.Login.Packets;
using System;
using System.Linq;

namespace Rhisis.Login
{
    public static class LoginHandler
    {
        [PacketHandler(PacketType.CERTIFY)]
        public static void OnLogin(LoginClient client, NetPacketBase packet)
        {
            var certify = new CertifyPacket(packet,
                client.Configuration.PasswordEncryption,
                client.Configuration.EncryptionKey);

            if (certify.BuildData != client.Configuration.BuildVersion)
            {
                Logger.Info($"User '{certify.Username}' logged in with bad build version.");
                LoginPacketFactory.SendLoginError(client, ErrorType.CERT_GENERAL);
                client.Dispose();
                client.LoginSever.DisconnectClient(client.Id);
                return;
            }

            using (var db = DatabaseService.GetContext())
            {
                User user = db.UserRepository.Get(x => x.Username.Equals(certify.Username, StringComparison.OrdinalIgnoreCase));

                if (user == null)
                {
                    Logger.Info($"User '{certify.Username}' logged in with bad credentials. (Bad username)");
                    LoginPacketFactory.SendLoginError(client, ErrorType.FLYFF_ACCOUNT);
                    client.LoginSever.DisconnectClient(client.Id);
                    return;
                }

                if (!user.Password.Equals(certify.Password, StringComparison.OrdinalIgnoreCase))
                {
                    Logger.Info($"User '{certify.Username}' logged in with bad credentials. (Bad password)");
                    LoginPacketFactory.SendLoginError(client, ErrorType.FLYFF_PASSWORD);
                    client.LoginSever.DisconnectClient(client.Id);
                    return;
                }

                LoginPacketFactory.SendServerList(client, user.Username, client.LoginSever.ClustersConnected);
            }
        }
    }
}