using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.World.Game.Common;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using System;

namespace Rhisis.World.Packets
{
    public partial class WorldPacketFactory
    {
        public static void SendChangePartyName(IPlayerEntity player, string name)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(player.Id, SnapshotType.PARTYCHANGENAME);
                packet.Write(name);

                player.Connection.Send(packet);
            }
        }

        public static void SendAddPartyRequestCancel(IPlayerEntity player, uint memberId, PartyRequestCancelMode mode)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(player.Id, SnapshotType.PARTYREQESTCANCEL);
                packet.Write(player.PlayerData.Id);
                packet.Write(memberId);
                packet.Write((int)mode);

                player.Connection.Send(packet);
            }
        }

        public static void SendAddPartyRequest(IPlayerEntity leader, IPlayerEntity member, bool troup)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(leader.Id, SnapshotType.PARTYREQEST);

                // Leader
                packet.Write(leader.PlayerData.Id);
                packet.Write(leader.Object.Level);
                packet.Write(leader.PlayerData.JobId);
                packet.Write(leader.VisualAppearance.Gender);

                // Member
                packet.Write(member.PlayerData.Id);
                packet.Write(member.Object.Level);
                packet.Write(member.PlayerData.JobId);
                packet.Write(member.VisualAppearance.Gender);

                packet.Write(leader.Object.Name);
                packet.Write(Convert.ToInt32(troup));

                member.Connection.Send(packet);
            }
        }

        public static void SendAddPartyMember(IPlayerEntity receiver, Party party, int playerId, string leaderName, string memberName)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(receiver.Id, SnapshotType.PARTYMEMBER);
                packet.Write(playerId);
                packet.Write(leaderName);
                packet.Write(memberName);

                if (party != null)
                    party.Serialize(packet);
                else
                    packet.Write(0);

                receiver.Connection.Send(packet);
            }
        }
    }
}
