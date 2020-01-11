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
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Systems.Skills
{
    [Injectable]
    internal class SkillSystem : ISkillSystem
    {
        private readonly ILogger<SkillSystem> _logger;
        private readonly IDatabase _database;
        private readonly IGameResources _gameResources;

        /// <inheritdoc />
        public int Order => 1;

        public SkillSystem(ILogger<SkillSystem> logger, IDatabase database, IGameResources gameResources)
        {
            this._logger = logger;
            this._database = database;
            this._gameResources = gameResources;
        }

        /// <inheritdoc />
        public void Initialize(IPlayerEntity player)
        {
            IEnumerable<SkillInfo> jobSkills = GetSkillsByJob(player, player.PlayerData.JobId);
            IEnumerable<DbSkill> playerSkills = _database.Skills.GetCharacterSkills(player.PlayerData.Id).AsQueryable().AsNoTracking().AsEnumerable();

            player.SkillTree.Skills = from x in jobSkills
                                      join s in playerSkills on x.SkillId equals s.SkillId into dbSkills
                                      from dbSkill in dbSkills.DefaultIfEmpty()
                                      select new SkillInfo(x.SkillId, player.PlayerData.Id, _gameResources.Skills[x.SkillId], dbSkill?.Id);
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
    }
}
