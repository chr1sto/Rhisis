using Ether.Network;
using System;
using Ether.Network.Packets;
using Rhisis.Cluster.Packets;
using Rhisis.Core.Network;
using Rhisis.Core.IO;
using Rhisis.Core.Exceptions;
using System.Collections.Generic;
using Rhisis.Core.Network.Packets;

namespace Rhisis.Cluster
{
    public sealed class ClusterClient : NetConnection
    {
        private readonly uint _sessionId;

        public ClusterClient()
        {
            this._sessionId = (uint)(new Random().Next(0, int.MaxValue));
        }

        public void InitializeClient()
        {
            ClusterPacketFactory.SendWelcome(this, this._sessionId);
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

            packet.Read<uint>(); // DPID: Always 0xFFFFFFFF
            var packetHeaderNumber = packet.Read<uint>();

            try
            {
                PacketHandler<ClusterClient>.Invoke(this, pak, (PacketType)packetHeaderNumber);
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
