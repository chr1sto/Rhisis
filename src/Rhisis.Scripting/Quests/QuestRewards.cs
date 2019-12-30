using NLua;
using Rhisis.Core.Common;
using Rhisis.Core.Extensions;
using Rhisis.Core.Helpers;
using Rhisis.Core.Structures.Game.Quests;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Scripting.Quests
{
    internal class QuestRewards : IQuestRewards
    {
        private readonly int? _gold;
        private readonly int _minGold;
        private readonly int _maxGold;

        /// <inheritdoc />
        public int Gold => this._gold ?? RandomHelper.Random(this._minGold, this._maxGold);

        /// <inheritdoc />
        public IEnumerable<QuestItem> Items { get; }

        /// <summary>
        /// Creates a new <see cref="QuestRewards"/> instance.
        /// </summary>
        /// <param name="questRewardsLuaTable">Quest script rewards section.</param>
        internal QuestRewards(LuaTable questRewardsLuaTable)
        {
            if (questRewardsLuaTable == null)
            {
                return;
            }

            object gold = questRewardsLuaTable[QuestScriptConstants.Gold];

            if (gold != null)
            {
                if (gold is LuaTable goldRangeTable)
                {
                    this._minGold = LuaScriptHelper.GetValue<int>(goldRangeTable, QuestScriptConstants.MinGold);
                    this._maxGold = LuaScriptHelper.GetValue<int>(goldRangeTable, QuestScriptConstants.MaxGold);
                }
                else
                {
                    this._gold = LuaScriptHelper.GetValue<int>(questRewardsLuaTable, QuestScriptConstants.Gold);
                }
            }

            if (questRewardsLuaTable[QuestScriptConstants.Items] is LuaTable items)
            {
                Items = items.Values.ToArray<LuaTable>().Select(x => new QuestItem
                {
                    Id = LuaScriptHelper.GetValue<string>(x, "id"),
                    Quantity = LuaScriptHelper.GetValue<int>(x, "quantity"),
                    Sex = LuaScriptHelper.GetValue<GenderType>(x, "sex")
                }).ToList();
            }
        }
    }
}
