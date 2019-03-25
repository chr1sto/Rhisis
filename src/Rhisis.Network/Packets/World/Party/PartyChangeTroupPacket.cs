using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World.Party
{
    /// <summary>
    /// Defines the <see cref="PartyChangeTroupPacket"/> structure.
    /// </summary>
    public struct PartyChangeTroupPacket : IEquatable<PartyChangeTroupPacket>
    {
        /// <summary>
        /// Gets the player id.
        /// </summary>
        public uint PlayerId { get; set; }

        /// <summary>
        /// Gets if a name was sent.
        /// </summary>
        public bool SendName { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Creates a new <see cref="PartyChangeTroupPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public PartyChangeTroupPacket(INetPacketStream packet)
        {
            this.PlayerId = packet.Read<uint>();
            this.SendName = packet.Read<int>() == 1;
            if (this.SendName)
                Name = packet.Read<string>();
            else
                Name = null;
        }

        /// <summary>
        /// Compares two <see cref="PartyChangeTroupPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="PartyChangeTroupPacket"/></param>
        public bool Equals(PartyChangeTroupPacket other)
        {
            return this.PlayerId == other.PlayerId && this.SendName == other.SendName && this.Name == other.Name;
        }
    }
}