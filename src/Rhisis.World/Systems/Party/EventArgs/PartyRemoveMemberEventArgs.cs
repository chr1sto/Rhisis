using Rhisis.Network.Packets.World.Party;
using Rhisis.World.Game.Core.Systems;

namespace Rhisis.World.Systems.Party.EventArgs
{
    public class PartyRemoveMemberEventArgs : SystemEventArgs
    {
        public uint LeaderId { get; }

        public uint MemberId { get; }

        /// <summary>
        /// Creates a new <see cref="PartyRemoveMemberEventArgs"/> instance.
        /// </summary>
        public PartyRemoveMemberEventArgs(PartyRemoveMemberPacket packet)
        {
            LeaderId = packet.LeaderId;
            MemberId = packet.MemberId;
        }

        public override bool CheckArguments()
        {
            return true;
        }
    }
}
