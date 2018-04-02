using System;
using System.Collections.Generic;
using System.Text;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Packets
{
    public static partial class WorldPacketFactory
    {
        public static void SendTradeRequest(IPlayerEntity player, int traderId)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(traderId, SnapshotType.CONFIRMTRADE);

                player.Connection.Send(packet);
            }
        }

        public static void SendTrade(IPlayerEntity player, int traderId, int playerId)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(traderId, SnapshotType.TRADE);

                packet.Write(playerId);

                player.Connection.Send(packet);
            }
        }
    }
}
