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
        public static TValue Get<TValue>(this LuaTable luaTable, string path) where TValue : IConvertible
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
    }
}
