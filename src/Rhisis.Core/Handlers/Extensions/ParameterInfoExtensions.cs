using System;
using System.Reflection;

namespace Rhisis.Core.Handlers.Extensions
{
    /// <summary>
    /// Provides extensions to the <see cref="ParameterInfo"/> class.
    /// </summary>
    internal static class ParameterInfoExtensions
    {
        /// <summary>
        /// Gets the default value of a <see cref="ParameterInfo"/>.
        /// </summary>
        /// <param name="parameter">Parameter informations.</param>
        /// <returns>Default value.</returns>
        public static object GetParameterDefaultValue(this ParameterInfo parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException();

            if (parameter.HasDefaultValue)
                return parameter.DefaultValue;

            if (parameter.ParameterType.IsValueType)
                return Activator.CreateInstance(parameter.ParameterType);

            return null;
        }
    }
}
