using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Handlers
{
    public static partial class WorldPacketFactory
    {
        /// <summary>
        /// Sends a chat packet to all players around the current player.
        /// </summary>
        /// <param name="player">Player</param>
        /// <param name="message">Message</param>
        public static void SendChat(IPlayerEntity player, string message)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(player.Id, SnapshotType.CHAT);
                packet.Write(message);

                player.Connection.Send(packet);
                SendToVisible(packet, player);
            }
        }
    }
}
