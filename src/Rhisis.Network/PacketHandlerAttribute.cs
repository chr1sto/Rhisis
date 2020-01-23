using System;

namespace Rhisis.Network
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class PacketHandlerAttribute : Attribute
    {
        /// <summary>
        /// Gets the packet attribute header.
        /// </summary>
        public object Header { get; private set; }

        /// <summary>
        /// Creates a new <see cref="PacketHandlerAttribute"/> instance.
        /// </summary>
        /// <param name="header"></param>
        public PacketHandlerAttribute(object header)
        {
            Header = header;
        }
    }
}
