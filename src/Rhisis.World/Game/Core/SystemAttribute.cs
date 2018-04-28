using System;

namespace Rhisis.World.Game.Core
{
    /// <summary>
    /// This attribute is used to flag classes that indicates that they are a Rhisis system.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SystemAttribute : Attribute
    {
    }
}