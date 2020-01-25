using Sylver.Network.Data;
using Rhisis.Core.Structures;

namespace Rhisis.Network.Packets.World
{
    public class PlayerMoved2Packet : IPacketDeserializer
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
        /// Gets the X angle.
        /// </summary>
        public float AngleX { get; private set; }

        /// <summary>
        /// Gets the power.
        /// </summary>
        public float AccPower { get; private set; }

        /// <summary>
        /// Gets the turn angle.
        /// </summary>
        public float TurnAngle { get; private set; }

        /// <summary>
        /// Gets the state.
        /// </summary>
        public uint State { get; private set; }

        /// <summary>
        /// Gets the state flag.
        /// </summary>
        public uint StateFlag { get; private set; }

        /// <summary>
        /// Gets the motion.
        /// </summary>
        public uint Motion { get; private set; }

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
        public uint MotionOption { get; private set; }

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
            AngleX = packet.Read<float>();
            AccPower = packet.Read<float>();
            TurnAngle = packet.Read<float>();
            State = packet.Read<uint>();
            StateFlag = packet.Read<uint>();
            Motion = packet.Read<uint>();
            MotionEx = packet.Read<int>();
            Loop = packet.Read<int>();
            MotionOption = packet.Read<uint>();
            TickCount = packet.Read<long>();
        }
    }
}