using System;

namespace Rhisis.World.Game.Core.Systems
{
    /// <summary>
    /// This attribute is used to flag classes that indicates that they are a Rhisis system.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SystemAttribute : Attribute
    {
        /// <summary>
        /// Gets the system type.
        /// </summary>
        public SystemType Type { get; }

        /// <summary>
        /// Creates a new <see cref="SystemAttribute"/> instance.
        /// </summary>
        /// <param name="type"></param>
        public SystemAttribute(SystemType type = SystemType.Updatable)
        {
            this.Type = type;
        }
    }
}