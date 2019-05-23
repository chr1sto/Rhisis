using Rhisis.Core.Data;
using Rhisis.Core.Helpers;
using Rhisis.Core.IO;
using Rhisis.Core.Structures;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;
using Rhisis.World.Systems;
using Rhisis.World.Systems.Battle;

namespace Rhisis.World.Game.Behaviors
{
    /// <summary>
    /// Default behavior for all monsters.
    /// </summary>
    [Behavior(BehaviorType.Monster, IsDefault: true)]
    public class DefaultMonsterBehavior : IBehavior<IMonsterEntity>
    {
        private const float MovingRange = 40f;

        /// <inheritdoc />
        public virtual void Update(IMonsterEntity entity)
        {
            if (!entity.Object.Spawned || entity.Health.IsDead)
                return;

            if (entity.Timers.LastAICheck > Time.GetElapsedTime())
                return;

            if (entity.Battle.IsFighting)
                this.ProcessMonsterFight(entity);
            else
                this.ProcessMonsterMovements(entity);
            
            entity.Timers.LastAICheck = Time.GetElapsedTime() + (long)(entity.Data.Speed * 100f);
        }

        /// <inheritdoc />
        public virtual void OnArrived(IMonsterEntity entity)
        {
            if (!entity.Battle.IsFighting)
                entity.Timers.NextMoveTime = Time.TimeInSeconds() + RandomHelper.LongRandom(5, 10);
        }

        /// <summary>
        /// Update monster moves.
        /// </summary>
        /// <param name="monster"></param>
        private void ProcessMonsterMovements(IMonsterEntity monster)
        {
            if (monster.Timers.NextMoveTime <= Time.TimeInSeconds() &&
                monster.Object.MovingFlags.HasFlag(ObjectState.OBJSTA_STAND))
            {
                this.MoveToPosition(monster, monster.Region.GetRandomPosition());
            }
            else if (monster.Object.MovingFlags.HasFlag(ObjectState.OBJSTA_STAND))
            {
                if (monster.Moves.ReturningToOriginalPosition)
                {
                    monster.Health.Hp = monster.Data.AddHp;
                    WorldPacketFactory.SendUpdateAttributes(monster, DefineAttributes.HP, monster.Health.Hp);
                    monster.Moves.ReturningToOriginalPosition = false;
                }
                else
                {
                    if (monster.Moves.SpeedFactor >= 2f)
                    {
                        monster.Moves.SpeedFactor = 1f;
                        WorldPacketFactory.SendSpeedFactor(monster, monster.Moves.SpeedFactor);
                    }
                }
            }
        }

        /// <summary>
        /// Moves the monster to a position.
        /// </summary>
        /// <param name="monster"></param>
        /// <param name="destPosition"></param>
        private void MoveToPosition(IMonsterEntity monster, Vector3 destPosition)
        {
            monster.Object.Angle = Vector3.AngleBetween(monster.Object.Position, destPosition);
            monster.Object.MovingFlags = ObjectState.OBJSTA_FMOVE;
            monster.Moves.DestinationPosition = destPosition.Clone();

            WorldPacketFactory.SendDestinationPosition(monster);
            WorldPacketFactory.SendDestinationAngle(monster, false);
        }

        /// <summary>
        /// Process the monster's fight.
        /// </summary>
        /// <param name="monster"></param>
        private void ProcessMonsterFight(IMonsterEntity monster)
        {
            if (monster.Follow.IsFollowing)
            {
                monster.Moves.DestinationPosition = monster.Follow.Target.Object.Position.Clone();

                if (monster.Moves.SpeedFactor != 2f)
                {
                    monster.Moves.SpeedFactor = 2;
                    WorldPacketFactory.SendSpeedFactor(monster, monster.Moves.SpeedFactor);
                }

                if (!monster.Object.Position.IsInCircle(monster.Moves.DestinationPosition, 1f))
                {
                    monster.Object.MovingFlags = ObjectState.OBJSTA_FMOVE;
                    WorldPacketFactory.SendFollowTarget(monster, monster.Follow.Target, 1f);
                }
                else
                {
                    if (monster.Timers.NextAttackTime <= Time.TimeInMilliseconds())
                    {
                        monster.Timers.NextAttackTime = (long)(Time.TimeInMilliseconds() + monster.Data.ReAttackDelay);

                        var meleeAttack = new MeleeAttackEventArgs(ObjectMessageType.OBJMSG_ATK1, monster.Battle.Target, monster.Data.AttackSpeed);
                        monster.NotifySystem<BattleSystem>(meleeAttack);
                    }
                }
            }
        }
    }
}
