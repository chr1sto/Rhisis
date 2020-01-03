using NLua;
using Rhisis.Core.Common;
using Rhisis.Core.Data;
using Rhisis.Core.Extensions;
using Rhisis.Core.Helpers;
using Rhisis.Core.Structures.Game.Quests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Scripting.Quests
{
    internal class QuestRewards : IQuestRewards
    {
        private readonly int? _gold;
        private readonly int _minGold;
        private readonly int _maxGold;
        private readonly DefineJob.Job? _rewardJob;
        private readonly LuaFunction _rewardJobFunction;
        private readonly LuaTable _mainLuaTable;

        /// <inheritdoc />
        public int Gold => this._gold ?? RandomHelper.Random(this._minGold, this._maxGold);

        /// <inheritdoc />
        public IEnumerable<QuestItem> Items { get; }

        /// <summary>
        /// Creates a new <see cref="QuestRewards"/> instance.
        /// </summary>
        /// <param name="mainLuaTable">Main script lua table.</param>
        /// <param name="questRewardsLuaTable">Quest script rewards section.</param>
        internal QuestRewards(LuaTable mainLuaTable, LuaTable questRewardsLuaTable)
        {
            _mainLuaTable = mainLuaTable;

            if (questRewardsLuaTable == null)
            {
                return;
            }

            object gold = questRewardsLuaTable[QuestScriptConstants.Gold];

            if (gold != null)
            {
                if (gold is LuaTable goldRangeTable)
                {
                    _minGold = LuaScriptHelper.GetValue<int>(goldRangeTable, QuestScriptConstants.MinGold);
                    _maxGold = LuaScriptHelper.GetValue<int>(goldRangeTable, QuestScriptConstants.MaxGold);
                }
                else
                {
                    _gold = LuaScriptHelper.GetValue<int>(questRewardsLuaTable, QuestScriptConstants.Gold);
                }
            }

            if (questRewardsLuaTable[QuestScriptConstants.Items] is LuaTable items)
            {
                Items = items.Values.ToArray<LuaTable>().Select(x => new QuestItem
                {
                    Id = LuaScriptHelper.GetValue<string>(x, "id"),
                    Quantity = LuaScriptHelper.GetValue<int>(x, "quantity"),
                    Sex = LuaScriptHelper.GetValue<GenderType>(x, "sex"),
                    Refine = LuaScriptHelper.GetValue<byte>(x, "refine"),
                    Element = LuaScriptHelper.GetValue<ElementType>(x, "element"),
                    ElementRefine = LuaScriptHelper.GetValue<byte>(x, "element_refine")
                }).ToList();
            }

            _rewardJob = LuaScriptHelper.GetValue<DefineJob.Job>(questRewardsLuaTable, QuestScriptConstants.Job);
            _rewardJobFunction = mainLuaTable[QuestScriptHooksConstants.ChangeJobReward] as LuaFunction;

            if (_rewardJobFunction != null)
            {
                var job = GetJob((int)DefineJob.Job.JOB_BLADE);
            }
        }

        /// <inheritdoc />
        public DefineJob.Job GetJob(object player)
        {
            if (_rewardJobFunction != null)
            {
                return (DefineJob.Job)Enum.Parse(typeof(DefineJob.Job), _rewardJobFunction.Call(_mainLuaTable, player).First()?.ToString());
            }

            return _rewardJob.GetValueOrDefault();
        }

        /// <inheritdoc />
        public bool HasJobReward() => _rewardJobFunction != null || _rewardJob.HasValue;
    }
}
