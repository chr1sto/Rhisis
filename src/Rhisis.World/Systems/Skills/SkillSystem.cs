using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Resources;
using Rhisis.Core.Structures.Game;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using Rhisis.World.Packets;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Systems.Skills
{
    [Injectable]
    internal class SkillSystem : ISkillSystem
    {
        /// <summary>
        /// Skill points usage based on the job type.
        /// </summary>
        private static readonly IReadOnlyDictionary<DefineJob.JobType, int> SkillPointUsage = new Dictionary<DefineJob.JobType, int>
        {
            { DefineJob.JobType.JTYPE_BASE, 1 },
            { DefineJob.JobType.JTYPE_EXPERT, 2 },
            { DefineJob.JobType.JTYPE_PRO, 3 },
            { DefineJob.JobType.JTYPE_MASTER, 3 },
            { DefineJob.JobType.JTYPE_HERO, 3 }
        };

        private readonly ILogger<SkillSystem> _logger;
        private readonly IDatabase _database;
        private readonly IGameResources _gameResources;
        private readonly ISkillPacketFactory _skillPacketFactory;
        private readonly ITextPacketFactory _textPacketFactory;
        private readonly ISpecialEffectPacketFactory _specialEffectPacketFactory;

        /// <inheritdoc />
        public int Order => 1;

        public SkillSystem(ILogger<SkillSystem> logger, IDatabase database, IGameResources gameResources, ISkillPacketFactory skillPacketFactory, ITextPacketFactory textPacketFactory, ISpecialEffectPacketFactory specialEffectPacketFactory)
        {
            this._logger = logger;
            this._database = database;
            this._gameResources = gameResources;
            this._skillPacketFactory = skillPacketFactory;
            this._textPacketFactory = textPacketFactory;
            this._specialEffectPacketFactory = specialEffectPacketFactory;
        }

        /// <inheritdoc />
        public void Initialize(IPlayerEntity player)
        {
            IEnumerable<SkillInfo> jobSkills = GetSkillsByJob(player, player.PlayerData.JobId);
            IEnumerable<DbSkill> playerSkills = _database.Skills.GetCharacterSkills(player.PlayerData.Id).AsQueryable().AsNoTracking().AsEnumerable();

            player.SkillTree.Skills = (from x in jobSkills
                                       join s in playerSkills on x.SkillId equals s.SkillId into dbSkills
                                       from dbSkill in dbSkills.DefaultIfEmpty()
                                       select new SkillInfo(x.SkillId, player.PlayerData.Id, _gameResources.Skills[x.SkillId], dbSkill?.Id)).ToList();
        }

        /// <inheritdoc />
        public void Save(IPlayerEntity player)
        {
            // TODO
        }

        private IEnumerable<SkillInfo> GetSkillsByJob(IPlayerEntity player, int jobId)
        {
            var skillsList = new List<SkillInfo>();

            if (_gameResources.Jobs.TryGetValue(jobId, out JobData job) && job.Parent != null)
            {
                skillsList.AddRange(GetSkillsByJob(player, job.Parent.Id));
            }

            IEnumerable<SkillInfo> jobSkills = from x in _gameResources.Skills.Values
                                               where x.Job == job.Name &&
                                                     x.JobType != DefineJob.JobType.JTYPE_COMMON &&
                                                     x.JobType != DefineJob.JobType.JTYPE_TROUPE &&
                                                     x.Id < (int)DefineJob.JobMax.MAX_SKILLS
                                               select new SkillInfo(x.Id, player.PlayerData.Id, x);

            skillsList.AddRange(jobSkills);

            return skillsList;
        }

        /// <inheritdoc />
        public void UpdateSkills(IPlayerEntity player, IReadOnlyDictionary<int, int> skillsToUpdate)
        {
            int requiredSkillPoints = 0;

            foreach (KeyValuePair<int, int> skill in skillsToUpdate)
            {
                int skillId = skill.Key;
                int skillLevel = skill.Value;
                SkillInfo playerSkill = player.SkillTree.GetSkill(skillId);

                if (playerSkill == null)
                {
                    _textPacketFactory.SendDefinedText(player, DefineText.TID_RESKILLPOINT_ERROR);
                    _logger.LogError($"Cannot find skill with id '{skillId}' for player '{player}'.");
                    return;
                }

                if (playerSkill.Level == skillLevel)
                {
                    // Skill hasn't change
                    continue;
                }

                if (player.Object.Level < playerSkill.Data.RequiredLevel)
                {
                    _textPacketFactory.SendDefinedText(player, DefineText.TID_RESKILLPOINT_ERROR);
                    _logger.LogError($"Cannot update skill with '{skillId}' for player '{player}'. Player need to be level '{playerSkill.Data.RequiredLevel}' to learn this skill.");
                    return;
                }

                if (!CheckRequiredSkill(playerSkill.Data.RequiredSkillId1, playerSkill.Data.RequiredSkillLevel1, skillsToUpdate))
                {
                    SkillData requiredSkill1 = _gameResources.Skills[playerSkill.Data.RequiredSkillId1];

                    _textPacketFactory.SendDefinedText(player, DefineText.TID_RESKILLPOINT_ERROR);
                    _logger.LogError($"Cannot update skill with '{skillId}' for player '{player}'. Skill '{requiredSkill1.Name}' must be at least Lv.{requiredSkill1.RequiredSkillLevel1}");
                    return;
                }

                if (!CheckRequiredSkill(playerSkill.Data.RequiredSkillId2, playerSkill.Data.RequiredSkillLevel2, skillsToUpdate))
                {
                    SkillData requiredSkill2 = _gameResources.Skills[playerSkill.Data.RequiredSkillId2];

                    _textPacketFactory.SendDefinedText(player, DefineText.TID_RESKILLPOINT_ERROR);
                    _logger.LogError($"Cannot update skill with '{skillId}' for player '{player}'. Skill '{requiredSkill2.Name}' must be at least Lv.{requiredSkill2.RequiredSkillLevel1}");
                    return;
                }

                if (skillLevel < 0 || skillLevel < playerSkill.Level || skillLevel > playerSkill.Data.MaxLevel)
                {
                    _textPacketFactory.SendDefinedText(player, DefineText.TID_RESKILLPOINT_ERROR);
                    _logger.LogError($"Cannot update skill with '{skillId}' for player '{player}'. The skill level is out of bounds.");
                    return;
                }

                if (!SkillPointUsage.TryGetValue(playerSkill.Data.JobType, out int requiredSkillPointAmount))
                {
                    _textPacketFactory.SendDefinedText(player, DefineText.TID_RESKILLPOINT_ERROR);
                    _logger.LogError($"Cannot update skill with '{skillId}' for player '{player}'. Cannot find required skill point for job type '{playerSkill.Data.JobType}'.");
                    return;
                }

                requiredSkillPoints += (skillLevel - playerSkill.Level) * requiredSkillPointAmount;
            }

            if (player.Statistics.SkillPoints < requiredSkillPoints)
            {
                _logger.LogError($"Cannot update skills for player '{player}'. Not enough skill points.");
                _textPacketFactory.SendDefinedText(player, DefineText.TID_RESKILLPOINT_ERROR);
                return;
            }

            player.Statistics.SkillPoints -= (ushort)requiredSkillPoints;

            foreach (KeyValuePair<int, int> skill in skillsToUpdate)
            {
                int skillId = skill.Key;
                int skillLevel = skill.Value;

                player.SkillTree.SetSkillLevel(skillId, skillLevel);
            }

            _specialEffectPacketFactory.SendSpecialEffect(player, DefineSpecialEffects.XI_SYS_EXCHAN01, false);
            _skillPacketFactory.SendSkillTreeUpdate(player);
        }

        /// <summary>
        /// Check if the required skill condition matches.
        /// </summary>
        /// <param name="requiredSkillId">Required skill id.</param>
        /// <param name="requiredSkillLevel">Required skill level.</param>
        /// <param name="skillsToUpdate">Dictionary of skills to update.</param>
        /// <returns>True if the requirement matches; false otherwise.</returns>
        public bool CheckRequiredSkill(int requiredSkillId, int requiredSkillLevel, IReadOnlyDictionary<int, int> skillsToUpdate)
        {
            if (requiredSkillId == -1)
            {
                return true;
            }

            if (skillsToUpdate.TryGetValue(requiredSkillId, out int skillLevel))
            {
                return skillLevel >= requiredSkillLevel;
            }

            return false;
        }
    }
}
