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
            var msPacket = new ModifyStatusPacket(packet);

            client.Player.Context.NotifySystem<StatisticsSystem>(client.Player,
                new StatisticsEventArgs(StatisticsActionType.ModifyStatus, msPacket));
        }
    }
}
