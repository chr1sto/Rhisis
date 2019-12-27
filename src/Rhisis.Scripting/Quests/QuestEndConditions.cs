using NLua;
using Rhisis.Core.Common;
using Rhisis.Core.Extensions;
using Rhisis.Core.Structures.Game.Quests;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Scripting.Quests
{
    internal class QuestEndConditions : IQuestEndConditions
    {
        /// <inheritdoc />
        public IEnumerable<QuestItem> Items { get; }

        /// <inheritdoc />
        public IEnumerable<QuestMonster> Monsters { get; }

        /// <inheritdoc />
        public IEnumerable<QuestPatrol> Patrols { get; }

        /// <summary>
        /// Creates a new <see cref="QuestEndConditions"/> instance.
        /// </summary>
        /// <param name="questEndConditionsTable"></param>
        internal QuestEndConditions(LuaTable questEndConditionsTable)
        {
            if (questEndConditionsTable[QuestScriptConstants.Items] is LuaTable items)
            {
                Items = items.Values.ToArray<LuaTable>().Select(x => new QuestItem
                {
                    Id = LuaScriptHelper.GetValue<string>(x, "id"),
                    Quantity = LuaScriptHelper.GetValue<int>(x, "quantity"),
                    Sex = LuaScriptHelper.GetValue<GenderType>(x, "sex"),
                    Remove = LuaScriptHelper.GetValue<bool>(x, "remove")
                }).ToList();
            }

            if (questEndConditionsTable[QuestScriptConstants.Monsters] is LuaTable monsters)
            {
                Monsters = monsters.Values.ToArray<LuaTable>().Select(x => new QuestMonster
                {
                    Id = LuaScriptHelper.GetValue<string>(x, "id"),
                    Amount = LuaScriptHelper.GetValue<int>(x, "quantity")
                }).ToList();
            }

            if (questEndConditionsTable[QuestScriptConstants.Patrols] is LuaTable patrols)
            {
                Patrols = patrols.Values.ToArray<LuaTable>().Select(x => new QuestPatrol
                {
                    MapId = LuaScriptHelper.GetValue<string>(x, "map"),
                    Left = LuaScriptHelper.GetValue<int>(x, "left"),
                    Top = LuaScriptHelper.GetValue<int>(x, "top"),
                    Right = LuaScriptHelper.GetValue<int>(x, "right"),
                    Bottom = LuaScriptHelper.GetValue<int>(x, "bottom")
                }).ToList();
            }
        }
    }
}
