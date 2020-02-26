using Rhisis.World.Game.Common;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Game.Structures
{
    public class MagicProjectileInfo : ProjectileInfo
    {
        /// <inheritdoc />
        public override AttackFlags Type => AttackFlags.AF_MAGIC;

        /// <summary>
        /// Gets the projectile magic power.
        /// </summary>
        public int MagicPower { get; }

        /// <summary>
        /// Creates a new <see cref="MagicProjectileInfo"/> instance created by a wand attack.
        /// </summary>
        /// <param name="owner">Projectile owner entity.</param>
        /// <param name="target">Projectile target entity.</param>
        /// <param name="magicPower">Magic attack power.</param>
        public MagicProjectileInfo(ILivingEntity owner, ILivingEntity target, int magicPower) 
            : base(owner, target)
        {
            MagicPower = magicPower;
        }
    }
}