using Microsoft.Extensions.Options;
using Rhisis.Core.Data;
using Rhisis.Core.Helpers;
using Rhisis.Core.IO;
using Rhisis.Core.Resources;
using Rhisis.Core.Structures;
using Rhisis.Core.Structures.Configuration.World;
using Rhisis.Core.Structures.Game;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Battle;
using Rhisis.World.Systems.Drop;
using Rhisis.World.Systems.Follow;
using Rhisis.World.Systems.Mobility;
using System;
using System.Linq;

namespace Rhisis.World.Game.Behaviors
{
    /// <summary>
    /// Default behavior for all monsters.
    /// </summary>
    [Behavior(BehaviorType.Monster, isDefault: true)]
    public class DefaultMonsterBehavior : IBehavior
    {
        private const float MovingRange = 30f;

        private readonly IMonsterEntity _monster;
        private readonly WorldConfiguration _worldConfiguration;
        private readonly IGameResources _gameResources;
        private readonly IMobilitySystem _mobilitySystem;
        private readonly IBattleSystem _battleSystem;
        private readonly IFollowSystem _followSystem;
        private readonly IDropSystem _dropSystem;
        private readonly IMoverPacketFactory _moverPacketFactory;

        public DefaultMonsterBehavior(IMonsterEntity monster,
            IOptions<WorldConfiguration> worldConfiguration,
            IGameResources gameResources, 
            IMobilitySystem mobilitySystem, 
            IBattleSystem battleSystem, 
            IFollowSystem followSystem, 
            IDropSystem dropSystem, 
            IMoverPacketFactory moverPacketFactory)
        {
            _monster = monster;
            _worldConfiguration = worldConfiguration.Value;
            _gameResources = gameResources;
            _mobilitySystem = mobilitySystem;
            _battleSystem = battleSystem;
            _followSystem = followSystem;
            _dropSystem = dropSystem;
            _moverPacketFactory = moverPacketFactory;
        }

        /// <inheritdoc />
        public virtual void Update()
        {
            if (!_monster.Object.Spawned || _monster.IsDead)
            {
                _monster.Follow.Reset();
                _monster.Battle.Reset();
                return;
            }

            if (_monster.Battle.IsFighting)
            {
                ProcessMonsterFight();
            }
            else
            {
                ProcessMonsterMovements();
            }

            _mobilitySystem.CalculatePosition(_monster);
        }

        /// <inheritdoc />
        public virtual void OnArrived()
        {
            if (!_monster.Battle.IsFighting)
            {
                long nextMoveTime = _monster.Moves.ReturningToOriginalPosition ? 
                    RandomHelper.LongRandom(1, 3) : 
                    RandomHelper.LongRandom(5, 10);

                _monster.Timers.NextMoveTime = Time.TimeInSeconds() + nextMoveTime;
                _monster.Object.BeginPosition.Copy(_monster.Object.Position);

                if (_monster.Moves.ReturningToOriginalPosition)
                {
                    _monster.Moves.ReturningToOriginalPosition = false;
                    _monster.Attributes[DefineAttributes.HP] = _monster.Data.AddHp;
                    _moverPacketFactory.SendUpdateAttributes(_monster, DefineAttributes.HP, _monster.Attributes[DefineAttributes.HP]);
                }
                if (_monster.Moves.SpeedFactor >= 2f)
                {
                    SetSpeedFactor(1f);
                }
            }
        }

        /// <inheritdoc />
        public void OnTargetKilled(ILivingEntity killedEntity)
        {
            _monster.Battle.Reset();
            _monster.Follow.Reset();
        }

        /// <inheritdoc />
        public void OnKilled(ILivingEntity killerEntity)
        {
            _monster.Timers.DespawnTime = Time.TimeInSeconds() + 5; // TODO: Configure this timer on world configuration

            // Drop items
            int itemCount = 0;
            foreach (DropItemData dropItem in _monster.Data.DropItems)
            {
                if (itemCount >= _monster.Data.MaxDropItem)
                    break;

                long dropChance = RandomHelper.LongRandom(0, DropSystem.MaxDropChance);

                if (dropItem.Probability * _worldConfiguration.Rates.Drop >= dropChance)
                {
                    var item = new Item(dropItem.ItemId, 1, -1, -1, -1, (byte)RandomHelper.Random(0, dropItem.ItemMaxRefine));

                    _dropSystem.DropItem(_monster, item, killerEntity);
                    itemCount++;
                }
            }

            // Drop item kinds
            foreach (DropItemKindData dropItemKind in _monster.Data.DropItemsKind)
            {
                var itemsDataByItemKind = _gameResources.Items.Values.Where(x => x.ItemKind3 == dropItemKind.ItemKind && x.Rare >= dropItemKind.UniqueMin && x.Rare <= dropItemKind.UniqueMax);

                if (!itemsDataByItemKind.Any())
                {
                    continue;
                }

                var itemData = itemsDataByItemKind.ElementAt(RandomHelper.Random(0, itemsDataByItemKind.Count() - 1));

                int itemRefine = RandomHelper.Random(0, 10);

                for (int i = itemRefine; i >= 0; i--)
                {
                    long itemDropProbability = (long)(_gameResources.ExpTables.GetDropLuck(itemData.Level > 120 ? 119 : itemData.Level, itemRefine) * (_monster.Data.CorrectionValue / 100f));
                    long dropChance = RandomHelper.LongRandom(0, DropSystem.MaxDropChance);

                    if (dropChance < itemDropProbability * _worldConfiguration.Rates.Drop)
                    {
                        var item = new Item(itemData.Id, 1, -1, -1, -1, (byte)itemRefine);

                        _dropSystem.DropItem(_monster, item, killerEntity);
                        break;
                    }
                }
            }

            // Drop gold
            int goldDropped = RandomHelper.Random(_monster.Data.DropGoldMin, _monster.Data.DropGoldMax);
            _dropSystem.DropGold(_monster, goldDropped, killerEntity);
        }

        /// <summary>
        /// Update monster moves.
        /// </summary>
        /// <param name="monster"></param>
        private void ProcessMonsterMovements()
        {
            if (_monster.Object.MovingFlags.HasFlag(ObjectState.OBJSTA_STAND) && _monster.Timers.NextMoveTime < Time.TimeInSeconds())
            {
                MoveToRandomPosition();
            }
        }

        /// <summary>
        /// Process the monster's fight.
        /// </summary>
        /// <param name="monster"></param>
        private void ProcessMonsterFight()
        {
            if (_monster.Follow.IsFollowing)
            {
                if (_monster.Moves.SpeedFactor != 2f)
                {
                    SetSpeedFactor(2f);
                }

                if (_monster.Object.Position.IsInCircle(_monster.Follow.Target.Object.Position, _monster.Follow.FollowDistance))
                {
                    if (_monster.Timers.NextAttackTime <= Time.TimeInMilliseconds())
                    {
                        _battleSystem.MeleeAttack(_monster, _monster.Battle.Target, ObjectMessageType.OBJMSG_ATK1, _monster.Data.AttackSpeed);
                        _monster.Timers.NextAttackTime = (long)(Time.TimeInMilliseconds() + _monster.Data.ReAttackDelay);
                    }
                }
                else
                {
                    if (_monster.Object.Position.IsInRange(_monster.Object.BeginPosition, MovingRange))
                    {
                        _followSystem.Follow(_monster, _monster.Battle.Target);
                    }
                    else
                    {
                        ReturnToBeginPosition();
                    }
                }
            }
            else
            {
                _followSystem.Follow(_monster, _monster.Battle.Target);
            }
        }

        /// <summary>
        /// Makes the monster return to its begin position.
        /// </summary>
        private void ReturnToBeginPosition()
        {
            _monster.Moves.ReturningToOriginalPosition = true;
            _monster.Battle.Reset();
            _monster.Follow.Reset();
            SetSpeedFactor(2.66f);
            MoveToPosition(_monster.Object.BeginPosition);
        }

        /// <summary>
        /// Sets the monster speed factor.
        /// </summary>
        /// <param name="speedFactor"></param>
        private void SetSpeedFactor(float speedFactor)
        {
            _monster.Moves.SpeedFactor = speedFactor;
            _moverPacketFactory.SendSpeedFactor(_monster, _monster.Moves.SpeedFactor);
        }

        /// <summary>
        /// Makes the monster move to a random position inside its region.
        /// </summary>
        private void MoveToRandomPosition()
        {
            MoveToPosition(_monster.Region.GetRandomPosition());
        }

        /// <summary>
        /// Movaes the monster to a given position.
        /// </summary>
        /// <param name="destinationPosition"></param>
        private void MoveToPosition(Vector3 destinationPosition)
        {
            _monster.Object.MovingFlags &= ~ObjectState.OBJSTA_STAND;
            _monster.Object.MovingFlags |= ObjectState.OBJSTA_FMOVE;
            _monster.Moves.DestinationPosition.Copy(destinationPosition);
            _monster.Object.Angle = Vector3.AngleBetween(_monster.Object.Position, _monster.Moves.DestinationPosition);

            _moverPacketFactory.SendDestinationPosition(_monster);
            _moverPacketFactory.SendDestinationAngle(_monster, false);
        }
    }
}
