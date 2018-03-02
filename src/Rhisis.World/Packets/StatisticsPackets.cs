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
        public static void SendUpdateState(IPlayerEntity player)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(player.Id, SnapshotType.SETSTATE);

                packet.Write<uint>(player.StatisticsComponent.Strenght);
                packet.Write<uint>(player.StatisticsComponent.Stamina);
                packet.Write<uint>(player.StatisticsComponent.Dexterity);
                packet.Write<uint>(player.StatisticsComponent.Intelligence);
                packet.Write(0);
                packet.Write<uint>(player.StatisticsComponent.StatPoints);

                player.Connection.Send(packet);
            }
        }
    }
}
