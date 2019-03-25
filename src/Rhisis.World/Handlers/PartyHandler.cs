using Ether.Network.Packets;
using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.Network.Packets.World.Party;
using Rhisis.World.Systems.Party;
using Rhisis.World.Systems.Party.EventArgs;

namespace Rhisis.World.Handlers
{
    internal static class PartyHandler
    {
        [PacketHandler(PacketType.MEMBERREQUEST)]
        public static void OnPartyMemberRequest(WorldClient client, INetPacketStream packet)
        {
            var partyMemberRequestPacket = new PartyMemberRequestPacket(packet);
            var partyMemberRequestEventArgs = new PartyMemberRequestEventArgs(partyMemberRequestPacket);

            client.Player.NotifySystem<PartySystem>(partyMemberRequestEventArgs);
        }

        [PacketHandler(PacketType.MEMBERREQUESTCANCLE)]
        public static void OnPartyMemberRequestCancel(WorldClient client, INetPacketStream packet)
        {
            var partyMemberRequestCancelPacket = new PartyMemberRequestCancelPacket(packet);
            var partyMemberRequestCancelEventArgs = new PartyMemberRequestCancelEventArgs(partyMemberRequestCancelPacket);

            client.Player.NotifySystem<PartySystem>(partyMemberRequestCancelEventArgs);
        }

        [PacketHandler(PacketType.ADDPARTYMEMBER)]
        public static void OnAddPartyMember(WorldClient client, INetPacketStream packet)
        {
            var addPartyMemberPacket = new PartyAddMemberPacket(packet);
            var addPartyMemberEventArgs = new PartyAddMemberEventArgs(addPartyMemberPacket);

            client.Player.NotifySystem<PartySystem>(addPartyMemberEventArgs);
        }

        [PacketHandler(PacketType.REMOVEPARTYMEMBER)]
        public static void OnRemovePartyMember(WorldClient client, INetPacketStream packet)
        {
            var removePartyMemberPacket = new PartyRemoveMemberPacket(packet);
            var removePartyMemberEventArgs = new PartyRemoveMemberEventArgs(removePartyMemberPacket);

            client.Player.NotifySystem<PartySystem>(removePartyMemberEventArgs);
        }
    }
}
