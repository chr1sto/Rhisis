using System;
using Sylver.Network.Data;
using Rhisis.Core.Structures;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="LocalPosFromIAPacket"/> structure.
    /// </summary>
    public struct LocalPosFromIAPacket : IEquatable<LocalPosFromIAPacket>
    {
        /// <summary>
        /// Gets the local position.
        /// </summary>
        public Vector3 LocalPosition { get; set; }

        /// <summary>
        /// Gets the id of the IA.
        /// </summary>
        public uint IAId { get; set; }
        /// <summary>
        /// Creates a new <see cref="LocalPosFromIAPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public LocalPosFromIAPacket(INetPacketStream packet)
        {
            LocalPosition = new Vector3(packet.Read<float>(), packet.Read<float>(), packet.Read<float>());
            IAId = packet.Read<uint>();
        }

        /// <summary>
        /// Compares two <see cref="LocalPosFromIAPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="LocalPosFromIAPacket"/></param>
        public bool Equals(LocalPosFromIAPacket other)
        {
            return this.LocalPosition == other.LocalPosition && this.IAId == other.IAId;
        }
    }
}