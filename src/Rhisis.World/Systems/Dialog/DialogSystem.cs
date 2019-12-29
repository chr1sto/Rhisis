using Microsoft.Extensions.Logging;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Extensions;
using Rhisis.Core.Structures.Game.Dialogs;
using Rhisis.Core.Structures.Game.Quests;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Quest;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Systems.Dialog
{
    [Injectable]
    public sealed class DialogSystem : IDialogSystem
    {
        private readonly ILogger<DialogSystem> _logger;
        private readonly IChatPacketFactory _chatPacketFactory;
        private readonly INpcDialogPacketFactory _npcDialogPacketFactory;
        private readonly IQuestSystem _questSystem;

        /// <summary>
        /// Creates a new <see cref="DialogSystem"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="chatPacketFactory">Chat packet factory.</param>
        /// <param name="npcDialogPacketFactory">Dialog packet factory.</param>
        /// <param name="questSystem">Quest system.</param>
        public DialogSystem(ILogger<DialogSystem> logger, IChatPacketFactory chatPacketFactory, INpcDialogPacketFactory npcDialogPacketFactory, IQuestSystem questSystem)
        {
            this._logger = logger;
            this._chatPacketFactory = chatPacketFactory;
            this._npcDialogPacketFactory = npcDialogPacketFactory;
            this._questSystem = questSystem;
        }

        /// <inheritdoc />
        public void OpenNpcDialog(IPlayerEntity player, uint npcObjectId, string dialogKey, int questId = 0)
        {
            var npcEntity = player.FindEntity<INpcEntity>(npcObjectId);

            if (npcEntity == null)
            {
                this._logger.LogError($"Cannot find NPC with id: {npcObjectId}.");
                return;
            }

            this._logger.LogTrace($"{npcEntity.Object.Name}: '{dialogKey}'/{questId}");

            if (!npcEntity.Data.HasDialog)
            {
                this._logger.LogError($"NPC '{npcEntity.Object.Name}' doesn't have a dialog.");
                return;
            }

            IEnumerable<string> dialogTexts = npcEntity.Data.Dialog.IntroText;
            var dialogLinks = new List<DialogLink>(npcEntity.Data.Dialog.Links);

            if (string.IsNullOrEmpty(dialogKey))
            {
                if (npcEntity.Quests.Any())
                {
                    IEnumerable<IQuestScript> availableQuests = npcEntity.Quests.Where(x => this._questSystem.CanStartQuest(player, x));

                    if (availableQuests.Count() == 1)
                    {
                        IQuestScript firstQuest = availableQuests.First();
                        var questState = this._questSystem.CanStartQuest(player, firstQuest) ? QuestStateType.Suggest : QuestStateType.End;

                        this._questSystem.ProcessQuest(player, npcEntity, firstQuest, questState);
                        return;
                    }
                }

                this.SendNpcDialog(player, npcEntity, dialogTexts, dialogLinks);
            }
            else
            {
                if (questId != 0)
                {
                    IQuestScript quest = npcEntity.Quests.FirstOrDefault(x => x.Id == questId);

                    if (quest == null)
                    {
                        this._logger.LogError($"Cannot find quest with id '{questId}' at npc '{npcEntity}' for player '{player}'.");
                        return;
                    }

                    this._questSystem.ProcessQuest(player, npcEntity, quest, dialogKey.ToEnum<QuestStateType>());
                }
                else
                {
                    if (dialogKey == DialogConstants.Bye)
                    {
                        this._chatPacketFactory.SendChatTo(npcEntity, player, npcEntity.Data.Dialog.ByeText);
                        this._npcDialogPacketFactory.SendCloseDialog(player);
                        return;
                    }
                    else
                    {
                        DialogLink dialogLink = npcEntity.Data.Dialog.Links?.FirstOrDefault(x => x.Id == dialogKey);

                        if (dialogLink == null)
                        {
                            this._logger.LogError($"Cannot find dialog key: '{dialogKey}' for NPC '{npcEntity.Object.Name}'.");
                            return;
                        }

                        dialogTexts = dialogLink.Texts;

                        this.SendNpcDialog(player, npcEntity, dialogTexts, dialogLinks);
                    }
                }
            }

        }

        /// <summary>
        /// Sends a NPC dialog to the given player.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="npc">Current npc.</param>
        /// <param name="texts">Npc dialog texts.</param>
        /// <param name="links">Npc dialog links.</param>
        private void SendNpcDialog(IPlayerEntity player, INpcEntity npc, IEnumerable<string> texts, IEnumerable<DialogLink> links)
        {
            this._npcDialogPacketFactory.SendDialog(player, texts, links);
            this._questSystem.SendQuestsInfo(player, npc);
        }
    }
}
