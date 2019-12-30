using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rhisis.Core.Common;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Resources;
using Rhisis.Core.Structures.Game;
using Rhisis.Core.Structures.Game.Dialogs;
using Rhisis.Core.Structures.Game.Quests;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using Rhisis.World.Packets;
using Rhisis.World.Systems.PlayerData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Systems.Quest
{
    [Injectable]
    public sealed class QuestSystem : IQuestSystem
    {
        private static readonly IEnumerable<DialogLink> AcceptDeclineButtons = new List<DialogLink>
        {
            new DialogLink(QuestStateType.BeginYes.ToString(), DialogConstants.Yes),
            new DialogLink(QuestStateType.BeginNo.ToString(), DialogConstants.No)
        };
        private static readonly IEnumerable<DialogLink> OkButtons = new List<DialogLink>
        {
            new DialogLink(DialogConstants.Bye, DialogConstants.Ok, 0)
        };
        private static readonly IEnumerable<DialogLink> FinishButtons = new List<DialogLink>
        {
            new DialogLink(QuestStateType.EndCompleted.ToString(), DialogConstants.Ok)
        };

        private readonly ILogger<QuestSystem> _logger;
        private readonly IDatabase _database;
        private readonly IGameResources _gameResources;
        private readonly IPlayerDataSystem _playerDataSystem;
        private readonly IQuestPacketFactory _questPacketFactory;
        private readonly INpcDialogPacketFactory _npcDialogPacketFactory;
        private readonly ITextPacketFactory _textPacketFactory;

        public QuestSystem(ILogger<QuestSystem> logger, IDatabase database, IGameResources gameResources, IPlayerDataSystem playerDataSystem, IQuestPacketFactory questPacketFactory, INpcDialogPacketFactory npcDialogPacketFactory, ITextPacketFactory textPacketFactory)
        {
            this._logger = logger;
            this._database = database;
            this._gameResources = gameResources;
            this._playerDataSystem = playerDataSystem;
            this._questPacketFactory = questPacketFactory;
            this._npcDialogPacketFactory = npcDialogPacketFactory;
            this._textPacketFactory = textPacketFactory;
        }

        /// <inheritdoc />
        public void Initialize(IPlayerEntity player)
        {
            IEnumerable<QuestInfo> playerQuests = this._database.Quests.GetCharactersQuests(player.PlayerData.Id)
                .AsQueryable()
                .AsNoTracking()
                .AsEnumerable()
                .Select(x =>
                {
                    IQuestScript questScript = _gameResources.Quests.GetValueOrDefault(x.QuestId);

                    if (questScript == null)
                    {
                        return null;
                    }

                    var quest = new QuestInfo(x.QuestId, x.CharacterId, x.Id)
                    {
                        IsChecked = x.IsChecked,
                        IsFinished = x.Finished,
                        StartTime = x.StartTime,
                        Monsters = new Dictionary<int, short>
                        {
                            { _gameResources.GetDefinedValue(questScript.EndConditions.Monsters.ElementAtOrDefault(0)?.Id), (short)x.MonsterKilled1 },
                            { _gameResources.GetDefinedValue(questScript.EndConditions.Monsters.ElementAtOrDefault(1)?.Id), (short)x.MonsterKilled2 }
                        },
                        IsPatrolDone = x.IsPatrolDone
                    };

                    return quest;
                })
                .Where(x => x != null);

            player.QuestDiary = new QuestDiaryComponent(playerQuests);
        }

        /// <inheritdoc />
        public void Save(IPlayerEntity player)
        {
            var questsSet = from x in _database.Quests.GetCharactersQuests(player.PlayerData.Id).ToList()
                            join q in player.QuestDiary on
                             new { x.QuestId, x.CharacterId }
                             equals
                             new { q.QuestId, q.CharacterId }
                            select new { DbQuest = x, PlayerQuest = q };

            foreach (var questSet in questsSet)
            {
                questSet.DbQuest.IsChecked = questSet.PlayerQuest.IsChecked;
                questSet.DbQuest.IsDeleted = questSet.PlayerQuest.IsDeleted;
                questSet.DbQuest.IsPatrolDone = questSet.PlayerQuest.IsPatrolDone;
                questSet.DbQuest.Finished = questSet.PlayerQuest.IsFinished;

                if (questSet.PlayerQuest.Monsters != null)
                {
                    questSet.DbQuest.MonsterKilled1 = questSet.PlayerQuest.Monsters.ElementAtOrDefault(0).Value;
                    questSet.DbQuest.MonsterKilled2 = questSet.PlayerQuest.Monsters.ElementAtOrDefault(1).Value;
                }

                _database.Quests.Update(questSet.DbQuest);
            }

            foreach (QuestInfo quest in player.QuestDiary)
            {
                if (!quest.DatabaseQuestId.HasValue)
                {
                    this._database.Quests.Create(new DbQuest
                    {
                        CharacterId = player.PlayerData.Id,
                        QuestId = quest.QuestId,
                        StartTime = quest.StartTime,
                        MonsterKilled1 = quest.Monsters.ElementAtOrDefault(0).Value,
                        MonsterKilled2 = quest.Monsters.ElementAtOrDefault(1).Value,
                        IsPatrolDone = quest.IsPatrolDone,
                        IsChecked = quest.IsChecked
                    });
                }
            }

            this._database.Complete();
        }

        /// <inheritdoc />
        public bool CanStartQuest(IPlayerEntity player, IQuestScript quest)
        {
            if (player.QuestDiary.HasQuest(quest.Id))
            {
                return false;
            }

            int previousQuestId = this._gameResources.GetDefinedValue(quest.StartRequirements.PreviousQuestId);

            if (previousQuestId != 0 && !player.QuestDiary.CompletedQuests.Any(x => x.QuestId == previousQuestId))
            {
                this._logger.LogTrace($"Cannot start quest '{quest.Name}' (id: '{quest.Id}') for player '{player}'. Did not finished quest '{quest.StartRequirements.PreviousQuestId}'.");
                return false;
            }

            if (player.Object.Level < quest.StartRequirements.MinLevel || player.Object.Level > quest.StartRequirements.MaxLevel)
            {
                this._logger.LogTrace($"Cannot start quest '{quest.Name}' (id: '{quest.Id}') for player: '{player}'. Level too low or too high.");
                return false;
            }

            if (quest.StartRequirements.Jobs != null && !quest.StartRequirements.Jobs.Contains((DefineJob.Job)player.PlayerData.JobId))
            {
                this._logger.LogTrace($"Cannot start quest '{quest.Name}' (id: '{quest.Id}') for player: '{player}'. Invalid job.");
                return false;
            }

            // TODO: add more checks

            return true;
        }

        /// <inheritdoc />
        public bool CanFinishQuest(IPlayerEntity player, IQuestScript quest)
        {
            QuestInfo questToFinish = player.QuestDiary.ActiveQuests.FirstOrDefault(x => x.QuestId == quest.Id);

            if (questToFinish != null)
            {
                // Check items
                if (quest.EndConditions.Items != null && quest.EndConditions.Items.Any())
                {
                    foreach (QuestItem questItem in quest.EndConditions.Items)
                    {
                        if (questItem.Sex == GenderType.Any || questItem.Sex == player.PlayerData.Gender)
                        {
                            Item inventoryItem = player.Inventory.GetItemById(this._gameResources.GetDefinedValue(questItem.Id));

                            if (inventoryItem == null || (inventoryItem != null && inventoryItem.Quantity < questItem.Quantity))
                            {
                                return false;
                            }
                        }
                    }
                }

                // Check monsters
                if (quest.EndConditions.Monsters != null && quest.EndConditions.Monsters.Any())
                {
                    foreach (QuestMonster questMonster in quest.EndConditions.Monsters)
                    {
                        int questMonsterId = _gameResources.GetDefinedValue(questMonster.Id);

                        if (questToFinish.Monsters.TryGetValue(questMonsterId, out short killedQuantity))
                        {
                            if (killedQuantity < questMonster.Amount)
                            {
                                return false;
                            }
                        }
                    }
                }
            
                // TODO: Check region patrols
            }

            return true;
        }

        /// <inheritdoc />
        public void ProcessQuest(IPlayerEntity player, INpcEntity npc, IQuestScript quest, QuestStateType state)
        {
            switch (state)
            {
                case QuestStateType.Suggest:
                    this.SuggestQuest(player, npc, quest);
                    break;
                case QuestStateType.BeginYes:
                    this.AcceptQuest(player, npc, quest);
                    break;
                case QuestStateType.BeginNo:
                    this.DeclineQuest(player, npc, quest);
                    break;
                case QuestStateType.End:
                    this.FinishQuest(player, npc, quest);
                    break;
                case QuestStateType.EndCompleted:
                    this.FinalizeQuest(player, npc, quest);
                    break;
                default:
                    this._logger.LogError($"Received unknown dialog quest state: {state}.");
                    break;
            }
        }

        /// <inheritdoc />
        public void CheckQuest(IPlayerEntity player, int questId, bool checkedState)
        {
            QuestInfo quest = player.QuestDiary.GetActiveQuest(questId);

            if (quest == null)
            {
                throw new ArgumentNullException(nameof(quest), $"Cannot find quest with id '{questId}' for player '{player}'.");
            }

            if (quest.IsChecked == checkedState)
            {
                throw new InvalidOperationException($"{player} tried to hack quest check state.");
            }

            quest.IsChecked = !quest.IsChecked;

            this._questPacketFactory.SendCheckedQuests(player, player.QuestDiary.CheckedQuests);
        }

        /// <inheritdoc />
        public void SendQuestsInfo(IPlayerEntity player, INpcEntity npc)
        {
            if (npc.Quests.Any())
            {
                IEnumerable<DialogLink> newQuestsLinks = from x in npc.Quests
                                                         where this.CanStartQuest(player, x)
                                                         select this.CreateQuestLink(x, QuestStateType.Suggest);
                IEnumerable<DialogLink> questsInProgress = from x in npc.Quests
                                                           where player.QuestDiary.ActiveQuests.Any(y => y.QuestId == x.Id)
                                                           select this.CreateQuestLink(x, QuestStateType.End);

                this._npcDialogPacketFactory.SendQuestDialogs(player, newQuestsLinks, questsInProgress);
            }
        }

        /// <inheritdoc />
        public void UpdateQuestDiary(IPlayerEntity player, QuestActionType actionType, params object[] values)
        {
            foreach (QuestInfo quest in player.QuestDiary.ActiveQuests)
            {
                if (!_gameResources.Quests.TryGetValue(quest.QuestId, out IQuestScript questScript))
                {
                    return;
                }

                bool updateQuest = false;

                if (actionType == QuestActionType.KillMonster)
                {
                    int killedMonsterId = Convert.ToInt32(values.ElementAtOrDefault(0));
                    short killedAmount = Convert.ToInt16(values.ElementAtOrDefault(1));

                    QuestMonster monsterToKill = questScript.EndConditions.Monsters.FirstOrDefault(x => _gameResources.GetDefinedValue(x.Id) == killedMonsterId);

                    if (quest.Monsters.ContainsKey(killedMonsterId) && quest.Monsters[killedMonsterId] < monsterToKill.Amount)
                    {
                        quest.Monsters[killedMonsterId] += killedAmount;
                        updateQuest = true;
                    }
                }

                if (actionType == QuestActionType.Patrol)
                {
                    // TODO
                }

                if (updateQuest)
                {
                    this._questPacketFactory.SendQuest(player, quest);
                }
            }
        }

        /// <summary>
        /// Suggest a quest to the current player.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="npc">Npc holding the quest.</param>
        /// <param name="quest">Quest to suggest.</param>
        private void SuggestQuest(IPlayerEntity player, INpcEntity npc, IQuestScript quest)
        {
            this.SendQuestDialog(player, npc, quest.Id, quest.BeginDialogs, AcceptDeclineButtons);
        }

        /// <summary>
        /// Accepts a quest.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="npc">Npc holding the quest.</param>
        /// <param name="quest">Quest to accept.</param>
        private void AcceptQuest(IPlayerEntity player, INpcEntity npc, IQuestScript quest)
        {
            var acceptedQuest = new QuestInfo(quest.Id, player.PlayerData.Id)
            {
                StartTime = DateTime.UtcNow,
                Monsters = quest.EndConditions.Monsters?.ToDictionary(x => _gameResources.GetDefinedValue(x.Id), x => (short)0)
            };

            player.QuestDiary.Add(acceptedQuest);

            this._questPacketFactory.SendQuest(player, acceptedQuest);
            this._textPacketFactory.SendDefinedText(player, DefineText.TID_EVE_STARTQUEST, $"\"{_gameResources.GetText(quest.Title)}\"");
            this.SendQuestDialog(player, npc, quest.Id, quest.AcceptedDialogs, OkButtons);
        }

        /// <summary>
        /// Declines a quest suggestion.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="npc">Npc holding the quest.</param>
        /// <param name="quest">Declined quest.</param>
        private void DeclineQuest(IPlayerEntity player, INpcEntity npc, IQuestScript quest)
        {
            this.SendQuestDialog(player, npc, quest.Id, quest.DeclinedDialogs, OkButtons);
        }

        /// <summary>
        /// Verify that the player can finish the quest.
        /// If the player can finish the quest, sends the completed dialog.
        /// If not, the failure dialog is sent.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="npc">Current npc.</param>
        /// <param name="quest">Current quest.</param>
        private void FinishQuest(IPlayerEntity player, INpcEntity npc, IQuestScript quest)
        {
            if (!CanFinishQuest(player, quest))
            {
                SendQuestDialog(player, npc, quest.Id, quest.NotFinishedDialogs, OkButtons);
                return;
            }

            SendQuestDialog(player, npc, quest.Id, quest.CompletedDialogs, FinishButtons);
        }

        /// <summary>
        /// Finalize the quest and give the reward to the player.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="npc">Current npc.</param>
        /// <param name="quest">Current quest.</param>
        private void FinalizeQuest(IPlayerEntity player, INpcEntity npc, IQuestScript quest)
        {
            QuestInfo questToFinish = player.QuestDiary.GetActiveQuest(quest.Id);

            if (questToFinish == null)
            {
                _logger.LogError($"Cannot find quest with id '{quest.Id}' for player '{player}'.");
                return;
            }

            // Check if player has enough space for reward items.
            if (quest.Rewards.Items != null && quest.Rewards.Items.Any())
            {
                IEnumerable<QuestItem> itemsForPlayer = quest.Rewards.Items.Where(x => x.Sex == player.PlayerData.Gender);

                if (player.Inventory.GetItemCount() + itemsForPlayer.Count() > player.Inventory.MaxStorageCapacity)
                {
                    _textPacketFactory.SendDefinedText(player, DefineText.TID_QUEST_NOINVENTORYSPACE);
                    return;
                }

                foreach (QuestItem rewardItem in itemsForPlayer)
                {
                    int rewardItemId = _gameResources.GetDefinedValue(rewardItem.Id);

                    if (_gameResources.Items.TryGetValue(rewardItemId, out ItemData itemData))
                    {
                        player.Inventory.CreateItem(new Item(itemData.Id, 0, 0, 0, itemData, -1));
                    }
                }
            }

            int goldRewad = quest.Rewards.Gold;

            if (this._playerDataSystem.IncreaseGold(player, goldRewad))
            {
                this._textPacketFactory.SendDefinedText(player, DefineText.TID_GAME_REAPMONEY, goldRewad.ToString("###,###,###,###"), player.PlayerData.Gold.ToString("###,###,###,###"));
            }

            questToFinish.IsFinished = true;
            questToFinish.State = QuestStateType.Completed;

            _textPacketFactory.SendDefinedText(player, DefineText.TID_EVE_ENDQUEST, $"\"{_gameResources.GetText(quest.Title)}\"");
            _questPacketFactory.SendQuest(player, questToFinish);

            // TODO: if the NPC has another quest to suggest, send quest suggestion and do not close the dialog box
            this._npcDialogPacketFactory.SendCloseDialog(player);
        }

        /// <summary>
        /// Creates a new quest <see cref="DialogLink"/>.
        /// </summary>
        /// <param name="quest">Quest.</param>
        /// <param name="questState">Quest state.</param>
        /// <returns>Quest <see cref="DialogLink"/>.</returns>
        private DialogLink CreateQuestLink(IQuestScript quest, QuestStateType questState)
            => new DialogLink(questState.ToString(), this._gameResources.GetText(quest.Title), quest.Id);

        /// <summary>
        /// Gets the quest dialog texts.
        /// </summary>
        /// <param name="questDialogsKeys">Quest dialog keys.</param>
        /// <returns>Quest dialog texts.</returns>
        private IEnumerable<string> GetQuestDialogsTexts(IEnumerable<string> questDialogsKeys)
            => questDialogsKeys.Select(x => this._gameResources.GetText(x));

        /// <summary>
        /// Sends a quest dialog
        /// </summary>
        /// <param name="player"></param>
        /// <param name="npc"></param>
        /// <param name="questId"></param>
        /// <param name="questTexts"></param>
        /// <param name="buttons"></param>
        private void SendQuestDialog(IPlayerEntity player, INpcEntity npc, int questId, IEnumerable<string> questTexts, IEnumerable<DialogLink> buttons)
        {
            this._npcDialogPacketFactory.SendDialog(player, GetQuestDialogsTexts(questTexts), npc.Data.Dialog.Links, buttons, questId);
            this.SendQuestsInfo(player, npc);
        }
    }
}
