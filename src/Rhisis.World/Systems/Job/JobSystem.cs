using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Resources;
using Rhisis.Core.Structures.Game;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using Rhisis.World.Systems.Skills;
using System.Collections.Generic;
using System.Linq;
using Rhisis.World.Packets;

namespace Rhisis.World.Systems.Job
{
    [Injectable]
    public class JobSystem : IJobSystem
    {
        private readonly IGameResources _gameResources;
        private readonly ISkillSystem _skillSystem;
        private readonly IPlayerPacketFactory _playerPacketFactory;
        private readonly ISpecialEffectPacketFactory _specialEffectPacketFactory;

        public JobSystem(IGameResources gameResources, ISkillSystem skillSystem, IPlayerPacketFactory playerPacketFactory, ISpecialEffectPacketFactory specialEffectPacketFactory)
        {
            _gameResources = gameResources;
            _skillSystem = skillSystem;
            _playerPacketFactory = playerPacketFactory;
            _specialEffectPacketFactory = specialEffectPacketFactory;
        }

        /// <inheritdoc />
        public void ChangeJob(IPlayerEntity player, DefineJob.Job newJob)
        {
            if (!_gameResources.Jobs.TryGetValue(newJob, out JobData jobData))
            {
                throw new KeyNotFoundException($"Cannot find job '{newJob}'.");
            }

            IEnumerable<SkillInfo> jobSkills = _skillSystem.GetSkillsByJob(newJob);

            var skillTree = new List<SkillInfo>();

            foreach (SkillInfo skill in jobSkills)
            {
                SkillInfo playerSkill = player.SkillTree.GetSkill(skill.SkillId);

                if (playerSkill != null)
                {
                    skillTree.Add(playerSkill);
                }
                else
                {
                    skillTree.Add(new SkillInfo(skill.SkillId, player.PlayerData.Id, skill.Data));
                }
            }

            player.PlayerData.JobData = jobData;
            player.SkillTree.Skills = skillTree.ToList();

            _playerPacketFactory.SendPlayerJobUpdate(player);
            _specialEffectPacketFactory.SendSpecialEffect(player, DefineSpecialEffects.XI_GEN_LEVEL_UP01, false);
        }
    }
}
