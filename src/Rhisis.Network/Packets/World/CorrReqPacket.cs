using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="CorrReqPacket"/> structure.
    /// </summary>
    public struct CorrReqPacket : IEquatable<CorrReqPacket>
    {
        /// <summary>
        /// Gets the object id.
        /// </summary>
        public uint ObjectId { get; set; }

        /// <summary>
        /// Creates a new <see cref="CorrReqPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public CorrReqPacket(INetPacketStream packet)
        {
            this.ObjectId = packet.Read<uint>();
        }

        /// <summary>
        /// Compares two <see cref="CorrReqPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="CorrReqPacket"/></param>
        public bool Equals(CorrReqPacket other)
        {
            return this.ObjectId == other.ObjectId;
        }
    }
}