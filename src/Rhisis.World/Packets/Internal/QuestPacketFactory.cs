using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.DependencyInjection;
using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Packets.Internal
{
    [Injectable(ServiceLifetime.Singleton)]
    internal class QuestPacketFactory : PacketFactoryBase, IQuestPacketFactory
    {
        /// <inheritdoc />
        public void SendQuest(IPlayerEntity player, QuestInfo quest)
        {
            using var packet = new FFPacket();
            
            packet.StartNewMergedPacket(player.Id, SnapshotType.SETQUEST);
            quest.Serialize(packet);

            SendToPlayer(player, packet);
        }

        /// <inheritdoc />
        public void SendCheckedQuests(IPlayerEntity player, IEnumerable<QuestInfo> checkedQuests)
        {
            using var packet = new FFPacket();

            packet.StartNewMergedPacket(player.Id, SnapshotType.QUEST_CHECKED);
            packet.Write((byte)checkedQuests.Count());

            foreach (QuestInfo quest in checkedQuests)
            {
                packet.Write((short)quest.QuestId);
            }

            SendToPlayer(player, packet);
        }
    }
}