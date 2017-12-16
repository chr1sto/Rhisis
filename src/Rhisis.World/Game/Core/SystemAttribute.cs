using System;

namespace Rhisis.World.Core.Systems
{
    /// <summary>
    /// This attribute is used to flag classes that indicates that they are a Rhisis system.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SystemAttribute : Attribute
    {
    }
}