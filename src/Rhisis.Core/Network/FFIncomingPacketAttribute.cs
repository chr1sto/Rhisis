using Rhisis.Core.Network.Packets;
using System;

namespace Rhisis.Core.Network
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class FFIncomingPacketAttribute : Attribute
    {
        /// <summary>
        /// Gets the packet attribute header.
        /// </summary>
        public PacketType Header { get; private set; }

        /// <summary>
        /// Creates a new FFIncomingPacketAttribute instance.
        /// </summary>
        /// <param name="header"></param>
        public FFIncomingPacketAttribute(PacketType header)
        {
            this.Header = header;
        }
    }
}
