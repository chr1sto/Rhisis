using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="ScriptRemoveAllItemPacket"/> structure.
    /// </summary>
    public struct ScriptRemoveAllItemPacket : IEquatable<ScriptRemoveAllItemPacket>
    {
        /// <summary>
        /// Gets the item id.
        /// </summary>
        public uint ItemId { get; set; }

        /// <summary>
        /// Creates a new <see cref="ScriptRemoveAllItemPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public ScriptRemoveAllItemPacket(INetPacketStream packet)
        {
            ItemId = packet.Read<uint>();
        }

        /// <summary>
        /// Compares two <see cref="ScriptRemoveAllItemPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="ScriptRemoveAllItemPacket"/></param>
        public bool Equals(ScriptRemoveAllItemPacket other)
        {
            return ItemId == other.ItemId;
        }
    }
}