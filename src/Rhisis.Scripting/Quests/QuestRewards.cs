using NLua;
using Rhisis.Core.Common;
using Rhisis.Core.Data;
using Rhisis.Core.Extensions;
using Rhisis.Core.Helpers;
using Rhisis.Core.Structures.Game.Quests;
using Rhisis.Scripting.Extensions;
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

        /// <inheritdoc />
        public bool Restat { get; }

        /// <inheritdoc />
        public bool Reskill { get; }

        /// <inheritdoc />
        public ushort SkillPoints { get; }

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
                    _minGold = goldRangeTable.GetValue<int>(QuestScriptConstants.Min);
                    _maxGold = goldRangeTable.GetValue<int>(QuestScriptConstants.Max);
                }
                else
                {
                    _gold = questRewardsLuaTable.GetValue<int>(QuestScriptConstants.Gold);
                }
            }

            object experience = questRewardsLuaTable[QuestScriptConstants.Experience];

            if (experience != null)
            {
                if (experience is LuaTable experienceRangeTable)
                {
                    _minExperience = experienceRangeTable.GetValue<long>(QuestScriptConstants.Min);
                    _maxExperience = experienceRangeTable.GetValue<long>(QuestScriptConstants.Max);
                }
                else
                {
                    _experience = questRewardsLuaTable.GetValue<long>(QuestScriptConstants.Experience);
                }
            }

            if (questRewardsLuaTable[QuestScriptConstants.Items] is LuaTable items)
            {
                Items = items.Values.ToArray<LuaTable>().Select(lua => new QuestItem
                {
                    Id = lua.GetValue<string>("id"),
                    Quantity = lua.GetValue<int>("quantity"),
                    Sex = lua.GetValue<GenderType>("sex"),
                    Refine = lua.GetValue<byte>("refine"),
                    Element = lua.GetValue<ElementType>("element"),
                    ElementRefine = lua.GetValue<byte>("element_refine")
                }).ToList();
            }

            _rewardJob = questRewardsLuaTable.GetValue<DefineJob.Job>(QuestScriptConstants.Job);
            _rewardJobFunction = mainLuaTable[QuestScriptHooksConstants.ChangeJobReward] as LuaFunction;

            Restat = questRewardsLuaTable.GetValue<bool>(QuestScriptConstants.Restat);
            Reskill = questRewardsLuaTable.GetValue<bool>(QuestScriptConstants.Reskill);
            SkillPoints = questRewardsLuaTable.GetValue<ushort>(QuestScriptConstants.SkillPoints);
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
