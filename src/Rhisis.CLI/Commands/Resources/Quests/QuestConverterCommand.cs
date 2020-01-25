using McMaster.Extensions.CommandLineUtils;
using Rhisis.Core.Data;
using Rhisis.Core.Structures.Game.Quests;
using Rhisis.Scripting.Quests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
            if (InputFiles == null)
            {
                return;
            }

            Output = string.IsNullOrEmpty(Output) ? Directory.GetCurrentDirectory() : Path.GetDirectoryName(Output);
            
            IEnumerable<string> questFiles = GetFilesFromInput();
            var questLoader = new LegacyQuestLoader();
            var questsSaved = new List<string>();

            foreach (string inputFile in questFiles)
            {
                questLoader.Load(inputFile);
            }

            foreach (QuestData quest in questLoader.Quests)
            {
                SaveQuestDataAsLua(quest);
                questsSaved.Add(quest.Name);
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
        /// Saves the quest has a LUA script.
        /// </summary>
        /// <param name="quest">Quest to save.</param>
        private void SaveQuestDataAsLua(QuestData quest)
        {
            string filePath = Path.Combine(Output, $"{quest.Name}.lua");

            Console.Write($"Saving quest: '{quest.Name}' into '{filePath}'...\r");

            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            using var writer = new StreamWriter(fileStream);

            WriteAtLevel(writer, 0, $"{quest.Name} = {{", includeComma: false);
            WriteMainInformations(writer, quest);
            WriteStartRequirements(writer, quest);
            WriteRewards(writer, quest);
            WriteEndConditions(writer, quest);
            WriteDialogs(writer, quest);
            WriteAtLevel(writer, 0, "}", includeComma: false);

            Console.WriteLine($"Done: '{quest.Name}' saved into '{filePath}'!");
        }

        /// <summary>
        /// Save the quest definition.
        /// </summary>
        /// <param name="quests">Quests to save.</param>
        private void SaveQuestDefinition(IEnumerable<string> quests)
        {
            string filePath = Path.Combine(Output, QuestScriptConstants.QuestDefinitionFile);
            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            using var writer = new StreamWriter(fileStream);

            if (quests == null || !quests.Any())
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
            WriteAtLevel(writer, 1, $"{QuestScriptConstants.EndCharacter} = '{quest.EndCharacter}'");
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
            WriteAtLevel(writer, 2, $"{QuestScriptConstants.PreviousQuest} = '{quest.PreviousQuestId}'");
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
                WriteAtLevel(writer, 3, $"{QuestScriptConstants.Min} = {quest.MinGold}");
                WriteAtLevel(writer, 3, $"{QuestScriptConstants.Max} = {quest.MaxGold}");
                WriteAtLevel(writer, 2, $"}}");
            }

            if (quest.MinExp == quest.MaxExp)
            {
                WriteAtLevel(writer, 2, $"{QuestScriptConstants.Experience} = {quest.MaxExp}");
            }
            else
            {
                WriteAtLevel(writer, 2, $"{QuestScriptConstants.Experience} = {{", includeComma: false);
                WriteAtLevel(writer, 3, $"{QuestScriptConstants.Min} = {quest.MinExp}");
                WriteAtLevel(writer, 3, $"{QuestScriptConstants.Max} = {quest.MaxExp}");
                WriteAtLevel(writer, 2, $"}}");
            }

            if (quest.RewardItems != null && quest.RewardItems.Any())
            {
                WriteAtLevel(writer, 2, $"{QuestScriptConstants.Items} = {{", includeComma: false);
                foreach (QuestItem item in quest.RewardItems)
                {
                    var builder = new StringBuilder();

                    builder.Append($"{{ id = '{item.Id}', quantity = { item.Quantity}, sex = '{item.Sex}'");

                    if (item.Refine > 0)
                    {
                        builder.Append($", refine = {item.Refine}");
                    }

                    if (item.Element != ElementType.None && item.ElementRefine > 0)
                    {
                        builder.Append($", element = '{item.Element.ToString()}', element_refine = {item.ElementRefine}");
                    }

                    builder.Append($" }}");

                    WriteAtLevel(writer, 3, builder.ToString());
                }
                WriteAtLevel(writer, 2, $"}}");
            }

            if (quest.RewardJob.HasValue)
            {
                WriteAtLevel(writer, 2, $"job = '{quest.RewardJob.Value.ToString()}'");
            }

            WriteAtLevel(writer, 1, $"}}");
        }

        /// <summary>
        /// Writes the end conditions of the quest.
        /// </summary>
        /// <param name="writer">Stream writer.</param>
        /// <param name="quest">Current quest.</param>
        private void WriteEndConditions(StreamWriter writer, QuestData quest)
        {
            WriteAtLevel(writer, 1, $"{QuestScriptConstants.EndConditions} = {{", includeComma: false);

            if (quest.EndQuestItems != null && quest.EndQuestItems.Any())
            {
                WriteAtLevel(writer, 2, $"{QuestScriptConstants.Items} = {{", includeComma: false);
                foreach (QuestItem item in quest.EndQuestItems)
                {
                    WriteAtLevel(writer, 3, $"{{ id = '{item.Id}', quantity = {item.Quantity}, sex = '{item.Sex}', remove = {item.Remove.ToString().ToLower()} }}");
                }
                WriteAtLevel(writer, 2, $"}}");
            }

            if (quest.EndQuestMonsters != null && quest.EndQuestMonsters.Any())
            {
                WriteAtLevel(writer, 2, $"{QuestScriptConstants.Monsters} = {{", includeComma: false);
                foreach (QuestMonster monster in quest.EndQuestMonsters)
                {
                    WriteAtLevel(writer, 3, $"{{ id = '{monster.Id}', quantity = {monster.Amount} }}");
                }
                WriteAtLevel(writer, 2, $"}}");
            }

            if (quest.EndQuestPatrols != null && quest.EndQuestPatrols.Any())
            {
                WriteAtLevel(writer, 2, $"{QuestScriptConstants.Patrols} = {{", includeComma: false);
                foreach (QuestPatrol patrol in quest.EndQuestPatrols)
                {
                    WriteAtLevel(writer, 3, $"{{ map = '{patrol.MapId}', left = {patrol.Left}, top = {patrol.Top}, right = {patrol.Right}, bottom = {patrol.Bottom} }}");
                }
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
            if (dialogs == null || !dialogs.Any())
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
    }
}
