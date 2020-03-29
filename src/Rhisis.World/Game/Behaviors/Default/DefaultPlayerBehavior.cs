using Microsoft.Extensions.Options;
using Rhisis.Core.Common;
using Rhisis.Core.Data;
using Rhisis.Core.IO;
using Rhisis.Core.Structures.Configuration.World;
using Rhisis.Core.Structures.Game.Quests;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;
using Rhisis.World.Systems;
using Rhisis.World.Systems.Experience;
using Rhisis.World.Systems.Inventory;
using Rhisis.World.Systems.Mobility;
using Rhisis.World.Systems.PlayerData;
using Rhisis.World.Systems.Quest;
using Rhisis.World.Systems.Recovery;

namespace Rhisis.World.Game.Behaviors
{
    [Behavior(BehaviorType.Player, isDefault: true)]
    public sealed class DefaultPlayerBehavior : IBehavior
    {
        private readonly IPlayerEntity _player;
        private readonly WorldConfiguration _worldConfiguration;
        private readonly IMobilitySystem _mobilitySystem;
        private readonly IInventorySystem _inventorySystem;
        private readonly IPlayerDataSystem _playerDataSystem;
        private readonly IRecoverySystem _recoverySystem;
        private readonly IRegionTriggerSystem _regionTriggerSystem;
        private readonly IQuestSystem _questSystem;
        private readonly IExperienceSystem _experienceSystem;
        private readonly IMoverPacketFactory _moverPacketFactory;
        private readonly ITextPacketFactory _textPacketFactory;

        /// <summary>
        /// Creates a new <see cref="DefaultPlayerBehavior"/> instance.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="worldConfiguration">World Server configuration.</param>
        /// <param name="mobilitySystem">Mobility system.</param>
        /// <param name="inventorySystem">Inventory system.</param>
        /// <param name="playerDataSystem">Player data system.</param>
        /// <param name="recoverySystem">Recovery system.</param>
        /// <param name="regionTriggerSystem">Region trigger system.</param>
        /// <param name="questSystem">Quest system.</param>
        /// <param name="experienceSystem">Experience system.</param>
        /// <param name="moverPacketFactory">Mover packet factory.</param>
        /// <param name="textPacketFactory">Text packet factory.</param>
        public DefaultPlayerBehavior(IPlayerEntity player, 
            IOptions<WorldConfiguration> worldConfiguration,
            IMobilitySystem mobilitySystem, 
            IInventorySystem inventorySystem, 
            IPlayerDataSystem playerDataSystem, 
            IRecoverySystem recoverySystem, 
            IRegionTriggerSystem regionTriggerSystem,
            IQuestSystem questSystem,
            IExperienceSystem experienceSystem,
            IMoverPacketFactory moverPacketFactory, 
            ITextPacketFactory textPacketFactory)
        {
            _player = player;
            _worldConfiguration = worldConfiguration.Value;
            _mobilitySystem = mobilitySystem;
            _inventorySystem = inventorySystem;
            _playerDataSystem = playerDataSystem;
            _recoverySystem = recoverySystem;
            _regionTriggerSystem = regionTriggerSystem;
            _questSystem = questSystem;
            _experienceSystem = experienceSystem;
            _moverPacketFactory = moverPacketFactory;
            _textPacketFactory = textPacketFactory;
        }

        /// <inheritdoc />
        public void Update()
        {
            if (!_player.Object.Spawned || _player.IsDead)
            {
                return;
            }

            ProcessIdleHeal();
            _regionTriggerSystem.CheckWrapzones(_player);
            _mobilitySystem.CalculatePosition(_player);
        }

        /// <inheritdoc />
        public void OnArrived()
        {
            if (_player.Follow.IsFollowing && _player.Follow.Target.Type == WorldEntityType.Drop)
            {
                if (_player.Follow.Target is IItemEntity target)
                {
                    PickUpDroppedItem(target);
                }

                _player.Follow.Reset();
            }
        }

        /// <inheritdoc />
        public void OnTargetKilled(ILivingEntity killedEntity)
        {
            if (killedEntity is IMonsterEntity deadMonster)
            {
                // Give experience
                _experienceSystem.GiveExeperience(_player, deadMonster.Data.Experience * _worldConfiguration.Rates.Experience);

                // Quest check
                _questSystem.UpdateQuestDiary(_player, QuestActionType.KillMonster, deadMonster.Data.Id, 1);
            }
        }

        /// <inheritdoc />
        public void OnKilled(ILivingEntity killerEntity)
        {
            // TODO:
        }

        /// <summary>
        /// Verify all conditions to pickup a dropped item.
        /// </summary>
        /// <param name="droppedItem">The dropped item.</param>
        private void PickUpDroppedItem(IItemEntity droppedItem)
        {
            // TODO: check if drop belongs to a party.

            if (droppedItem.Drop.HasOwner && droppedItem.Drop.Owner != _player)
            {
                _textPacketFactory.SendDefinedText(_player, DefineText.TID_GAME_PRIORITYITEMPER, $"\"{droppedItem.Object.Name}\"");
                return;
            }

            if (droppedItem.Drop.IsGold)
            {
                int droppedGoldAmount = droppedItem.Drop.Item.Quantity;

                if (_playerDataSystem.IncreaseGold(_player, droppedGoldAmount))
                {
                    _textPacketFactory.SendDefinedText(_player, DefineText.TID_GAME_REAPMONEY, droppedGoldAmount.ToString("###,###,###,###"), _player.PlayerData.Gold.ToString("###,###,###,###"));
                }
            }
            else
            {
                _inventorySystem.CreateItem(_player, droppedItem.Drop.Item, droppedItem.Drop.Item.Quantity);
                _textPacketFactory.SendDefinedText(_player, DefineText.TID_GAME_REAPITEM, $"\"{droppedItem.Object.Name}\"");
            }

            _moverPacketFactory.SendMotion(_player, ObjectMessageType.OBJMSG_PICKUP);
            droppedItem.Delete();
        }

        /// <summary>
        /// Process Idle heal logic when player is not fighting.
        /// </summary>
        /// <param name="player"></param>
        private void ProcessIdleHeal()
        {
            if (_player.Timers.NextHealTime <= Time.TimeInSeconds() && !_player.Battle.IsFighting)
            {
                _recoverySystem.IdleRecevory(_player, isSitted: false);
            }
        }
    }
}
