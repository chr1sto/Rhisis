using Microsoft.Extensions.Logging;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Structures.Game.Dialogs;
using Rhisis.Core.Structures.Game.Quests;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Systems.Quest
{
    [Injectable]
    public sealed class QuestSystem : IQuestSystem
    {
        private readonly ILogger<QuestSystem> _logger;
        private readonly IDatabase _database;
        private readonly IQuestPacketFactory _questPacketFactory;
        private readonly INpcDialogPacketFactory _npcDialogPacketFactory;
        private readonly ITextPacketFactory _textPacketFactory;

        public QuestSystem(ILogger<QuestSystem> logger, IDatabase database, IQuestPacketFactory questPacketFactory, INpcDialogPacketFactory npcDialogPacketFactory, ITextPacketFactory textPacketFactory)
        {
            this._logger = logger;
            this._database = database;
            this._questPacketFactory = questPacketFactory;
            this._npcDialogPacketFactory = npcDialogPacketFactory;
            this._textPacketFactory = textPacketFactory;
        }

        /// <inheritdoc />
        public void Initialize(IPlayerEntity player)
        {
            // TODO: Initialize player quests.
        }

        /// <inheritdoc />
        public bool CanStartQuest(IPlayerEntity player, IQuestScript quest)
        {
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
        public void ProcessQuest(IPlayerEntity player, INpcEntity npc, IQuestScript quest, QuestStateType state)
        {
            switch (state)
            {
                case QuestStateType.Suggest:
                    this._logger.LogDebug($"Suggest quest '{quest.Title}' to '{player}'.");
                    this.SuggestQuest(player, npc, quest);
                    break;
                case QuestStateType.BeginYes:
                    this.AcceptQuest(player, npc, quest);
                    break;
                case QuestStateType.BeginNo:
                    this.DeclineQuest(player, npc, quest);
                    break;
                default:
                    this._logger.LogError($"Received unknown dialog quest state: {state}.");
                    break;
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
            var dialogLinks = new List<DialogLink>(npc.Data.Dialog.Links);
            dialogLinks.AddRange(npc.Quests.Where(x => this.CanStartQuest(player, x)).Select(x => this.CreateQuestLink(x)));

            var questAnswersButtons = new List<DialogLink>
            {
                new DialogLink(QuestStateType.BeginYes.ToString(), DialogConstants.Yes, quest.Id),
                new DialogLink(QuestStateType.BeginNo.ToString(), DialogConstants.No, quest.Id)
            };

            this._npcDialogPacketFactory.SendDialog(player, quest.BeginDialogs, dialogLinks, questAnswersButtons, quest.Id);
        }

        /// <summary>
        /// Accepts a quest.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="npc">Npc holding the quest.</param>
        /// <param name="quest">Quest to accept.</param>
        private void AcceptQuest(IPlayerEntity player, INpcEntity npc, IQuestScript quest)
        {
            var dialogLinks = new List<DialogLink>(npc.Data.Dialog.Links);
            dialogLinks.AddRange(npc.Quests.Where(x => this.CanStartQuest(player, x)).Select(x => this.CreateQuestLink(x)));

            var questAnswersButtons = new List<DialogLink>
            {
                new DialogLink(DialogConstants.Bye, DialogConstants.Ok)
            };

            this._npcDialogPacketFactory.SendDialog(player, quest.AcceptedDialogs, dialogLinks, questAnswersButtons, quest.Id);

            var newDatabaseQuest = new DbQuest
            {
                QuestId = quest.Id,
                StartTime = DateTime.UtcNow,
                CharacterId = player.PlayerData.Id
            };

            this._database.Quests.Create(newDatabaseQuest);
            this._database.Complete();

            var acceptedQuest = new Game.Structures.Quest(quest.Id, newDatabaseQuest.Id, player.PlayerData.Id);

            player.QuestDiary.ActiveQuests.Add(acceptedQuest);

            this._questPacketFactory.SendQuest(player, acceptedQuest);
            this._textPacketFactory.SendDefinedText(player, DefineText.TID_EVE_STARTQUEST, quest.Title);
        }

        /// <summary>
        /// Declines a quest suggestion.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="npc">Npc holding the quest.</param>
        /// <param name="quest">Declined quest.</param>
        private void DeclineQuest(IPlayerEntity player, INpcEntity npc, IQuestScript quest)
        {
            var dialogLinks = new List<DialogLink>(npc.Data.Dialog.Links);
            dialogLinks.AddRange(npc.Quests.Where(x => this.CanStartQuest(player, x)).Select(x => this.CreateQuestLink(x)));

            var questAnswersButtons = new List<DialogLink>
            {
                new DialogLink(DialogConstants.Bye, DialogConstants.Ok)
            };

            this._npcDialogPacketFactory.SendDialog(player, quest.DeclinedDialogs, dialogLinks, questAnswersButtons, quest.Id);
        }

        /// <summary>
        /// Creates a new quest <see cref="DialogLink"/>.
        /// </summary>
        /// <param name="quest">Quest.</param>
        /// <returns>Quest <see cref="DialogLink"/>.</returns>
        public DialogLink CreateQuestLink(IQuestScript quest) 
            => new DialogLink(QuestStateType.Suggest.ToString(), quest.Title, quest.Id);
    }
}
