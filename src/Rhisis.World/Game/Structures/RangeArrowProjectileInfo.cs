using Rhisis.World.Game.Common;
using Rhisis.World.Game.Entities;
using System;

namespace Rhisis.World.Game.Structures
{
    public class RangeArrowProjectileInfo : ProjectileInfo
    {
        /// <inheritdoc />
        public override AttackFlags Type => AttackFlags.AF_GENERIC | AttackFlags.AF_RANGE;

        /// <summary>
        /// Gets the range arrow power.
        /// </summary>
        public int Power { get; }

        /// <summary>
        /// Creates a new <see cref="RangeArrowProjectileInfo"/> instance.
        /// </summary>
        /// <param name="owner">Projectile owner entity.</param>
        /// <param name="target">Projectile target entity.</param>
        /// <param name="power">Range arrow power.</param>
        /// <param name="onArrived">Action to execute when the magic projectile arrives to its target.</param>
        public RangeArrowProjectileInfo(ILivingEntity owner, ILivingEntity target, int power, Action onArrived) 
            : base(owner, target, onArrived)
        {
            Power = power;
        }
    }
}