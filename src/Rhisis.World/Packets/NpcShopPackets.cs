using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Packets
{
    public static partial class WorldPacketFactory
    {
        /// <summary>
        /// Sends the NPC shop to the player.
        /// </summary>
        /// <param name="player">Player</param>
        /// <param name="npc">NPC</param>
        public static void SendNpcShop(IPlayerEntity player, INpcEntity npc)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(npc.Id, SnapshotType.OPENSHOPWND);

                foreach(ItemContainerComponent shopTab in npc.Shop)
                    shopTab.Serialize(packet);

                player.Connection.Send(packet);   
            }
        }
    }
}
