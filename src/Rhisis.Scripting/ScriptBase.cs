using NLua;

namespace Rhisis.Scripting
{
    internal abstract class ScriptBase
    {
        /// <summary>
        /// Gets the script table.
        /// </summary>
        protected LuaTable ScriptTable { get; }

        /// <summary>
        /// Creates a new <see cref="ScriptBase"/> instance.
        /// </summary>
        /// <param name="luaScriptTable">Lua script.</param>
        protected ScriptBase(LuaTable luaScriptTable)
        {
            ScriptTable = luaScriptTable;
        }
    }
}
