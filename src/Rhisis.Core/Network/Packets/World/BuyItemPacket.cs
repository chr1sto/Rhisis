using Ether.Network.Packets;
using System;

namespace Rhisis.Core.Network.Packets.World
{
    public struct BuyItemPacket : IEquatable<BuyItemPacket>
    {
        /// <summary>
        /// Gets the tab.
        /// </summary>
        public byte Tab { get; }

        /// <summary>
        /// Gets the slot of the item.
        /// </summary>
        public byte Slot { get; }

        /// <summary>
        /// Gets the quantity of items.
        /// </summary>
        public short Quantity { get; }

        /// <summary>
        /// Gets the Item Id.
        /// </summary>
        public int ItemId { get; }
        
        /// <summary>
        /// Creates a new <see cref="BuyItemPacket"/> instance.
        /// </summary>
        /// <param name="packet">Incoming packet stream</param>
        public BuyItemPacket(INetPacketStream packet)
        {
            this.Tab = packet.Read<byte>();
            this.Slot = packet.Read<byte>();
            this.Quantity = packet.Read<short>();
            this.ItemId = packet.Read<int>();
        }

        /// <summary>
        /// Compares two <see cref="BuyItemPacket"/> instances.
        /// </summary>
        /// <param name="other">Other packet</param>
        /// <returns></returns>
        public bool Equals(BuyItemPacket other)
        {
            return this.Tab == other.Tab 
                && this.Slot == other.Slot 
                && this.Quantity == other.Quantity
                && this.ItemId == other.ItemId;
        }
    }
}
