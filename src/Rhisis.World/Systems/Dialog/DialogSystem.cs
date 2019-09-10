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

            if (npcEntity.Quests.Any())
            {
                IEnumerable<DialogLink> questLinks = from x in npcEntity.Quests
                                                     where this._questSystem.CanStartQuest(player, x)
                                                     select this._questSystem.CreateQuestLink(x);
                dialogLinks.AddRange(questLinks);
            }

            if (string.IsNullOrEmpty(dialogKey))
            {
                if (npcEntity.Quests.Count() == 1)
                {
                    this._questSystem.ProcessQuest(player, npcEntity, npcEntity.Quests.First(), QuestStateType.Suggest);
                }
                else
                {
                    this._npcDialogPacketFactory.SendDialog(player, dialogTexts, dialogLinks);
                }
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

                        this._npcDialogPacketFactory.SendDialog(player, dialogTexts, dialogLinks);
                    }
                }
            }

        }
    }
}
