using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="StateModeCancelPacket"/> structure.
    /// </summary>
    public struct StateModeCancelPacket : IEquatable<StateModeCancelPacket>
    {

        /// <summary>
        /// Gets the state.
        /// </summary>
        public StateType StateMode { get; set; }

        /// <summary>
        /// Gets the flag.
        /// </summary>
        public StateModeType Flag { get; set; }

        /// <summary>
        /// Creates a new <see cref="StateModeCancelPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public StateModeCancelPacket(INetPacketStream packet)
        {
            this.StateMode = (StateType) packet.Read<uint>();
            this.Flag = (StateModeType) packet.Read<int>();
        }

        /// <summary>
        /// Compares two <see cref="StateModeCancelPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="StateModeCancelPacket"/></param>
        public bool Equals(StateModeCancelPacket other)
        {
            return this.StateMode == other.StateMode && this.Flag == other.Flag;
        }
    }
}