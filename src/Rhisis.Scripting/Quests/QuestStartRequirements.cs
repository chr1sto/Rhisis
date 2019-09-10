using NLua;
using Rhisis.Core.Data;
using Rhisis.Core.Extensions;
using Rhisis.Core.Structures.Game.Quests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Scripting.Quests
{
    internal class QuestStartRequirements : IQuestStartRequirements
    {
        /// <inheritdoc />
        public int MinLevel { get; }

        /// <inheritdoc />
        public int MaxLevel { get; }

        /// <inheritdoc />
        public IEnumerable<DefineJob.Job> Jobs { get; }

        /// <summary>
        /// Creates a new <see cref="QuestStartRequirements"/> instance.
        /// </summary>
        /// <param name="questStartRequirementsTable">Quest requirements lua table.</param>
        internal QuestStartRequirements(LuaTable questStartRequirementsTable)
        {
            this.MinLevel = LuaScriptHelper.GetValue<int>(questStartRequirementsTable, QuestScriptConstants.MinLevel);
            this.MaxLevel = LuaScriptHelper.GetValue<int>(questStartRequirementsTable, QuestScriptConstants.MaxLevel);
            this.Jobs = (questStartRequirementsTable[QuestScriptConstants.Job] as LuaTable)?.Values
                .ToArray<string>()
                .Select(x => (DefineJob.Job)Enum.Parse(typeof(DefineJob.Job), x));
        }
    }
}
