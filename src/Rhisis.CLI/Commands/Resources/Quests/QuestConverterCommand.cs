using McMaster.Extensions.CommandLineUtils;
using Rhisis.Core.Resources.Include;
using Rhisis.Core.Structures.Game.Quests;
using Rhisis.Scripting.Quests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rhisis.CLI.Commands.Game.Quests
{
    [Command("convert", Description = "Converts FLYFF quests into Rhisis LUA quests.")]
    public class QuestConverterCommand
    {
        [Argument(0, Description = "Official quest files.")]
        public string[] InputFiles { get; set; }

        [Option(CommandOptionType.SingleValue, ShortName = "o", LongName = "output", Description = "Output directory.")]
        public string Output { get; set; }

        /// <summary>
        /// Loads every input files and creates a LUA script based on it.
        /// </summary>
        public void OnExecute()
        {
            if (this.InputFiles == null)
            {
                return;
            }

            IEnumerable<string> questFiles = GetFilesFromInput();
            this.Output = string.IsNullOrEmpty(this.Output) ? Directory.GetCurrentDirectory() : Path.GetDirectoryName(this.Output);

            var questsSaved = new List<string>();

            foreach (string inputFile in questFiles)
            {
                using var questIncludeFile = new IncludeFile(inputFile);

                foreach (IStatement questStatement in questIncludeFile.Statements)
                {
                    if (!(questStatement is Block questBlock))
                        continue;

                    QuestData quest = CreateQuest(questBlock);

                    if (string.IsNullOrEmpty(quest.StartCharacter))
                    {
                        Console.WriteLine($"{quest.Name}: Start charcter is missing.");
                        continue;
                    }

                    if (quest.StartJobs == null || (quest.StartJobs != null && !quest.StartJobs.Any()))
                    {
                        Console.WriteLine($"{quest.Name}: A quest must be linked to a job.");
                        continue;
                    }

                    if (quest.MinLevel <= 0 || quest.MaxLevel <= 0)
                    {
                        Console.WriteLine($"{quest.Name}: A quest must have a min level and a max level.");
                        continue;
                    }

                    SaveQuestDataAsLua(quest);

                    questsSaved.Add(quest.Name);
                }
            }

            SaveQuestDefinition(questsSaved);
        }

        /// <summary>
        /// Gets the correct file path from the file input.
        /// </summary>
        /// <returns>Collection of file paths.</returns>
        private IEnumerable<string> GetFilesFromInput()
        {
            return InputFiles.Select(x => new
            {
                Path = Path.GetDirectoryName(x),
                Filter = Path.GetFileName(x)
            }).SelectMany(x => Directory.EnumerateFiles(x.Path, x.Filter));
        }

        /// <summary>
        /// Creates a new <see cref="QuestData"/> based on an official quest block instruction.
        /// </summary>
        /// <param name="questBlock"></param>
        /// <returns></returns>
        private QuestData CreateQuest(Block questBlock)
        {
            var quest = new QuestData();

            quest.Name = int.TryParse(questBlock.Name, out int _) ? $"{QuestScriptConstants.QuestPrefix}{questBlock.Name}" : questBlock.Name;
            quest.Title = questBlock.GetInstruction("SetTitle")?.GetParameter<string>(0);

            Block questSettingsBlock = questBlock.GetBlockByName("setting");

            if (questSettingsBlock != null)
            {
                quest.StartCharacter = questSettingsBlock.GetInstruction("SetCharacter")?.GetParameter<string>(0);
                quest.MinLevel = questSettingsBlock.GetInstruction("SetBeginCondLevel")?.GetParameter<int>(0) ?? default;
                quest.MaxLevel = questSettingsBlock.GetInstruction("SetBeginCondLevel")?.GetParameter<int>(1) ?? default;
                quest.StartJobs = questSettingsBlock.GetInstruction("SetBeginCondJob")?.Parameters.Select(x => x.ToString()).ToArray();

                // rewards
                quest.MinGold = questSettingsBlock.GetInstruction("SetEndRewardGold")?.GetParameter<int>(0) ?? default;
                quest.MaxGold = questSettingsBlock.GetInstruction("SetEndRewardGold")?.GetParameter<int>(1) ?? default;
            }

            // Dialogs
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

            return quest;
        }

        /// <summary>
        /// Saves the quest has a LUA script.
        /// </summary>
        /// <param name="quest">Quest to save.</param>
        private void SaveQuestDataAsLua(QuestData quest)
        {
            string filePath = Path.Combine(this.Output, $"{quest.Name}.lua");

            Console.Write($"Saving quest: '{quest.Name}' into '{filePath}'...\r");

            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            using var writer = new StreamWriter(fileStream);

            WriteAtLevel(writer, 0, $"{quest.Name} = {{", includeComma: false);
            WriteMainInformations(writer, quest);
            WriteStartRequirements(writer, quest);
            WriteRewards(writer, quest);
            WriteDialogs(writer, quest);
            WriteAtLevel(writer, 0, "}", includeComma: false);

            Console.WriteLine($"Done: '{quest.Name}' saved into '{filePath}'!");
        }

        /// <summary>
        /// Writes content at a given tabulation level.
        /// </summary>
        /// <param name="writer">Stream writer.</param>
        /// <param name="level">Tabulation level.</param>
        /// <param name="content">Content to write.</param>
        /// <param name="includeComma">Includes a comma at the end of the line if true; doesn't include otherwise.</param>
        private void WriteAtLevel(StreamWriter writer, int level, string content, bool includeComma = true)
        {
            var tabs = new string(Enumerable.Repeat('\t', level).ToArray());
            var comma = includeComma ? "," : string.Empty;

            writer.WriteLine($"{tabs}{content}{comma}");
        }

        /// <summary>
        /// Save the quest definition.
        /// </summary>
        /// <param name="quests">Quests to save.</param>
        private void SaveQuestDefinition(IEnumerable<string> quests)
        {
            string filePath = Path.Combine(this.Output, QuestScriptConstants.QuestDefinitionFile);
            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            using var writer = new StreamWriter(fileStream);

            if (quests == null || (quests != null && !quests.Any()))
            {
                writer.WriteLine($"{QuestScriptConstants.QuestDefinitionKey} = nil");
            }
            else
            {
                writer.WriteLine($"{QuestScriptConstants.QuestDefinitionKey} = {{");
                foreach (string questName in quests)
                {
                    writer.WriteLine($"\t'{questName}',");
                }
                writer.WriteLine("}");
            }
        }

        /// <summary>
        /// Writes the quest main informations to the given LUA stream file.
        /// </summary>
        /// <param name="writer">Stream writer.</param>
        /// <param name="quest">Current quest.</param>
        private void WriteMainInformations(StreamWriter writer, QuestData quest)
        {
            WriteAtLevel(writer, 1, $"{QuestScriptConstants.Title} = '{quest.Title}'");
            WriteAtLevel(writer, 1, $"{QuestScriptConstants.StartCharacter} = '{quest.StartCharacter}'");
        }

        /// <summary>
        /// Writes the quest start requirements to the given LUA stream file.
        /// </summary>
        /// <param name="writer">Stream writer.</param>
        /// <param name="quest">Current quest.</param>
        private void WriteStartRequirements(StreamWriter writer, QuestData quest)
        {
            WriteAtLevel(writer, 1, $"{QuestScriptConstants.StartRequirements} = {{", includeComma: false);
            WriteAtLevel(writer, 2, $"{QuestScriptConstants.MinLevel} = {quest.MinLevel}");
            WriteAtLevel(writer, 2, $"{QuestScriptConstants.MaxLevel} = {quest.MaxLevel}");
            WriteAtLevel(writer, 2, $"{QuestScriptConstants.Job} = {{ {string.Join(", ", quest.StartJobs.Select(x => $"'{x}'"))} }}");
            WriteAtLevel(writer, 1, $"}}");
        }

        /// <summary>
        /// Writes the quest rewards to the given LUA stream file.
        /// </summary>
        /// <param name="writer">Stream writer.</param>
        /// <param name="quest">Current quest.</param>
        private void WriteRewards(StreamWriter writer, QuestData quest)
        {
            WriteAtLevel(writer, 1, $"{QuestScriptConstants.Rewards} = {{", includeComma: false);

            if (quest.MinGold == quest.MaxGold)
            {
                WriteAtLevel(writer, 2, $"{QuestScriptConstants.Gold} = {quest.MaxGold}");
            }
            else
            {
                WriteAtLevel(writer, 2, $"{QuestScriptConstants.Gold} = {{", includeComma: false);
                WriteAtLevel(writer, 3, $"{QuestScriptConstants.MinGold} = {quest.MinGold}");
                WriteAtLevel(writer, 3, $"{QuestScriptConstants.MaxGold} = {quest.MaxGold}");
                WriteAtLevel(writer, 2, $"}}");
            }

            WriteAtLevel(writer, 1, $"}}");
        }

        /// <summary>
        /// Writes the quest dialogs to the given LUA stream file.
        /// </summary>
        /// <param name="writer">Stream writer.</param>
        /// <param name="quest">Current quest.</param>
        private void WriteDialogs(StreamWriter writer, QuestData quest)
        {
            WriteAtLevel(writer, 1, $"{QuestScriptConstants.Dialogs} = {{", includeComma: false);
            WriteDialogSection(writer, 2, QuestScriptConstants.BeginDialogs, quest.BeginDialogs);
            WriteDialogSection(writer, 2, QuestScriptConstants.BeginYesDialogs, quest.AcceptDialogs);
            WriteDialogSection(writer, 2, QuestScriptConstants.BeginNoDialogs, quest.DeclinedDialogs);
            WriteDialogSection(writer, 2, QuestScriptConstants.CompletedDialogs, quest.CompletedDialogs);
            WriteDialogSection(writer, 2, QuestScriptConstants.NotFinishedDialogs, quest.FailureDialogs);
            WriteAtLevel(writer, 1, $"}}", includeComma: false);
        }

        /// <summary>
        /// Writes a dialog section to the given LUA stream file.
        /// </summary>
        /// <param name="writer">Stream writer.</param>
        /// <param name="level">Tabulation level.</param>
        /// <param name="sectionName">Dialog section name.</param>
        /// <param name="dialogs">Dialogs string enumerable/array.</param>
        private void WriteDialogSection(StreamWriter writer, int level, string sectionName, IEnumerable<string> dialogs)
        {
            if (dialogs == null || (dialogs != null && !dialogs.Any()))
            {
                WriteAtLevel(writer, level, $"{sectionName} = nil");
            }
            else
            {
                WriteAtLevel(writer, level, $"{sectionName} = {{", includeComma: false);

                foreach (string dialog in dialogs)
                {
                    WriteAtLevel(writer, level + 1, $"'{dialog}'");
                }

                WriteAtLevel(writer, 2, $"}}");
            }
        }

        /// <summary>
        /// Official quest dialog keys.
        /// </summary>
        private enum QuestDialogKeys
        {
            QSAY_BEGIN1 = 0,
            QSAY_BEGIN2 = 1,
            QSAY_BEGIN3 = 2,
            QSAY_BEGIN4 = 3,
            QSAY_BEGIN5 = 4,
            QSAY_BEGIN_YES = 5,
            QSAY_BEGIN_NO = 6,
            QSAY_END_COMPLETE1 = 7,
            QSAY_END_COMPLETE2 = 8,
            QSAY_END_COMPLETE3 = 9,
            QSAY_END_FAILURE1 = 10,
            QSAY_END_FAILURE2 = 11,
            QSAY_END_FAILURE3 = 12,
            QSAY_EXTRA01 = 15,
            QSAY_EXTRA02 = 16,
            QSAY_EXTRA03 = 17,
            QSAY_EXTRA04 = 18,
            QSAY_EXTRA05 = 19,
            QSAY_EXTRA06 = 20,
            QSAY_EXTRA07 = 21,
            QSAY_EXTRA08 = 22,
            QSAY_EXTRA09 = 23,
            QSAY_EXTRA10 = 24,
            QSAY_EXTRA11 = 25,
            QSAY_EXTRA12 = 26,
            QSAY_EXTRA13 = 27,
            QSAY_EXTRA14 = 28,
            QSAY_EXTRA15 = 29
        }
    }
}
