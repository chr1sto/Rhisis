using System;

namespace Rhisis.Core.Handlers.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class HandlerAttribute : Attribute
    {
    }
}
