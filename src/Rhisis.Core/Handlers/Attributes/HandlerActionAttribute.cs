using System;

namespace Rhisis.Core.Handlers.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class HandlerActionAttribute : Attribute
    {
        /// <summary>
        /// Gets the handler action type.
        /// </summary>
        public object Action { get; }

        /// <summary>
        /// Creates a new <see cref="HandlerActionAttribute"/> instance.
        /// </summary>
        /// <param name="action">Action type.</param>
        public HandlerActionAttribute(object action)
        {
            this.Action = action;
        }
    }
}
