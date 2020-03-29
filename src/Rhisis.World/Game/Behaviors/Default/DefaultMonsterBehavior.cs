using Rhisis.Core.Data;
using Rhisis.Core.Helpers;
using Rhisis.Core.IO;
using Rhisis.Core.Structures;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Battle;
using Rhisis.World.Systems.Follow;
using Rhisis.World.Systems.Mobility;

namespace Rhisis.World.Game.Behaviors
{
    /// <summary>
    /// Default behavior for all monsters.
    /// </summary>
    [Behavior(BehaviorType.Monster, isDefault: true)]
    public class DefaultMonsterBehavior : IBehavior
    {
        private const float MovingRange = 40f;

        private readonly IMonsterEntity _monster;
        private readonly IMobilitySystem _mobilitySystem;
        private readonly IBattleSystem _battleSystem;
        private readonly IFollowSystem _followSystem;
        private readonly IMoverPacketFactory _moverPacketFactory;

        public DefaultMonsterBehavior(IMonsterEntity monster, IMobilitySystem mobilitySystem, IBattleSystem battleSystem, IFollowSystem followSystem, IMoverPacketFactory moverPacketFactory)
        {
            _monster = monster;
            _mobilitySystem = mobilitySystem;
            _battleSystem = battleSystem;
            _followSystem = followSystem;
            _moverPacketFactory = moverPacketFactory;
        }

        /// <inheritdoc />
        public virtual void Update()
        {
            if (!_monster.Object.Spawned || _monster.IsDead)
            {
                return;
            }

            if (_monster.Battle.IsFighting)
            {
                ProcessMonsterFight(_monster);
            }
            else
            {
                ProcessMonsterMovements(_monster);
            }

            _mobilitySystem.CalculatePosition(_monster);
        }

        /// <inheritdoc />
        public virtual void OnArrived()
        {
            if (!_monster.Battle.IsFighting)
            {
                _monster.Timers.NextMoveTime = Time.TimeInSeconds() + RandomHelper.LongRandom(5, 10);
            }
        }

        /// <inheritdoc />
        public void OnTargetKilled(ILivingEntity killedEntity)
        {
            _monster.Battle.Reset();
            _monster.Follow.Reset();
        }

        /// <summary>
        /// Update monster moves.
        /// </summary>
        /// <param name="monster"></param>
        private void ProcessMonsterMovements(IMonsterEntity monster)
        {
            if (monster.Object.MovingFlags.HasFlag(ObjectState.OBJSTA_STAND))
            {
                if (monster.Timers.NextMoveTime < Time.TimeInSeconds())
                {
                    monster.Object.MovingFlags &= ~ObjectState.OBJSTA_STAND;
                    monster.Object.MovingFlags |= ObjectState.OBJSTA_FMOVE; 
                    monster.Moves.DestinationPosition.Copy(monster.Region.GetRandomPosition());
                    monster.Object.Angle = Vector3.AngleBetween(monster.Object.Position, monster.Moves.DestinationPosition);
                    
                    _moverPacketFactory.SendDestinationPosition(monster);
                    _moverPacketFactory.SendDestinationAngle(monster, false);
                }
                else if (monster.Moves.ReturningToOriginalPosition)
                {
                    monster.Attributes[DefineAttributes.HP] = monster.Data.AddHp;
                    _moverPacketFactory.SendUpdateAttributes(monster, DefineAttributes.HP, monster.Attributes[DefineAttributes.HP]);
                    monster.Moves.ReturningToOriginalPosition = false;
                }
                else if (monster.Moves.SpeedFactor >= 2f)
                {
                    monster.Moves.SpeedFactor = 1f;
                    _moverPacketFactory.SendSpeedFactor(monster, monster.Moves.SpeedFactor);
                }
            }
        }

        /// <summary>
        /// Process the monster's fight.
        /// </summary>
        /// <param name="monster"></param>
        private void ProcessMonsterFight(IMonsterEntity monster)
        {
            if (monster.Battle.Target.IsDead)
            {
                monster.Follow.Reset();
                monster.Battle.Reset();
                return;
            }

            if (monster.Follow.IsFollowing)
            {
                if (monster.Moves.SpeedFactor != 2f)
                {
                    monster.Moves.SpeedFactor = 2;
                    _moverPacketFactory.SendSpeedFactor(monster, monster.Moves.SpeedFactor);
                }

                if (monster.Object.Position.IsInCircle(monster.Follow.Target.Object.Position, monster.Follow.FollowDistance))
                {
                    if (monster.Timers.NextAttackTime <= Time.TimeInMilliseconds())
                    {
                        _battleSystem.MeleeAttack(monster, monster.Battle.Target, ObjectMessageType.OBJMSG_ATK1, monster.Data.AttackSpeed);
                        monster.Timers.NextAttackTime = (long)(Time.TimeInMilliseconds() + monster.Data.ReAttackDelay);
                    }
                }
                else
                {
                    _followSystem.Follow(monster, monster.Battle.Target);
                }
            }
            else
            {
                _followSystem.Follow(monster, monster.Battle.Target);
            }
        }
    }
}
