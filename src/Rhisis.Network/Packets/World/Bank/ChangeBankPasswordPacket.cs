using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Bank
{
    /// <summary>
    /// Defines the <see cref="ChangeBankPasswordPacket"/> structure.
    /// </summary>
    public struct ChangeBankPasswordPacket : IEquatable<ChangeBankPasswordPacket>
    {
        /// <summary>
        /// Gets the old password.
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// Gets the new password.
        /// </summary>
        public string NewPassword { get; set; }

        /// <summary>
        /// Gets the id.
        /// </summary>
        public uint Id { get; set; }

        /// <summary>
        /// Gets the item id.
        /// </summary>
        public uint ItemId { get; set; }

        /// <summary>
        /// Creates a new <see cref="ChangeBankPasswordPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public ChangeBankPasswordPacket(INetPacketStream packet)
        {
            this.OldPassword = packet.Read<string>();
            this.NewPassword = packet.Read<string>();
            this.Id = packet.Read<uint>();
            this.ItemId = packet.Read<uint>();
        }

        /// <summary>
        /// Compares two <see cref="ChangeBankPasswordPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="ChangeBankPasswordPacket"/></param>
        public bool Equals(ChangeBankPasswordPacket other)
        {
            return this.OldPassword == other.OldPassword && this.NewPassword == other.NewPassword && this.Id == other.Id && this.ItemId == other.ItemId;
        }
    }
}