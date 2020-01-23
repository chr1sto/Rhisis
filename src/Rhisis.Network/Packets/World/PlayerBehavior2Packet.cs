using System;
using Sylver.Network.Data;
using Rhisis.Core.Structures;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="PlayerBehavior2Packet"/> structure.
    /// </summary>
    public struct PlayerBehavior2Packet : IEquatable<PlayerBehavior2Packet>
    {
        /// <summary>
        /// Gets the BeginPosition.
        /// </summary>
        public Vector3 BeginPosition { get; }

        /// <summary>
        /// Gets the DestinationPosition.
        /// </summary>
        public Vector3 DestinationPosition { get; }

        /// <summary>
        /// Gets the Angle.
        /// </summary>
        public float Angle { get; }

        /// <summary>
        /// Gets the X angle.
        /// </summary>
        public float AngleX { get; set; }

        /// <summary>
        /// Gets the power.
        /// </summary>
        public float AccPower { get; set; }

        /// <summary>
        /// Gets the turn angle.
        /// </summary>
        public float TurnAngle { get; set; }

        /// <summary>
        /// Gets the state.
        /// </summary>
        public uint State { get; }

        /// <summary>
        /// Gets the state flag.
        /// </summary>
        public uint StateFlag { get; }

        /// <summary>
        /// Gets the motion.
        /// </summary>
        public uint Motion { get; }

        /// <summary>
        /// Gets the motion ex.
        /// </summary>
        public int MotionEx { get; }

        /// <summary>
        /// Gets the loop.
        /// </summary>
        public int Loop { get; }

        /// <summary>
        /// Gets the motion option.
        /// </summary>
        public uint MotionOption { get; }

        /// <summary>
        /// Gets the tick count.
        /// </summary>
        public long TickCount { get; }

        /// <summary>
        /// Creates a new <see cref="PlayerBehavior2Packet"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public PlayerBehavior2Packet(INetPacketStream packet)
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

        /// <summary>
        /// Compares two <see cref="PlayerBehavior2Packet"/>.
        /// </summary>
        /// <param name="other">Other <see cref="PlayerBehavior2Packet"/></param>
        public bool Equals(PlayerBehavior2Packet other)
        {
            return BeginPosition == other.BeginPosition &&
                   DestinationPosition == other.DestinationPosition &&
                   Angle == other.Angle &&
                   AngleX == other.AngleX &&
                   AccPower == other.AccPower &&
                   TurnAngle == other.TurnAngle &&
                   State == other.State &&
                   StateFlag == other.StateFlag &&
                   Motion == other.Motion &&
                   MotionEx == other.MotionEx &&
                   Loop == other.Loop &&
                   MotionOption == other.MotionOption &&
                   TickCount == other.TickCount;
        }
    }
}