using System;
using Ether.Network.Packets;
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
            this.BeginPosition = new Vector3(packet.Read<float>(), packet.Read<float>(), packet.Read<float>());
            this.DestinationPosition = new Vector3(packet.Read<float>(), packet.Read<float>(), packet.Read<float>());
            this.Angle = packet.Read<float>();
            this.AngleX = packet.Read<float>();
            this.AccPower = packet.Read<float>();
            this.TurnAngle = packet.Read<float>();
            this.State = packet.Read<uint>();
            this.StateFlag = packet.Read<uint>();
            this.Motion = packet.Read<uint>();
            this.MotionEx = packet.Read<int>();
            this.Loop = packet.Read<int>();
            this.MotionOption = packet.Read<uint>();
            this.TickCount = packet.Read<long>();
        }

        /// <summary>
        /// Compares two <see cref="PlayerBehavior2Packet"/>.
        /// </summary>
        /// <param name="other">Other <see cref="PlayerBehavior2Packet"/></param>
        public bool Equals(PlayerBehavior2Packet other)
        {
            return this.BeginPosition == other.BeginPosition &&
                   this.DestinationPosition == other.DestinationPosition &&
                   this.Angle == other.Angle &&
                   this.AngleX == other.AngleX &&
                   this.AccPower == other.AccPower &&
                   this.TurnAngle == other.TurnAngle &&
                   this.State == other.State &&
                   this.StateFlag == other.StateFlag &&
                   this.Motion == other.Motion &&
                   this.MotionEx == other.MotionEx &&
                   this.Loop == other.Loop &&
                   this.MotionOption == other.MotionOption &&
                   this.TickCount == other.TickCount;
        }
    }
}