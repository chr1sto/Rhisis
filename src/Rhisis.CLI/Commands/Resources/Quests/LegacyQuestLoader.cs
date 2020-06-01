using Rhisis.Core.Common;
using Rhisis.Core.Data;
using Rhisis.Core.Resources.Include;
using Rhisis.Core.Structures.Game.Quests;
using Rhisis.Scripting.Quests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.CLI.Commands.Game.Quests
{
    internal class LegacyQuestLoader
    {
        private readonly List<QuestData> _quests;
        private readonly IReadOnlyDictionary<string, DefineJob.Job> _jobs = new Dictionary<string, DefineJob.Job>
        {
            // Expert
            { "QUEST_VOCMER_TRN3", DefineJob.Job.JOB_MERCENARY },
            { "QUEST_VOCASS_TRN3", DefineJob.Job.JOB_ASSIST },
            { "QUEST_VOCACR_TRN3", DefineJob.Job.JOB_ACROBAT },
            { "QUEST_VOCMAG_TRN3", DefineJob.Job.JOB_MAGICIAN },

            // Profesionnal
            { "QUEST_HEROKNI_TRN5", DefineJob.Job.JOB_KNIGHT },
            { "QUEST_HEROBLA_TRN5", DefineJob.Job.JOB_BLADE },
            { "QUEST_HEROBIL_TRN5", DefineJob.Job.JOB_BILLPOSTER },
            { "QUEST_HERORIG_TRN5", DefineJob.Job.JOB_RINGMASTER },
            { "QUEST_HERORAN_TRN5", DefineJob.Job.JOB_RANGER },
            { "QUEST_HEROJES_TRN5", DefineJob.Job.JOB_JESTER },
            { "QUEST_HEROPSY_TRN5", DefineJob.Job.JOB_PSYCHIKEEPER },
            { "QUEST_HEROELE_TRN5", DefineJob.Job.JOB_ELEMENTOR },
        };

        /// <summary>
        /// Gets the quests.
        /// </summary>
        public IEnumerable<QuestData> Quests => _quests;

        /// <summary>
        /// Creates a new <see cref="LegacyQuestLoader"/> instance.
        /// </summary>
        public LegacyQuestLoader()
        {
            _quests = new List<QuestData>();
        }

        /// <summary>
        /// Loads an official quest property file.
        /// </summary>
        /// <param name="filePath">Official quest property file path.</param>
        public void Load(string filePath)
        {
            using var questIncludeFile = new IncludeFile(filePath);

            foreach (IStatement questStatement in questIncludeFile.Statements)
            {
                if (!(questStatement is Block questBlock))
                    continue;

                QuestData quest = CreateQuest(questBlock);

                if (IsQuestValid(quest))
                    _quests.Add(quest);
            }
        }

        /// <summary>
        /// Creates a new <see cref="QuestData"/> based on an official quest block instruction.
        /// </summary>
        /// <param name="questBlock"></param>
        /// <returns></returns>
        private QuestData CreateQuest(Block questBlock)
        {
            var quest = new QuestData
            {
                Name = int.TryParse(questBlock.Name, out int _) ? $"{QuestScriptConstants.QuestPrefix}{questBlock.Name}" : questBlock.Name,
                Title = questBlock.GetInstruction("SetTitle")?.GetParameter<string>(0)
            };

            Block questSettingsBlock = questBlock.GetBlockByName("setting");

            if (questSettingsBlock != null)
            {
                quest.StartCharacter = questSettingsBlock.GetInstruction("SetCharacter")?.GetParameter<string>(0);
                quest.EndCharacter = questSettingsBlock.GetInstruction("SetEndCondCharacter")?.GetParameter<string>(0);
                quest.MinLevel = questSettingsBlock.GetInstruction("SetBeginCondLevel")?.GetParameter<int>(0) ?? default;
                quest.MaxLevel = questSettingsBlock.GetInstruction("SetBeginCondLevel")?.GetParameter<int>(1) ?? default;
                quest.StartJobs = questSettingsBlock.GetInstruction("SetBeginCondJob")?.Parameters.Select(x => x.ToString()).ToArray();
                quest.PreviousQuestId = questSettingsBlock.GetInstruction("SetBeginCondPreviousQuest")?.GetParameter<string>(1) ?? default;

                LoadEndConditions(quest, questSettingsBlock);
                LoadQuestRewards(quest, questSettingsBlock);
            }

            LoadQuestDialogs(quest, questBlock);
            SetQuestJobChange(quest);

            return quest;
        }

        private void LoadEndConditions(QuestData quest, Block settingsBlock)
        {
            // Load items
            IEnumerable<Instruction> questEndItems = settingsBlock.GetInstructions("SetEndCondItem");

            IEnumerable<QuestItem> itemsToRemove = settingsBlock.GetInstructions("SetEndRemoveItem").Select(x => new QuestItem
            {
                Id = x.GetParameter<string>(1)
            });
            quest.EndQuestItems = questEndItems.Select(x => new QuestItem
            {
                Sex = x.GetParameter<GenderType>(0),
                Id = x.GetParameter<string>(3),
                Quantity = x.GetParameter<int>(4),
                Remove = itemsToRemove.Any(y => y.Id == x.GetParameter<string>(3))
            }).ToList();

            // Load kill monsters

            IEnumerable<Instruction> questKillNpcs = settingsBlock.GetInstructions("SetEndCondKillNPC");
            quest.EndQuestMonsters = questKillNpcs.OrderBy(x => x.GetParameter<int>(0)).Select(x => new QuestMonster
            {
                Id = x.GetParameter<string>(1),
                Amount = x.GetParameter<int>(2)
            });

            // Load patrols

            IEnumerable<Instruction> questPatrols = settingsBlock.GetInstructions("SetEndCondPatrolZone");
            quest.EndQuestPatrols = questPatrols.Select(x => new QuestPatrol
            {
                MapId = x.GetParameter<string>(0),
                Left = x.GetParameter<int>(1),
                Top = x.GetParameter<int>(2),
                Right = x.GetParameter<int>(3),
                Bottom = x.GetParameter<int>(4)
            });
        }

        private void LoadQuestRewards(QuestData quest, Block settingsBlock)
        {
            Instruction goldReward = settingsBlock.GetInstruction("SetEndRewardGold");
            if (goldReward != null)
            {
                quest.MinGold = goldReward.GetParameter<int>(0);
                quest.MaxGold = goldReward.GetParameter<int>(1);
            }

            Instruction experienceReward = settingsBlock.GetInstruction("SetEndRewardExp");
            if (experienceReward != null)
            {
                quest.MinExp = experienceReward.GetParameter<int>(0);
                quest.MaxExp = experienceReward.GetParameter<int>(1);
            }

            // Load items
            IEnumerable<Instruction> questRewardItems = settingsBlock.GetInstructions("SetEndRewardItem").Concat(settingsBlock.GetInstructions("SetEndRewardItemWithAbilityOption"));
            quest.RewardItems = questRewardItems.Select(x => new QuestItem
            {
                Sex = x.GetParameter<GenderType>(0),
                Id = x.GetParameter<string>(3),
                Quantity = x.GetParameter<int>(4),
                Refine = x.Parameters.Count > 5 ? x.GetParameter<byte>(5) : default
            }).ToList();
        }

        private void LoadQuestDialogs(QuestData quest, Block questBlock)
        {
            IDictionary<QuestDialogKeys, string> dialogs = (from x in questBlock.Statements
                                                            where x.Name == "SetDialog" && x.Type == StatementType.Instruction
                                                            let instruction = x as Instruction
                                                            select KeyValuePair.Create(
                                                                (QuestDialogKeys)Enum.Parse(typeof(QuestDialogKeys), instruction.GetParameter<string>(0)),
                                                                instruction.GetParameter<string>(1))
                                                            ).ToDictionary(x => x.Key, x => x.Value);

            if (dialogs.Any())
            {
                quest.BeginDialogs = dialogs.Where(x => x.Key >= QuestDialogKeys.QSAY_BEGIN1 && x.Key <= QuestDialogKeys.QSAY_BEGIN5).Select(x => x.Value).ToArray();
                quest.AcceptDialogs = dialogs.Where(x => x.Key == QuestDialogKeys.QSAY_BEGIN_YES).Select(x => x.Value).ToArray();
                quest.DeclinedDialogs = dialogs.Where(x => x.Key == QuestDialogKeys.QSAY_BEGIN_NO).Select(x => x.Value).ToArray();
                quest.FailureDialogs = dialogs.Where(x => x.Key >= QuestDialogKeys.QSAY_END_FAILURE1 && x.Key <= QuestDialogKeys.QSAY_END_FAILURE3).Select(x => x.Value).ToArray();
                quest.CompletedDialogs = dialogs.Where(x => x.Key >= QuestDialogKeys.QSAY_END_COMPLETE1 && x.Key <= QuestDialogKeys.QSAY_END_COMPLETE3).Select(x => x.Value).ToArray();
            }
        }

        private bool IsQuestValid(QuestData quest)
        {
            if (string.IsNullOrEmpty(quest.StartCharacter))
            {
                Console.WriteLine($"{quest.Name}: Start charcter is missing.");
                return false;
            }

            if (quest.StartJobs == null || !quest.StartJobs.Any())
            {
                Console.WriteLine($"{quest.Name}: A quest must be linked to a job.");
                return false;
            }

            if (quest.MinLevel <= 0 || quest.MaxLevel <= 0)
            {
                Console.WriteLine($"{quest.Name}: A quest must have a min level and a max level.");
                return false;
            }

            return true;
        }

        private void SetQuestJobChange(QuestData quest)
        {
            if (_jobs.TryGetValue(quest.Name, out DefineJob.Job job))
            {
                quest.RewardJob = job;
            }
        }
    }
}
