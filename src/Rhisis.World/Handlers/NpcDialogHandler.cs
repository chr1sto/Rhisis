using Ether.Network.Packets;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.Core.Network.Packets.World;
using Rhisis.World.Systems.NpcDialog;

namespace Rhisis.World.Handlers
{
    public static class NpcDialogHandler
    {
        [PacketHandler(PacketType.SCRIPTDLG)]
        public static void OnDialogScript(WorldClient client, INetPacketStream packet)
        {
            var dialogPacket = new DialogPacket(packet);
            var dialogEvent = new NpcDialogOpenEventArgs(dialogPacket.ObjectId, dialogPacket.Key);

            client.Player.Context.NotifySystem<NpcDialogSystem>(client.Player, dialogEvent);
        }
    }
}