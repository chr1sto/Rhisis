using Ether.Network;
using Ether.Network.Packets;
using Rhisis.Core.Exceptions;
using Rhisis.Core.IO;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.Database;
using Rhisis.Database.Structures;
using Rhisis.Login.Packets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Login
{
    public sealed class LoginClient : NetConnection
    {
        private readonly uint _sessionId;
        private LoginServer _loginServer;

        public LoginClient()
        {
            this._sessionId = (uint)(new Random().Next(0, int.MaxValue));
        }

        public void InitializeClient(LoginServer loginServer)
        {
            this._loginServer = loginServer;
            LoginPacketFactory.SendWelcome(this, this._sessionId);
        }

        public override void HandleMessage(NetPacketBase packet)
        {
            var pak = packet as FFPacket;
            var packetHeader = new PacketHeader(pak);

            if (!FFPacket.VerifyPacketHeader(packetHeader, (int)this._sessionId))
            {
                Logger.Warning("Invalid header for packet: {0}", packetHeader.Header);
                return;
            }

            var packetHeaderNumber = packet.Read<uint>();

            try
            {
                PacketHandler<LoginClient>.Invoke(this, pak, (PacketType)packetHeaderNumber);
            }
            catch (KeyNotFoundException)
            {
                FFPacket.UnknowPacket<PacketType>(packetHeaderNumber, 2);
            }
            catch (RhisisPacketException packetException)
            {
                Logger.Error(packetException.Message);
#if DEBUG
                Logger.Debug("STACK TRACE");
                Logger.Debug(packetException.InnerException?.StackTrace);
#endif
            }
        }

        [PacketHandler(PacketType.CERTIFY)]
        public void OnLogin(NetPacketBase packet)
        {
            var certify = new CertifyPacket(packet,
                this._loginServer.LoginConfiguration.PasswordEncryption,
                this._loginServer.LoginConfiguration.EncryptionKey);

            if (certify.BuildData != this._loginServer.LoginConfiguration.BuildVersion)
            {
                Logger.Info($"User '{certify.Username}' logged in with bad build version.");
                LoginPacketFactory.SendLoginError(this, ErrorType.CERT_GENERAL);
                this.Dispose();
                this._loginServer.DisconnectClient(this.Id);
                return;
            }

            using (var db = DatabaseService.GetContext())
            {
                User user = db.Users.FirstOrDefault(x => x.Username.Equals(certify.Username, StringComparison.OrdinalIgnoreCase));

                if (user == null)
                {
                    Logger.Info($"User '{certify.Username}' logged in with bad credentials. (Bad username)");
                    LoginPacketFactory.SendLoginError(this, ErrorType.FLYFF_ACCOUNT);
                    this._loginServer.DisconnectClient(this.Id);
                    return;
                }

                if (!user.Password.Equals(certify.Password, StringComparison.OrdinalIgnoreCase))
                {
                    Logger.Info($"User '{certify.Username}' logged in with bad credentials. (Bad password)");
                    LoginPacketFactory.SendLoginError(this, ErrorType.FLYFF_PASSWORD);
                    this._loginServer.DisconnectClient(this.Id);
                    return;
                }

                LoginPacketFactory.SendServerList(this, user.Username, this._loginServer.ClustersConnected);
            }
        }
    }
}