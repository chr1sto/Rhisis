using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="SetQuestPacket"/> structure.
    /// </summary>
    public struct SetQuestPacket : IEquatable<SetQuestPacket>
    {
        /// <summary>
        /// Gets the quest id.
        /// </summary>
        public int QuestId { get; set; }

        /// <summary>
        /// Gets the state.
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// Creates a new <see cref="SetQuestPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public SetQuestPacket(INetPacketStream packet)
        {
            QuestId = packet.Read<int>();
            State = packet.Read<int>();
        }

        /// <summary>
        /// Compares two <see cref="SetQuestPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="SetQuestPacket"/></param>
        public bool Equals(SetQuestPacket other)
        {
            return QuestId == other.QuestId && State == other.State;
        }
    }
}