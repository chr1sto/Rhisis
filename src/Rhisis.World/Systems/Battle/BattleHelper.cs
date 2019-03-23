using Rhisis.Core.Data;
using Rhisis.Core.Helpers;
using Rhisis.Core.Structures;
using Rhisis.World.Game.Common;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using Rhisis.World.Systems.Inventory;
using System;

namespace Rhisis.World.Systems.Battle
{
    /// <summary>
    /// This class provides methods to calculate everything related to the <see cref="BattleSystem"/>.
    /// </summary>
    public static class BattleHelper
    {
        /// <summary>
        /// Gets the weapon attack damages based on player's statistics.
        /// </summary>
        /// <param name="weaponType">Weapon type</param>
        /// <param name="player">Player</param>
        /// <returns></returns>
        public static int GetWeaponAttackDamages(WeaponType weaponType, IPlayerEntity player)
        {
            float attribute = 0f;
            float levelFactor = 0f;
            float jobFactor = 1f;

            switch (weaponType)
            {
                case WeaponType.MELEE_SWD:
                    attribute = player.Statistics.Strength - 12;
                    levelFactor = player.Object.Level * 1.1f;
                    jobFactor = player.PlayerData.JobData.MeleeSword;
                    break;
                case WeaponType.MELEE_AXE:
                    attribute = player.Statistics.Strength - 12;
                    levelFactor = player.Object.Level * 1.2f;
                    jobFactor = player.PlayerData.JobData.MeleeAxe;
                    break;
                case WeaponType.MELEE_STAFF:
                    attribute = player.Statistics.Strength - 10;
                    levelFactor = player.Object.Level * 1.1f;
                    jobFactor = player.PlayerData.JobData.MeleeStaff;
                    break;
                case WeaponType.MELEE_STICK:
                    attribute = player.Statistics.Strength - 10;
                    levelFactor = player.Object.Level * 1.3f;
                    jobFactor = player.PlayerData.JobData.MeleeStick;
                    break;
                case WeaponType.MELEE_KNUCKLE:
                    attribute = player.Statistics.Strength - 10;
                    levelFactor = player.Object.Level * 1.2f;
                    jobFactor = player.PlayerData.JobData.MeleeKnuckle;
                    break;
                case WeaponType.MAGIC_WAND:
                    attribute = player.Statistics.Intelligence - 10;
                    levelFactor = player.Object.Level * 1.2f;
                    jobFactor = player.PlayerData.JobData.MagicWand;
                    break;
                case WeaponType.MELEE_YOYO:
                    attribute = player.Statistics.Strength - 10;
                    levelFactor = player.Object.Level * 1.1f;
                    jobFactor = player.PlayerData.JobData.MeleeYoyo;
                    break;
                case WeaponType.RANGE_BOW:
                    attribute = (player.Statistics.Dexterity - 14) * 4f;
                    levelFactor = player.Object.Level * 1.3f;
                    jobFactor = (player.Statistics.Strength * 0.2f) * 0.7f;
                    break;
            }

            return (int)(attribute * jobFactor + levelFactor);
        }

        // TODO: move this to utility
        public static int MulDiv(int number, int numerator, int denominator)
        {
            return (int)(((long)number * numerator) / denominator);
        }

        /// <summary>
        /// Knocks back an entity by calculating his destination position.
        /// </summary>
        /// <param name="entity">Entity to knockback</param>
        public static void KnockbackEntity(ILivingEntity entity)
        {
            var delta = new Vector3();
            float angle = MathHelper.ToRadian(entity.Object.Angle);
            float angleY = MathHelper.ToRadian(145f);

            delta.Y = (float)(-Math.Cos(angleY) * 0.18f);
            float dist = (float)(Math.Sin(angleY) * 0.18f);
            delta.X = (float)(Math.Sin(angle) * dist);
            delta.Z = (float)(-Math.Cos(angle) * dist);

            entity.MovableComponent.DestinationPosition.X += delta.X;
            entity.MovableComponent.DestinationPosition.Z += delta.Z;
        }
    }
}
