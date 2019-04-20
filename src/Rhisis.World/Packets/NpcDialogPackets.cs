using System.Collections.Generic;
using Rhisis.Core.Data;
using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.World.Game.Entities;
using Rhisis.Core.Structures.Game.Dialogs;

namespace Rhisis.World.Packets
{
    public static partial class WorldPacketFactory
    {
        /// <summary>
        /// Sends a NPC dialog to a player.
        /// </summary>
        /// <param name="player">Player</param>
        /// <param name="text">Npc dialog text</param>
        /// <param name="dialogLinks">Npc dialog links</param>
        public static void SendDialog(IPlayerEntity player, IEnumerable<string> dialogTexts, IEnumerable<DialogLink> dialogLinks)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(player.Id, SnapshotType.RUNSCRIPTFUNC);
                packet.Write((short)DialogOptions.FUNCTYPE_REMOVEALLKEY);

                foreach (string text in dialogTexts)
                {
                    packet.StartNewMergedPacket(player.Id, SnapshotType.RUNSCRIPTFUNC);
                    packet.Write((short)DialogOptions.FUNCTYPE_SAY);
                    packet.Write(text);
                    packet.Write(0); // quest id
                }

                foreach (DialogLink link in dialogLinks)
                {
                    packet.StartNewMergedPacket(player.Id, SnapshotType.RUNSCRIPTFUNC);
                    packet.Write((short)DialogOptions.FUNCTYPE_ADDKEY);
                    packet.Write(link.Title);
                    packet.Write(link.Id);
                    packet.Write(0);
                    packet.Write(0);
                }

                player.Connection.Send(packet);
            }
        }

        /// <summary>
        /// Send a packet to close the NPC dialog box.
        /// </summary>
        /// <param name="player"></param>
        public static void SendCloseDialog(IPlayerEntity player)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(player.Id, SnapshotType.RUNSCRIPTFUNC);
                packet.Write((short)DialogOptions.FUNCTYPE_EXIT);

                player.Connection.Send(packet);
            }
        }
    }
}
