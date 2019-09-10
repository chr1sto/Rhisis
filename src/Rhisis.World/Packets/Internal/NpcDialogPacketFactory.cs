using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Structures.Game.Dialogs;
using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.World.Game.Entities;
using System.Collections.Generic;

namespace Rhisis.World.Packets.Internal
{
    [Injectable(ServiceLifetime.Singleton)]
    public sealed class NpcDialogPacketFactory : INpcDialogPacketFactory
    {
        /// <inheritdoc />
        public void SendDialog(IPlayerEntity player, IEnumerable<string> dialogTexts, IEnumerable<DialogLink> dialogLinks, IEnumerable<DialogLink> questButtons = null, int questId = 0)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(player.Id, SnapshotType.RUNSCRIPTFUNC);
                packet.Write((short)DialogOptions.FUNCTYPE_REMOVEALLKEY);

                if (dialogTexts != null)
                {
                    foreach (var text in dialogTexts)
                    {
                        packet.StartNewMergedPacket(player.Id, SnapshotType.RUNSCRIPTFUNC);
                        packet.Write((short)DialogOptions.FUNCTYPE_SAY);
                        packet.Write(text);
                        packet.Write(questId);
                    }
                }

                if (dialogLinks != null)
                {
                    foreach (DialogLink link in dialogLinks)
                    {
                        packet.StartNewMergedPacket(player.Id, SnapshotType.RUNSCRIPTFUNC);
                        packet.Write((short)DialogOptions.FUNCTYPE_ADDKEY);
                        packet.Write(link.Title);
                        packet.Write(link.Id);
                        packet.Write(0);
                        packet.Write(link.QuestId);
                    }
                }

                if (questButtons != null)
                {
                    foreach (DialogLink buttonLink in questButtons)
                    {
                        packet.StartNewMergedPacket(player.Id, SnapshotType.RUNSCRIPTFUNC);
                        packet.Write((short)DialogOptions.FUNCTYPE_ADDANSWER);
                        packet.Write(buttonLink.Title);
                        packet.Write(buttonLink.Id);
                        packet.Write(0);
                        packet.Write(buttonLink.QuestId);
                    }
                }

                player.Connection.Send(packet);
            }
        }

        /// <inheritdoc />
        public void SendCloseDialog(IPlayerEntity player)
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
