using Ether.Network.Packets;
using System;

namespace Rhisis.Core.Network.Packets.World
{
    public struct DialogPacket : IEquatable<DialogPacket>
    {
        /// <summary>
        /// Gets the dialog owner object id.
        /// </summary>
        public int ObjectId { get; }

        /// <summary>
        /// Gets the dialog key.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Gets the first parameter.
        /// </summary>
        /// <remarks>
        /// Figure out what this value is.
        /// </remarks>
        public int Param { get; }

        /// <summary>
        /// Gets the dialog quest id.
        /// </summary>
        public int QuestId { get; }

        /// <summary>
        /// Gets the quest mover id.
        /// </summary>
        public int MoverId { get; }

        /// <summary>
        /// Gets the quest player id.
        /// </summary>
        public int PlayerId { get; }

        /// <summary>
        /// Creates a new <see cref="DialogPacket"/> instance.
        /// </summary>
        /// <param name="packet"></param>
        public DialogPacket(INetPacketStream packet)
        {
            this.ObjectId = packet.Read<int>();
            this.Key = packet.Read<string>();
            this.Param = packet.Read<int>();
            this.QuestId = packet.Read<int>();
            this.MoverId = packet.Read<int>();
            this.PlayerId = packet.Read<int>();
        }

        /// <inheritdoc />
        public bool Equals(DialogPacket other)
        {
            return this.ObjectId == other.ObjectId
                && this.Key == other.Key
                && this.Param == other.Param
                && this.QuestId == other.QuestId
                && this.MoverId == other.MoverId
                && this.PlayerId == other.PlayerId;
        }
    }
}
