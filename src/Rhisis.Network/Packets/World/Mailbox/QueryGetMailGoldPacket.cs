using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Mailbox
{
    public struct QueryGetMailGoldPacket : IEquatable<QueryGetMailGoldPacket>
    {
        /// <summary>
        /// Gets the id of the mail
        /// </summary>
        public int MailId { get; }

        /// <summary>
        /// Creates a new <see cref="QueryGetMailGoldPacket"/> instance.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public QueryGetMailGoldPacket(INetPacketStream packet)
        {
            MailId = packet.Read<int>();
        }

        /// <summary>
        /// Compares two <see cref="QueryGetMailGoldPacket"/> objects.
        /// </summary>
        /// <param name="other">Other <see cref="QueryGetMailGoldPacket"/></param>
        /// <returns></returns>
        public bool Equals(QueryGetMailGoldPacket other)
        {
            throw new NotImplementedException();
        }
    }
}
