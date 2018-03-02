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

                packet.Write(player.StatisticsComponent.Strenght);
                packet.Write(player.StatisticsComponent.Stamina);
                packet.Write(player.StatisticsComponent.Dexterity);
                packet.Write(player.StatisticsComponent.Intelligence);
                packet.Write(0);
                packet.Write(player.StatisticsComponent.StatPoints);

                player.Connection.Send(packet);
            }
        }
    }
}
