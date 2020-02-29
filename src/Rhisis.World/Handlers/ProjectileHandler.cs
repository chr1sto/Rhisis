using Microsoft.Extensions.Logging;
using Rhisis.Network.Packets;
using Rhisis.Network.Packets.World;
using Rhisis.World.Client;
using Rhisis.World.Game.Common;
using Rhisis.World.Game.Structures;
using Rhisis.World.Systems.Projectile;
using Sylver.HandlerInvoker.Attributes;

namespace Rhisis.World.Handlers
{
    [Handler]
    public class ProjectileHandler
    {
        private readonly ILogger<ProjectileHandler> _logger;
        private readonly IProjectileSystem _projectileSystem;

        /// <summary>
        /// Creates a new <see cref="ProjectileHandler"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="projectileSystem">Projectile system.</param>
        public ProjectileHandler(ILogger<ProjectileHandler> logger, IProjectileSystem projectileSystem)
        {
            _logger = logger;
            _projectileSystem = projectileSystem;
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
                bool isProjectileValid = true;

                if (projectile.Target.Id != packet.TargetId)
                {
                    _logger.LogError($"Invalid projectile target for '{client.Player}'");
                    isProjectileValid = false;
                }

                if (projectile.Type != (AttackFlags)packet.Type)
                {
                    _logger.LogError($"Invalid projectile type for '{client.Player}'");
                    isProjectileValid = false;
                }

                if (!isProjectileValid)
                {
                    _projectileSystem.RemoveProjectile(client.Player, packet.IdSfxHit);
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
                bool isProjectileValid = true;

                if (projectile.Type == AttackFlags.AF_MAGIC && projectile is MagicProjectileInfo magicProjectile)
                {
                    isProjectileValid = packet.AttackerId == client.Player.Id && packet.MagicPower == magicProjectile.MagicPower;
                }
                else if (projectile.Type == AttackFlags.AF_MAGICSKILL && projectile is MagicSkillProjectileInfo magicSkillProjectile)
                {
                    isProjectileValid = packet.AttackerId == client.Player.Id && packet.SkillId == magicSkillProjectile.Skill.SkillId;
                }
                else if (projectile.Type == AttackFlags.AF_RANGE)
                {
                    // TODO
                    isProjectileValid = false;
                }

                if (isProjectileValid)
                {
                    projectile.OnArrived?.Invoke();
                }
                else
                {
                    _logger.LogError($"Invalid projectile information for player '{client.Player}'.");
                }
            }
            else
            {
                _logger.LogError($"Cannot find projectile with id '{packet.Id}' for '{client.Player}'.");
            }
        }
    }
}
