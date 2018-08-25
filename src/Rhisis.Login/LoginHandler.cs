using Ether.Network.Packets;
using NLog;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.Core.Network.Packets.Login;
using Rhisis.Database.Entities;
using Rhisis.Database.Repositories;
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
            var pak = new PingPacket(packet);

            if (!pak.IsTimeOut)
                CommonPacketFactory.SendPong(client, pak.Time);
        }

        [PacketHandler(PacketType.CERTIFY)]
        public static void OnLogin(LoginClient client, INetPacketStream packet)
        {
            var pak = new CertifyPacket(packet, client.Configuration.PasswordEncryption, client.Configuration.EncryptionKey);

            if (pak.BuildVersion != client.Configuration.BuildVersion)
            {
                Logger.Warn($"Unable to authenticate user from {client.RemoteEndPoint}. Reason: bad client build version.");
                LoginPacketFactory.SendLoginError(client, ErrorType.CERT_GENERAL);
                client.Disconnect();
                return;
            }

            var userRepository = new UserRepository();
            DbUser dbUser = userRepository.Get(x => x.Username.Equals(pak.Username, StringComparison.OrdinalIgnoreCase));

            if (dbUser == null)
            {
                Logger.Warn($"Unable to authenticate user from {client.RemoteEndPoint}. Reason: bad username.");
                LoginPacketFactory.SendLoginError(client, ErrorType.FLYFF_ACCOUNT);
                client.Disconnect();
                return;
            }

            if (!dbUser.Password.Equals(pak.Password, StringComparison.OrdinalIgnoreCase))
            {
                Logger.Warn($"Unable to authenticate user from {client.RemoteEndPoint}. Reason: bad password.");
                LoginPacketFactory.SendLoginError(client, ErrorType.FLYFF_PASSWORD);
                client.Disconnect();
                return;
            }

            LoginPacketFactory.SendServerList(client, pak.Username, client.ClustersConnected);
            Logger.Info($"User '{pak.Username}' logged succesfully from {client.RemoteEndPoint}.");
        }
    }
}