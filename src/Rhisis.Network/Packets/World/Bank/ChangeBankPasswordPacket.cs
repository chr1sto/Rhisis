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
            OldPassword = packet.Read<string>();
            NewPassword = packet.Read<string>();
            Id = packet.Read<uint>();
            ItemId = packet.Read<uint>();
        }

        /// <summary>
        /// Compares two <see cref="ChangeBankPasswordPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="ChangeBankPasswordPacket"/></param>
        public bool Equals(ChangeBankPasswordPacket other)
        {
            return OldPassword == other.OldPassword && NewPassword == other.NewPassword && Id == other.Id && ItemId == other.ItemId;
        }
    }
}