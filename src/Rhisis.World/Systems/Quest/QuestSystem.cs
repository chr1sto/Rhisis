using Microsoft.Extensions.Logging;
using Rhisis.Core.Common;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Resources;
using Rhisis.Core.Structures.Game.Dialogs;
using Rhisis.Core.Structures.Game.Quests;
using Rhisis.Database;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using Rhisis.World.Packets;
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
        private readonly IQuestPacketFactory _questPacketFactory;
        private readonly INpcDialogPacketFactory _npcDialogPacketFactory;
        private readonly ITextPacketFactory _textPacketFactory;

        public QuestSystem(ILogger<QuestSystem> logger, IDatabase database, IGameResources gameResources, IQuestPacketFactory questPacketFactory, INpcDialogPacketFactory npcDialogPacketFactory, ITextPacketFactory textPacketFactory)
        {
            this._logger = logger;
            this._database = database;
            this._gameResources = gameResources;
            this._questPacketFactory = questPacketFactory;
            this._npcDialogPacketFactory = npcDialogPacketFactory;
            this._textPacketFactory = textPacketFactory;
        }

        /// <inheritdoc />
        public void Initialize(IPlayerEntity player)
        {
            IEnumerable<QuestInfo> playerQuests = this._database.Quests.GetAll(x => x.CharacterId == player.PlayerData.Id)
                .Select(x => new QuestInfo(x.QuestId, x.CharacterId, x.Id)
                {
                    IsChecked = x.IsChecked,
                    IsFinished = x.Finished,
                    StartTime = x.StartTime
                });

            player.QuestDiary = new QuestDiaryComponent(playerQuests);
        }

        /// <inheritdoc />
        public void Save(IPlayerEntity player)
        {
            // TODO
        }

        /// <inheritdoc />
        public bool CanStartQuest(IPlayerEntity player, IQuestScript quest)
        {
            if (player.QuestDiary.HasQuest(quest.Id))
            {
                return false;
            }

            if (player.Object.Level < quest.StartRequirements.MinLevel || player.Object.Level > quest.StartRequirements.MaxLevel)
            {
                this._logger.LogTrace($"Cannot start quest '{quest.Title}' (id: '{quest.Id}') for player: '{player}'. Level too low or too high.");
                return false;
            }

            if (quest.StartRequirements.Jobs != null && !quest.StartRequirements.Jobs.Contains((DefineJob.Job)player.PlayerData.JobId))
            {
                this._logger.LogTrace($"Cannot start quest '{quest.Title}' (id: '{quest.Id}') for player: '{player}'. Invalid job.");
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

                // TODO: Check killed monsters
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
            QuestInfo quest = player.QuestDiary.ActiveQuests.FirstOrDefault(x => x.QuestId == questId);

            if (quest == null)
            {
                throw new ArgumentNullException(nameof(quest), $"Cannot find quest with id '{questId}' for player '{player}'.");
            }

            if (quest.IsChecked == checkedState)
            {
                throw new InvalidOperationException($"{player} tried to hack quest check state.");
            }

            quest.IsChecked = !quest.IsChecked;

            this._questPacketFactory.SendCheckedQuests(player, player.QuestDiary.GetCheckedQuests());
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
            this.SendQuestDialog(player, npc, quest.Id, quest.AcceptedDialogs, OkButtons);

            var acceptedQuest = new QuestInfo(quest.Id, player.PlayerData.Id)
            {
                StartTime = DateTime.UtcNow,
            };

            player.QuestDiary.ActiveQuests.Add(acceptedQuest);

            this._questPacketFactory.SendQuest(player, acceptedQuest);
            this._textPacketFactory.SendDefinedText(player, DefineText.TID_EVE_STARTQUEST, _gameResources.GetText(quest.Title));
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
            this._logger.LogDebug($"Finalize quest '{quest.Name}' for player '{player}'.");

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
