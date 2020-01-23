using Sylver.Network.Data;
using System;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="QueryPlayerDataPacket"/> packet structure.
    /// </summary>
    public struct QueryPlayerDataPacket : IEquatable<QueryPlayerDataPacket>
    {
        /// <summary>
        /// Gets the player id.
        /// </summary>
        public uint PlayerId { get; }

        /// <summary>
        /// Gets the version.
        /// </summary>
        public int Version { get; }

        /// <summary>
        /// Creates a new <see cref="QueryPlayerDataPacket"/> instance.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public QueryPlayerDataPacket(INetPacketStream packet)
        {
            PlayerId = packet.Read<uint>();
            Version = packet.Read<int>();
        }

        /// <summary>
        /// Compare two <see cref="QueryPlayerDataPacket"/> instances.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(QueryPlayerDataPacket other) => PlayerId == other.PlayerId && Version == other.Version;
    }
}
