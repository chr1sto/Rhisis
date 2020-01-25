using System;
using Sylver.Network.Data;

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
            PlayerId = packet.Read<uint>();
            SendName = packet.Read<int>() == 1;
            if (SendName)
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
            return PlayerId == other.PlayerId && SendName == other.SendName && Name == other.Name;
        }
    }
}