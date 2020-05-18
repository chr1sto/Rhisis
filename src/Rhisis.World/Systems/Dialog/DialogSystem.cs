using Microsoft.Extensions.Logging;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Extensions;
using Rhisis.Core.Structures.Game.Dialogs;
using Rhisis.Core.Structures.Game.Quests;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Quest;
using System;
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
            _logger = logger;
            _chatPacketFactory = chatPacketFactory;
            _npcDialogPacketFactory = npcDialogPacketFactory;
            _questSystem = questSystem;
        }

        /// <inheritdoc />
        public void OpenNpcDialog(IPlayerEntity player, uint npcObjectId, string dialogKey, int questId = 0)
        {
            var npc = player.FindEntity<INpcEntity>(npcObjectId);

            if (npc == null)
            {
                _logger.LogError($"Cannot find NPC with id: {npcObjectId}.");
                return;
            }

            _logger.LogTrace($"{npc.Object.Name}: '{dialogKey}'/{questId}");

            if (string.IsNullOrEmpty(dialogKey))
            {
                if (npc.Quests.Any())
                {
                    IEnumerable<IQuestScript> availableQuests = npc.Quests.Where(x => _questSystem.CanStartQuest(player, npc, x));

                    if (availableQuests.Count() == 1)
                    {
                        IQuestScript firstQuest = availableQuests.First();
                        var questState = _questSystem.CanStartQuest(player, npc, firstQuest) ? QuestStateType.Suggest : QuestStateType.End;

                        _questSystem.ProcessQuest(player, npc, firstQuest, questState);
                        return;
                    }
                }

                IEnumerable<IQuestScript> questsToBeFinished = player.QuestDiary.ActiveQuests.Where(x => npc.Object.Name.Equals(x.Script.EndCharacter, StringComparison.OrdinalIgnoreCase)).Select(x => x.Script);

                if (questsToBeFinished.Any())
                {
                    _questSystem.ProcessQuest(player, npc, questsToBeFinished.First(), QuestStateType.End);
                    return;
                }

                if (!npc.NpcData.HasDialog)
                {
                    _logger.LogError($"NPC '{npc.Object.Name}' doesn't have a dialog.");
                    return;
                }

                SendNpcDialog(player, npc, npc.NpcData.Dialog.IntroText, npc.NpcData.Dialog.Links);
            }
            else
            {
                if (questId != 0)
                {
                    // Check if the quest exists for the current NPC
                    IQuestScript quest = npc.Quests.FirstOrDefault(x => x.Id == questId);

                    if (quest == null)
                    {
                        // If not, check if the npc is the end character of player's active quest
                        quest = player.QuestDiary.ActiveQuests.Where(x => x.QuestId == questId && npc.Object.Name.Equals(x.Script.EndCharacter, StringComparison.OrdinalIgnoreCase)).Select(x => x.Script).FirstOrDefault();

                        if (quest == null)
                        {
                            _logger.LogError($"Cannot find quest with id '{questId}' at npc '{npc}' for player '{player}'.");
                            return;
                        }
                    }

                    _questSystem.ProcessQuest(player, npc, quest, dialogKey.ToEnum<QuestStateType>());
                }
                else
                {
                    if (!npc.NpcData.HasDialog)
                    {
                        _logger.LogError($"NPC '{npc.Object.Name}' doesn't have a dialog.");

                        if (dialogKey == DialogConstants.Bye)
                        {
                            _npcDialogPacketFactory.SendCloseDialog(player);
                        }

                        return;
                    }

                    if (dialogKey == DialogConstants.Bye)
                    {
                        _chatPacketFactory.SendChatTo(npc, player, npc.NpcData.Dialog.ByeText);
                        _npcDialogPacketFactory.SendCloseDialog(player);
                        return;
                    }
                    else
                    {
                        DialogLink dialogLink = npc.NpcData.Dialog.Links?.FirstOrDefault(x => x.Id == dialogKey);

                        if (dialogLink == null)
                        {
                            _logger.LogError($"Cannot find dialog key: '{dialogKey}' for NPC '{npc.Object.Name}'.");
                            return;
                        }

                        SendNpcDialog(player, npc, dialogLink.Texts, npc.NpcData.Dialog.Links);
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
            _npcDialogPacketFactory.SendDialog(player, texts, links);
            _questSystem.SendQuestsInfo(player, npc);
        }
    }
}
