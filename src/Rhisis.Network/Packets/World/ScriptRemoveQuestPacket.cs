using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="ScriptRemoveQuestPacket"/> structure.
    /// </summary>
    public struct ScriptRemoveQuestPacket : IEquatable<ScriptRemoveQuestPacket>
    {
        /// <summary>
        /// Gets the gold.
        /// </summary>
        public int QuestId { get; set; }

        /// <summary>
        /// Creates a new <see cref="ScriptRemoveQuestPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public ScriptRemoveQuestPacket(INetPacketStream packet)
        {
            QuestId = packet.Read<int>();
        }

        /// <summary>
        /// Compares two <see cref="ScriptRemoveQuestPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="ScriptRemoveQuestPacket"/></param>
        public bool Equals(ScriptRemoveQuestPacket other)
        {
            return QuestId == other.QuestId;
        }
    }
}