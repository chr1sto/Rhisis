using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="ChangeJobPacket"/> structure.
    /// </summary>
    public struct ChangeJobPacket : IEquatable<ChangeJobPacket>
    {
        /// <summary>
        /// Gets the job.
        /// </summary>
        public int Job { get; set; }

        /// <summary>
        /// I have no idea what gama is. Probably game?
        /// </summary>
        public bool Gama { get; set; }

        /// <summary>
        /// Creates a new <see cref="ChangeJobPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public ChangeJobPacket(INetPacketStream packet)
        {
            this.Job = packet.Read<int>();
            this.Gama = packet.Read<int>() == 1;
        }

        /// <summary>
        /// Compares two <see cref="ChangeJobPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="ChangeJobPacket"/></param>
        public bool Equals(ChangeJobPacket other)
        {
            return this.Job == other.Job && this.Gama == other.Gama;
        }
    }
}