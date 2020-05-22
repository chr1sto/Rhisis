using Sylver.Network.Data;
using Rhisis.Core.Structures;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Structure for the <see cref="PacketType.PLAYERMOVED"/> packet.
    /// </summary>
    public class PlayerMovedPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the BeginPosition.
        /// </summary>
        public Vector3 BeginPosition { get; private set; }

        /// <summary>
        /// Gets the DestinationPosition.
        /// </summary>
        public Vector3 DestinationPosition { get; private set; }

        /// <summary>
        /// Gets the Angle.
        /// </summary>
        public float Angle { get; private set; }

        /// <summary>
        /// Gets the object state (MovingFlag).
        /// </summary>
        public uint State { get; private set; }

        /// <summary>
        /// Gets the state flag. (Motion flag)
        /// </summary>
        public int StateFlag { get; private set; }

        /// <summary>
        /// Gets the motion.
        /// </summary>
        public int Motion { get; private set; }

        /// <summary>
        /// Gets the motion ex.
        /// </summary>
        public int MotionEx { get; private set; }

        /// <summary>
        /// Gets the loop.
        /// </summary>
        public int Loop { get; private set; }

        /// <summary>
        /// Gets the motion option.
        /// </summary>
        public int MotionOption { get; private set; }

        /// <summary>
        /// Gets the tick count.
        /// </summary>
        public long TickCount { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            BeginPosition = new Vector3(packet.Read<float>(), packet.Read<float>(), packet.Read<float>());
            DestinationPosition = new Vector3(packet.Read<float>(), packet.Read<float>(), packet.Read<float>());
            Angle = packet.Read<float>();
            State = packet.Read<uint>();
            StateFlag = packet.Read<int>();
            Motion = packet.Read<int>();
            MotionEx = packet.Read<int>();
            Loop = packet.Read<int>();
            MotionOption = packet.Read<int>();
            TickCount = packet.Read<long>();
        }
    }
}