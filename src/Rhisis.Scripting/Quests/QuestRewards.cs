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

        private readonly long? _experience;
        private readonly long _minExperience;
        private readonly long _maxExperience;

        private readonly DefineJob.Job? _rewardJob;
        private readonly LuaFunction _rewardJobFunction;
        private readonly LuaTable _mainLuaTable;

        /// <inheritdoc />
        public int Gold => _gold ?? RandomHelper.Random(_minGold, _maxGold);

        /// <inheritdoc />
        public long Experience => _experience ?? RandomHelper.LongRandom(_minExperience, _maxExperience);

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
                    _minGold = LuaScriptHelper.GetValue<int>(goldRangeTable, QuestScriptConstants.Min);
                    _maxGold = LuaScriptHelper.GetValue<int>(goldRangeTable, QuestScriptConstants.Max);
                }
                else
                {
                    _gold = LuaScriptHelper.GetValue<int>(questRewardsLuaTable, QuestScriptConstants.Gold);
                }
            }

            object experience = questRewardsLuaTable[QuestScriptConstants.Experience];

            if (experience != null)
            {
                if (experience is LuaTable experienceRangeTable)
                {
                    _minExperience = LuaScriptHelper.GetValue<long>(experienceRangeTable, QuestScriptConstants.Min);
                    _maxExperience = LuaScriptHelper.GetValue<long>(experienceRangeTable, QuestScriptConstants.Max);
                }
                else
                {
                    _experience = LuaScriptHelper.GetValue<long>(questRewardsLuaTable, QuestScriptConstants.Experience);
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
