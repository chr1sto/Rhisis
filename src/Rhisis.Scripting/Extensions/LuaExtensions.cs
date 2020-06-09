using NLua;
using System;

namespace Rhisis.Scripting.Extensions
{
    public static class LuaExtensions
    {
        /// <summary>
        /// Gets the value of a given script field.
        /// </summary>
        /// <typeparam name="T">Target type.</typeparam>
        /// <param name="table">Lua script table.</param>
        /// <param name="path">Lua script field.</param>
        /// <returns>Lua script value converted into the target type.</returns>
        public static TValue GetValue<TValue>(this LuaTable luaTable, string path) where TValue : IConvertible
        {
            object scriptObject = luaTable[path];

            if (typeof(TValue).IsEnum)
            {
                if (scriptObject == null)
                {
                    return default;
                }

                return Enum.TryParse(typeof(TValue), scriptObject.ToString(), out object result) ? (TValue)result : default;
            }

            Type targetType = typeof(TValue);

            return scriptObject != null ? (TValue)Convert.ChangeType(scriptObject, Nullable.GetUnderlyingType(targetType) ?? targetType) : default;
        }

        /// <summary>
        /// Gets the value of a given script field or returns the default value if the field is not found.
        /// </summary>
        /// <typeparam name="TValue">Target type.</typeparam>
        /// <param name="luaTable">Lua script table.</param>
        /// <param name="path">Lya script field.</param>
        /// <param name="defaultValue">Default value to return in case the field is not found.</param>
        /// <returns>Lua script value converted into the target type or the default value if not found.</returns>
        public static TValue GetValueOrDefault<TValue>(this LuaTable luaTable, string path, TValue defaultValue) where TValue : IConvertible
        {
            if (luaTable[path] == null)
            {
                return defaultValue;
            }

            return GetValue<TValue>(luaTable, path);
        }
    }
}
