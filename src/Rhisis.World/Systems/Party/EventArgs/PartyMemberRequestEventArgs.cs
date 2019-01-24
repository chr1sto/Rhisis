using Rhisis.Network.Packets.World.Party;
using Rhisis.World.Game.Core.Systems;
using System;

namespace Rhisis.World.Systems.Party.EventArgs
{
    public class PartyMemberRequestEventArgs : SystemEventArgs
    {
        /// <summary>
        /// Gets the player id.
        /// </summary>
        public uint PlayerId { get; set; }

        /// <summary>
        /// Gets the member id.
        /// </summary>
        public uint MemberId { get; set; }

        /// <summary>
        /// Gets if it's a troup.
        /// </summary>
        public bool Troup { get; set; }

        /// <summary>
        /// Creates a new <see cref="PartyMemberRequestEventArgs"/> instance.
        /// </summary>
        /// <param name="packet"></param>
        public PartyMemberRequestEventArgs(PartyMemberRequestPacket packet)
        {
            PlayerId = packet.PlayerId;
            MemberId = packet.MemberId;
            Troup = packet.Troup;
        }

        public override bool CheckArguments()
        {
            return PlayerId > 0 && MemberId > 0 && PlayerId != MemberId;
        }
    }
}
