using Ether.Network.Packets;
using System;

namespace Rhisis.Core.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="JoinPacket"/> structure.
    /// </summary>
    public struct JoinPacket : IEquatable<JoinPacket>
    {
        /// <summary>
        /// Gets the World Id.
        /// </summary>
        public int WorldId { get; }

        /// <summary>
        /// Gets the Player Id.
        /// </summary>
        public int PlayerId { get; }

        /// <summary>
        /// Gets the Authentication key.
        /// </summary>
        public int AuthenticationKey { get; }

        /// <summary>
        /// Gets the player's party id.
        /// </summary>
        public int PartyId { get; }

        /// <summary>
        /// Gets the player's guild id.
        /// </summary>
        public int GuildId { get; }

        /// <summary>
        /// Gets the player's guild war id.
        /// </summary>
        public int GuildWarId { get; }

        /// <summary>
        /// Gets the Id of multi
        /// </summary>
        /// <remarks>
        /// What is this ?
        /// </remarks>
        public int IdOfMulti { get; }

        /// <summary>
        /// Gets the player's slot.
        /// </summary>
        public byte Slot { get; }

        /// <summary>
        /// Gets the player's name.
        /// </summary>
        public string PlayerName { get; }

        /// <summary>
        /// Gets the player's account username.
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// Gets the player's account password.
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// Gets the messenger state.
        /// </summary>
        public int MessengerState { get; }

        /// <summary>
        /// Gets the messenger count.
        /// </summary>
        public int MessengerCount { get; }

        /// <summary>
        /// Creates a new <see cref="JoinPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public JoinPacket(NetPacketBase packet)
        {
            this.WorldId = packet.Read<int>();
            this.PlayerId = packet.Read<int>();
            this.AuthenticationKey = packet.Read<int>();
            this.PartyId = packet.Read<int>();
            this.GuildId = packet.Read<int>();
            this.GuildWarId = packet.Read<int>();
            this.IdOfMulti = packet.Read<int>(); // what is this?
            this.Slot = packet.Read<byte>();
            this.PlayerName = packet.Read<string>();
            this.Username = packet.Read<string>();
            this.Password = packet.Read<string>();
            this.MessengerState = packet.Read<int>();
            this.MessengerCount = packet.Read<int>();
        }

        /// <summary>
        /// Compares two <see cref="JoinPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="JoinPacket"/></param>
        public bool Equals(JoinPacket other)
        {
            return this.WorldId == other.WorldId
                && this.PlayerId == other.PlayerId
                && this.AuthenticationKey == other.AuthenticationKey
                && this.PartyId == other.PartyId
                && this.GuildId == other.GuildId
                && this.GuildWarId == other.GuildWarId
                && this.IdOfMulti == other.IdOfMulti
                && this.Slot == other.Slot
                && this.PlayerName.Equals(other.PlayerName, StringComparison.OrdinalIgnoreCase)
                && this.Username.Equals(other.Username, StringComparison.OrdinalIgnoreCase)
                && this.Password.Equals(other.Password, StringComparison.OrdinalIgnoreCase)
                && this.MessengerState == other.MessengerState
                && this.MessengerCount == other.MessengerCount;
        }
    }
}
