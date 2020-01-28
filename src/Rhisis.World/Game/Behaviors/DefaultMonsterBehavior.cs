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
    [Behavior(BehaviorType.Monster, IsDefault: true)]
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
                return;

            if (_monster.Timers.LastAICheck > Time.GetElapsedTime())
                return;

            _mobilitySystem.CalculatePosition(_monster);

            if (_monster.Battle.IsFighting)
                ProcessMonsterFight(_monster);
            else
                ProcessMonsterMovements(_monster);

            _monster.Timers.LastAICheck = Time.GetElapsedTime() + (long)(_monster.Moves.Speed * 100f);
        }

        /// <inheritdoc />
        public virtual void OnArrived()
        {
            if (!_monster.Battle.IsFighting)
                _monster.Timers.NextMoveTime = Time.TimeInSeconds() + RandomHelper.LongRandom(5, 10);
        }

        /// <inheritdoc />
        public void OnTargetKilled(ILivingEntity killedEntity)
        {
            _monster.Battle.Target = null;
            _monster.Battle.Targets.Clear();
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
                MoveToPosition(monster, monster.Region.GetRandomPosition());   
            }
            else if (monster.Object.MovingFlags.HasFlag(ObjectState.OBJSTA_STAND))
            {
                if (monster.Moves.ReturningToOriginalPosition)
                {
                    monster.Attributes[DefineAttributes.HP] = monster.Data.AddHp;
                    _moverPacketFactory.SendUpdateAttributes(monster, DefineAttributes.HP, monster.Attributes[DefineAttributes.HP]);
                    monster.Moves.ReturningToOriginalPosition = false;
                }
                else
                {
                    if (monster.Moves.SpeedFactor >= 2f)
                    {
                        monster.Moves.SpeedFactor = 1f;
                        _moverPacketFactory.SendSpeedFactor(monster, monster.Moves.SpeedFactor);
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

            _moverPacketFactory.SendDestinationPosition(monster);
            _moverPacketFactory.SendDestinationAngle(monster, false);
        }

        /// <summary>
        /// Process the monster's fight.
        /// </summary>
        /// <param name="monster"></param>
        private void ProcessMonsterFight(IMonsterEntity monster)
        {
            if (monster.Battle.Target.IsDead)
            {
                monster.Follow.Target = null;
                monster.Battle.Target = null;
                monster.Battle.Targets.Clear();
                return;
            }

            if (monster.Follow.IsFollowing)
            {
                monster.Moves.DestinationPosition = monster.Follow.Target.Object.Position.Clone();

                if (monster.Moves.SpeedFactor != 2f)
                {
                    monster.Moves.SpeedFactor = 2;
                    _moverPacketFactory.SendSpeedFactor(monster, monster.Moves.SpeedFactor);
                }

                if (!monster.Object.Position.IsInCircle(monster.Moves.DestinationPosition, 1f))
                {
                    monster.Object.MovingFlags = ObjectState.OBJSTA_FMOVE;
                    _followSystem.Follow(monster, monster.Battle.Target);
                }
                else
                {
                    if (monster.Timers.NextAttackTime <= Time.TimeInMilliseconds())
                    {
                        monster.Timers.NextAttackTime = (long)(Time.TimeInMilliseconds() + monster.Data.ReAttackDelay);

                        _battleSystem.MeleeAttack(monster, monster.Battle.Target, ObjectMessageType.OBJMSG_ATK1, monster.Data.AttackSpeed);
                    }
                }
            }
        }
    }
}
