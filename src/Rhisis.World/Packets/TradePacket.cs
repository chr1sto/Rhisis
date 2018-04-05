using System;
using System.Collections.Generic;
using System.Text;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.World.Game.Entities;
using Rhisis.World.Systems.Trade;

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

        public static void SendTrade(IPlayerEntity player, IPlayerEntity target, int playerId)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(target.Id, SnapshotType.TRADE);

                packet.Write(playerId);

                target.Inventory.Serialize(packet);

                player.Connection.Send(packet);
            }
        }

        public static void SendTradePut(IPlayerEntity player, int traderId, byte slot, byte itemType, byte id, short count)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(traderId, SnapshotType.TRADEPUT);

                packet.Write(slot);
                packet.Write(itemType);
                packet.Write(id);
                packet.Write(count);

                player.Connection.Send(packet);
            }
        }
    }
}
