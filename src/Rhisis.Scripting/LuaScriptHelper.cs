using NLua;
using System;

namespace Rhisis.Scripting
{
    /// <summary>
    /// Provides helpers to parse lua scripts.
    /// </summary>
    internal static class LuaScriptHelper
    {
        /// <summary>
        /// Gets the value of a given script field.
        /// </summary>
        /// <typeparam name="T">Target type.</typeparam>
        /// <param name="table">Lua script table.</param>
        /// <param name="path">Lua script field.</param>
        /// <returns>Lua script value converted into the target type.</returns>
        internal static T GetValue<T>(LuaTable table, string path)
        {
            object scriptObject = table[path];

            return scriptObject != null ? (T)Convert.ChangeType(scriptObject, typeof(T)) : default;
        }
    }
}
