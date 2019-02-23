using System;

namespace Rhisis.Database.Attributes
{
    /// <summary>
    /// Specifies that the value field is encrypted.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class EncryptedAttribute : Attribute
    {
    }
}
