using NLua;
using Rhisis.Core.Extensions;
using Rhisis.Core.Structures.Game.Quests;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Rhisis.Scripting.Quests
{
    [DebuggerDisplay("{Name}")]
    internal class QuestScript : ScriptBase, IQuestScript
    {
        /// <inheritdoc />
        public int Id { get; private set; }

        /// <inheritdoc />
        public string Name { get; private set; }

        /// <inheritdoc />
        public string Title { get; private set; }

        /// <inheritdoc />
        public string StartCharacter { get; private set; }

        /// <inheritdoc />
        public IQuestRewards Rewards { get; private set; }

        /// <inheritdoc />
        public IQuestStartRequirements StartRequirements { get; }

        /// <inheritdoc />
        public IEnumerable<string> BeginDialogs { get; }

        /// <inheritdoc />
        public IEnumerable<string> AcceptedDialogs { get; }

        /// <inheritdoc />
        public IEnumerable<string> DeclinedDialogs { get; }

        /// <inheritdoc />
        public IEnumerable<string> CompletedDialogs { get; }

        /// <inheritdoc />
        public IEnumerable<string> NotFinishedDialogs { get; }

        /// <summary>
        /// Creates a new <see cref="QuestScript"/> instance.
        /// </summary>
        /// <param name="questId">Quest id.</param>
        /// <param name="questName">Quest name.</param>
        /// <param name="luaScriptTable">Lua script table.</param>
        public QuestScript(int questId, string questName, LuaTable luaScriptTable)
            : base(luaScriptTable)
        {
            this.Id = questId;
            this.Name = questName;
            this.Title = LuaScriptHelper.GetValue<string>(luaScriptTable, QuestScriptConstants.Title);
            this.StartCharacter = LuaScriptHelper.GetValue<string>(luaScriptTable, QuestScriptConstants.StartCharacter);
            this.Rewards = new QuestRewards(this.ScriptTable[QuestScriptConstants.Rewards] as LuaTable);
            this.StartRequirements = new QuestStartRequirements(this.ScriptTable[QuestScriptConstants.StartRequirements] as LuaTable);

            if (this.ScriptTable[QuestScriptConstants.Dialogs] is LuaTable dialogsTable)
            {
                this.BeginDialogs = this.GetDialogs(dialogsTable, QuestScriptConstants.BeginDialogs);
                this.AcceptedDialogs = this.GetDialogs(dialogsTable, QuestScriptConstants.BeginYesDialogs);
                this.DeclinedDialogs = this.GetDialogs(dialogsTable, QuestScriptConstants.BeginNoDialogs);
                this.CompletedDialogs = this.GetDialogs(dialogsTable, QuestScriptConstants.CompletedDialogs);
                this.NotFinishedDialogs = this.GetDialogs(dialogsTable, QuestScriptConstants.NotFinishedDialogs);
            }
        }

        /// <summary>
        /// Gets the dialogs values.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private IEnumerable<string> GetDialogs(LuaTable table, string path) => (table[path] as LuaTable)?.Values.ToArray<string>();
    }
}
