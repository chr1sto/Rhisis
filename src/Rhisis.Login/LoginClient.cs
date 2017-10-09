using Ether.Network;
using Ether.Network.Packets;
using Rhisis.Core.Exceptions;
using Rhisis.Core.IO;
using Rhisis.Core.ISC.Structures;
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
        
        public IEnumerable<ClusterServerInfo> ClustersConnected => this._loginServer.ClustersConnected;

        public LoginConfiguration Configuration => this._loginServer.LoginConfiguration;

        public LoginClient()
        {
            this._sessionId = (uint)(new Random().Next(0, int.MaxValue));
        }

        public void InitializeClient(LoginServer loginServer)
        {
            this._loginServer = loginServer;
            CommonPacketFactory.SendWelcome(this, this._sessionId);
        }

        public void Disconnect()
        {
            this.Dispose();
            this._loginServer.DisconnectClient(this.Id);
        }

        public override void HandleMessage(NetPacketBase packet)
        {
            var pak = packet as FFPacket;
            var packetHeader = new PacketHeader(pak);

            if (!FFPacket.VerifyPacketHeader(packetHeader, (int)this._sessionId))
            {
                Logger.Warning("Invalid FlyFF header for packet: 0x{0}", packetHeader.Header.ToString("X2"));
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