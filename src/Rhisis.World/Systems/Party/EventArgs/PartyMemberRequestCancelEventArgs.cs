using Rhisis.Network.Packets.World.Party;
using Rhisis.World.Game.Common;
using Rhisis.World.Game.Core.Systems;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.World.Systems.Party.EventArgs
{
    public class PartyMemberRequestCancelEventArgs : SystemEventArgs
    {
        /// <summary>
        /// Gets the leader id.
        /// </summary>
        public uint LeaderId { get; }

        /// <summary>
        /// Gets the member id.
        /// </summary>
        public uint MemberId { get; }

        /// <summary>
        /// Gets the mode.
        /// </summary>
        public PartyRequestCancelMode Mode { get; }

        /// <summary>
        /// Creates a new <see cref="PartyMemberRequestCancelEventArgs"/> instance.
        /// </summary>
        public PartyMemberRequestCancelEventArgs(PartyMemberRequestCancelPacket packet)
        {
            LeaderId = packet.LeaderId;
            MemberId = packet.MemberId;
            Mode = (PartyRequestCancelMode)packet.Mode;
        }

        public override bool CheckArguments()
        {
            return LeaderId > 0 && MemberId > 0 && LeaderId != MemberId;
        }
    }
}
