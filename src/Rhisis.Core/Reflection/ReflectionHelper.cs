using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhisis.Core.Reflection
{
    public static class ReflectionHelper
    {
        /// <summary>
        /// Get classes with a custom attribute.
        /// </summary>
        /// <param name="type">Attribute type</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetClassesWithCustomAttribute(Type type)
        {
            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.GetTypeInfo().GetCustomAttribute(type) != null);
        }

        /// <summary>
        /// Get classes with a custom attribute.
        /// </summary>
        /// <typeparam name="T">Attribute type</typeparam>
        /// <returns></returns>
        public static IEnumerable<Type> GetClassesWithCustomAttribute<T>() => GetClassesWithCustomAttribute(typeof(T));

        /// <summary>
        /// Get methods with custom attributes.
        /// </summary>
        /// <param name="type">Attribute type</param>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> GetMethodsWithAttributes(Type type)
        {
            return Assembly.GetEntryAssembly()
                .GetTypes()
                .SelectMany(x => x.GetMethods())
                .Where(x => x.GetCustomAttributes(type)?.Count() > 0);
        }

        /// <summary>
        /// Get methods with custom attributes.
        /// </summary>
        /// <typeparam name="T">Attribute type</typeparam>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> GetMethodsWithAttributes<T>() => GetMethodsWithAttributes(typeof(T));
    }
}
