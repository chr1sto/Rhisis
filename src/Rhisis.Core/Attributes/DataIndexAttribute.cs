using System;

namespace Rhisis.Core.Attributes
{
    /// <summary>
    /// When applied to the property of a type, specifies that the property should be indexed.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class DataIndexAttribute : Attribute
    {
    }
}
