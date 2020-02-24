using Microsoft.Extensions.Logging;
using Rhisis.Core.Data;
using Rhisis.Network.Packets;
using Rhisis.Network.Packets.World;
using Rhisis.World.Client;
using Rhisis.World.Game.Common;
using Rhisis.World.Game.Structures;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Projectile;
using Rhisis.World.Systems.Skills;
using Sylver.HandlerInvoker.Attributes;
using System.Linq;

namespace Rhisis.World.Handlers
{
    [Handler]
    public class SkillHandler
    {
        private readonly ILogger<SkillHandler> _logger;
        private readonly ISkillSystem _skillSystem;
        private readonly IProjectileSystem _projectileSystem;
        private readonly ISkillPacketFactory _skillPacketFactory;

        public SkillHandler(ILogger<SkillHandler> logger, ISkillSystem skillSystem, IProjectileSystem projectileSystem, ISkillPacketFactory skillPacketFactory)
        {
            _logger = logger;
            _skillSystem = skillSystem;
            _projectileSystem = projectileSystem;
            _skillPacketFactory = skillPacketFactory;
        }

        /// <summary>
        /// Updates the player's skill levels.
        /// </summary>
        /// <param name="client">Current client.</param>
        /// <param name="packet">Incoming packet.</param>
        [HandlerAction(PacketType.DOUSESKILLPOINT)]
        public void OnDoUseSkillPoints(IWorldClient client, DoUseSkillPointsPacket packet)
        {
            if (!packet.Skills.Any())
            {
                _logger.LogWarning($"Player {client.Player} tried to update skills, but no skills were sent.");
                return;
            }

            _skillSystem.UpdateSkills(client.Player, packet.Skills);
        }

        /// <summary>
        /// Use a skill for a given player.
        /// </summary>
        /// <param name="client">Current client.</param>
        /// <param name="packet">Incoming packet.</param>
        [HandlerAction(PacketType.USESKILL)]
        public void OnUseSkill(IWorldClient client, UseSkillPacket packet)
        {
            if (packet.SkillIndex < 0 || packet.SkillIndex > (int)DefineJob.JobMax.MAX_SKILLS)
            {
                _logger.LogWarning($"Player {client.Player} tried to use an unknown skill: '{packet.SkillIndex}'.");
                _skillPacketFactory.SendSkillCancellation(client.Player);
                return;
            }

            if (packet.TargetObjectId < 0)
            {
                _logger.LogWarning($"Player {client.Player} tried to use a skill on an unknown target.");
                _skillPacketFactory.SendSkillCancellation(client.Player);
                return;
            }

            SkillInfo skill = client.Player.SkillTree.GetSkillByIndex(packet.SkillIndex);

            _skillSystem.UseSkill(client.Player, skill, packet.TargetObjectId, packet.UseType);
        }

        /// <summary>
        /// Indicates that a projectile has been fired.
        /// </summary>
        /// <param name="client">Current client.</param>
        /// <param name="packet">Projectile packet.</param>
        [HandlerAction(PacketType.SFX_ID)]
        public void OnProjectileLaunched(IWorldClient client, SfxIdPacket packet)
        {
            var projectile = _projectileSystem.GetProjectile<ProjectileInfo>(client.Player, packet.IdSfxHit);

            if (projectile != null)
            {
                if (projectile.Target.Id != packet.TargetId)
                {
                    _logger.LogError($"Invalid projectile target for '{client.Player}'");
                    return;
                }

                if (projectile.Type != (AttackFlags)packet.Type)
                {
                    _logger.LogError($"Invalid projectile type for '{client.Player}'");
                    return;
                }
            }
        }

        /// <summary>
        /// Indicates that a projectile has arrivied and hit its target.
        /// </summary>
        /// <param name="client">Current client.</param>
        /// <param name="packet">Projectile hit packet.</param>
        [HandlerAction(PacketType.SFX_HIT)]
        public void OnProjectileArrived(IWorldClient client, SfxHitPacket packet)
        {
            var projectile = _projectileSystem.GetProjectile<ProjectileInfo>(client.Player, packet.Id);

            if (projectile != null)
            {
                if (projectile.Type == AttackFlags.AF_MAGIC)
                {
                    // TODO: inflict melee damages using a wand attack
                }
                else if (projectile.Type == AttackFlags.AF_MAGICSKILL)
                {
                    // TODO: inflict magic damages using the given skill
                }
                else if (projectile.Type == AttackFlags.AF_RANGE)
                {
                    // TODO: inflict melee damages using a bow
                }
            }
        }
    }
}
