using Ether.Network;
using Ether.Network.Packets;
using Rhisis.Core.Exceptions;
using Rhisis.Core.IO;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Login.Packets;
using System;
using System.Collections.Generic;

namespace Rhisis.Login
{
    public sealed class LoginClient : NetConnection
    {
        private readonly uint _sessionId;
        private LoginServer _loginServer;

        public LoginServer LoginSever => this._loginServer;

        public LoginConfiguration Configuration => this._loginServer.LoginConfiguration;

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

        
    }
}