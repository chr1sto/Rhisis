using NLua;
using Rhisis.Core.Common;
using Rhisis.Core.Extensions;
using Rhisis.Core.Structures.Game.Quests;
using Rhisis.Scripting.Extensions;
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
                Items = items.Values.ToArray<LuaTable>().Select(lua => new QuestItem
                {
                    Id = lua.Get<string>("id"),
                    Quantity = lua.Get<int>("quantity"),
                    Sex = lua.Get<GenderType>("sex"),
                    Remove = lua.Get<bool>("remove")
                }).ToList();
            }

            if (questEndConditionsTable[QuestScriptConstants.Monsters] is LuaTable monsters)
            {
                Monsters = monsters.Values.ToArray<LuaTable>().Select(lua => new QuestMonster
                {
                    Id = lua.Get<string>("id"),
                    Amount = lua.Get<int>("quantity")
                }).ToList();
            }

            if (questEndConditionsTable[QuestScriptConstants.Patrols] is LuaTable patrols)
            {
                Patrols = patrols.Values.ToArray<LuaTable>().Select(lua => new QuestPatrol
                {
                    MapId = lua.Get<string>("map"),
                    Left = lua.Get<int>("left"),
                    Top = lua.Get<int>("top"),
                    Right = lua.Get<int>("right"),
                    Bottom = lua.Get<int>("bottom")
                }).ToList();
            }
        }
    }
}
