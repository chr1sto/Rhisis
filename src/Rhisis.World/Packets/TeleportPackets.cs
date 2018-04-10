using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Interfaces;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using System.Collections.Generic;
using System.Linq;
using Rhisis.World.Systems.Inventory;

namespace Rhisis.World.Packets
{
    public static partial class WorldPacketFactory
    {
        /// <summary>
        /// Send the spawn packet to the current player.
        /// </summary>
        /// <param name="player">Current player</param>
        public static void SendPlayerTeleport(IPlayerEntity player)
        {
            using (var packet = new FFPacket())
            {

                packet.StartNewMergedPacket(player.Id, SnapshotType.SETPOS);

                packet.Write(player.Object.Position.X);
                packet.Write(player.Object.Position.Y);
                packet.Write(player.Object.Position.Z);
                packet.Write(player.Object.MapId);

                packet.StartNewMergedPacket(player.Id, SnapshotType.WORLD_READINFO);

                packet.Write(player.Object.MapId);
                packet.Write(player.Object.Position.X);
                packet.Write(player.Object.Position.Y);
                packet.Write(player.Object.Position.Z);

                player.Connection.Send(packet);
            }
        }
    }
}
