using System.Reflection;

namespace Rhisis.Core.Handlers.Models
{
    /// <summary>
    /// Provides a high level structure for a handler action.
    /// </summary>
    internal class HandlerActionModel
    {
        /// <summary>
        /// Gets the handler action type.
        /// </summary>
        public object ActionType { get; }

        /// <summary>
        /// Gets the handler action name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the handler action's method informations.
        /// </summary>
        public MethodInfo Method { get; }

        /// <summary>
        /// Gets the target handler type informations where the current action is.
        /// </summary>
        public TypeInfo HandlerTypeInfo { get; }

        /// <summary>
        /// Creates a new <see cref="HandlerActionModel"/> instance.
        /// </summary>
        /// <param name="handlerActionType">Handler action type.</param>
        /// <param name="methodInfo">Handler action method informations.</param>
        /// <param name="handlerTypeInfo">Target handler type informations.</param>
        public HandlerActionModel(object handlerActionType, MethodInfo methodInfo, TypeInfo handlerTypeInfo)
        {
            this.ActionType = handlerActionType;
            this.Name = methodInfo.Name;
            this.Method = methodInfo;
            this.HandlerTypeInfo = handlerTypeInfo;
        }

        /// <inheritdoc />
        public override string ToString() => $"{this.HandlerTypeInfo.Name}.{this.Name}()";
    }
}
