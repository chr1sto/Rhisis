using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.DependencyInjection;
using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;

namespace Rhisis.World.Packets.Internal
{
    [Injectable(ServiceLifetime.Singleton)]
    internal sealed class QuestPacketFactory : IQuestPacketFactory
    {
        /// <inheritdoc />
        public void SendQuest(IPlayerEntity player, QuestInfo quest)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(player.Id, SnapshotType.SETQUEST);
                quest.Serialize(packet);

                player.Connection.Send(packet);
            }
        }
    }
}