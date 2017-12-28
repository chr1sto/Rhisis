using Ether.Network.Packets;
using System;

namespace Rhisis.Core.Network.Packets.World
{
    public struct EquipItemPacket : IEquatable<EquipItemPacket>
    {
        /// <summary>
        /// Gets the item unique id.
        /// </summary>
        public int UniqueId { get; }

        /// <summary>
        /// Gets the equip item destination part.
        /// </summary>
        public int Part { get; }

        /// <summary>
        /// Creates a new <see cref="EquipItemPacket"/> instance.
        /// </summary>
        /// <param name="packet"></param>
        public EquipItemPacket(NetPacketBase packet)
        {
            this.UniqueId = packet.Read<int>();
            this.Part = packet.Read<int>();
        }

        /// <summary>
        /// Comapares two 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(EquipItemPacket other)
        {
            throw new NotImplementedException();
        }
    }
}
