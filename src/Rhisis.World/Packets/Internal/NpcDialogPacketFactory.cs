using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Structures.Game.Dialogs;
using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.World.Game.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Packets.Internal
{
    [Injectable(ServiceLifetime.Singleton)]
    internal class NpcDialogPacketFactory : PacketFactoryBase, INpcDialogPacketFactory
    {
        /// <inheritdoc />
        public void SendDialog(IPlayerEntity player, IEnumerable<string> dialogTexts, IEnumerable<DialogLink> dialogLinks, IEnumerable<DialogLink> buttons = null, int questId = 0)
        {
            using var packet = new FFPacket();
            
            packet.StartNewMergedPacket(player.Id, SnapshotType.RUNSCRIPTFUNC);
            packet.Write((short)DialogOptions.FUNCTYPE_REMOVEALLKEY);

            if (dialogTexts != null)
            {
                foreach (string text in dialogTexts)
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
                    packet.Write(link.QuestId.GetValueOrDefault());
                }
            }

            if (buttons != null)
            {
                foreach (DialogLink buttonLink in buttons)
                {
                    packet.StartNewMergedPacket(player.Id, SnapshotType.RUNSCRIPTFUNC);
                    packet.Write((short)DialogOptions.FUNCTYPE_ADDANSWER);
                    packet.Write(buttonLink.Title);
                    packet.Write(buttonLink.Id);
                    packet.Write(0);
                    packet.Write(buttonLink.QuestId.GetValueOrDefault(questId));
                }
            }

            SendToPlayer(player, packet);
        }

        /// <inheritdoc />
        public void SendQuestDialogs(IPlayerEntity player, IEnumerable<DialogLink> newQuests, IEnumerable<DialogLink> currentQuests)
        {
            if (newQuests != null || currentQuests != null)
            {
                using var packet = new FFPacket();

                if (newQuests != null)
                {
                    foreach (DialogLink newQuestLink in newQuests)
                    {
                        packet.StartNewMergedPacket(player.Id, SnapshotType.RUNSCRIPTFUNC);
                        packet.Write((short)DialogOptions.FUNCTYPE_NEWQUEST);
                        packet.Write(newQuestLink.Title);
                        packet.Write(newQuestLink.Id);
                        packet.Write(0);
                        packet.Write(newQuestLink.QuestId.GetValueOrDefault());
                    }
                }

                if (currentQuests != null)
                {
                    foreach (DialogLink currentQuestLink in currentQuests)
                    {
                        packet.StartNewMergedPacket(player.Id, SnapshotType.RUNSCRIPTFUNC);
                        packet.Write((short)DialogOptions.FUNCTYPE_CURRQUEST);
                        packet.Write(currentQuestLink.Title);
                        packet.Write(currentQuestLink.Id);
                        packet.Write(0);
                        packet.Write(currentQuestLink.QuestId.GetValueOrDefault());
                    }
                }

                SendToPlayer(player, packet);
            }
        }

        /// <inheritdoc />
        public void SendCloseDialog(IPlayerEntity player)
        {
            using var packet = new FFPacket();

            packet.StartNewMergedPacket(player.Id, SnapshotType.RUNSCRIPTFUNC);
            packet.Write((short)DialogOptions.FUNCTYPE_EXIT);

            SendToPlayer(player, packet);
        }
    }
}
