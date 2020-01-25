using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="ScriptEquipItemPacket"/> structure.
    /// </summary>
    public struct ScriptEquipItemPacket : IEquatable<ScriptEquipItemPacket>
    {
        /// <summary>
        /// Gets the item id.
        /// </summary>
        public uint ItemId { get; set; }

        /// <summary>
        /// Gets the option.
        /// </summary>
        public int Option { get; set; }

        /// <summary>
        /// Creates a new <see cref="ScriptEquipItemPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public ScriptEquipItemPacket(INetPacketStream packet)
        {
            ItemId = packet.Read<uint>();
            Option = packet.Read<int>();
        }

        /// <summary>
        /// Compares two <see cref="ScriptEquipItemPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="ScriptEquipItemPacket"/></param>
        public bool Equals(ScriptEquipItemPacket other)
        {
            return ItemId == other.ItemId && Option == other.Option;
        }
    }
}