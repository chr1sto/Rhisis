using Ether.Network.Packets;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.Core.Network.Packets.World;
using Rhisis.World.Systems.Statistics;

namespace Rhisis.World.Handlers
{
    public static class StatisticsHandler
    {
        [PacketHandler(PacketType.MODIFY_STATUS)]
        public static void OnModifyStatus(WorldClient client, INetPacketStream packet)
        {
            var modifyStatusPacket = new ModifyStatusPacket(packet);
            var statisticsEventArgs = new StatisticsModifyEventArgs(modifyStatusPacket.Strenght,
                modifyStatusPacket.Stamina,
                modifyStatusPacket.Dexterity,
                modifyStatusPacket.Intelligence);

            client.Player.NotifySystem<StatisticsSystem>(statisticsEventArgs);
        }
    }
}
