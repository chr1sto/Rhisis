using Ether.Network;
using System;
using Ether.Network.Packets;
using Rhisis.Core.IO;
using Rhisis.Core.Helpers;
using Rhisis.World.Packets;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using System.Collections.Generic;
using Rhisis.Core.Exceptions;

namespace Rhisis.World
{
    public sealed class WorldClient : NetConnection
    {
        private readonly uint _sessionId;
        //private WorldServer _worldServer;

        public WorldClient()
        {
            this._sessionId = RandomHelper.GenerateSessionKey();
        }

        public void InitializeClient(WorldServer worldServer)
        {
            //this._worldServer = worldServer;
            WorldPacketFactory.SendWelcome(this, this._sessionId);
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
                PacketHandler<WorldClient>.Invoke(this, pak, (PacketType)packetHeaderNumber);
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
