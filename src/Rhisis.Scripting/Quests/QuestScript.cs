using NLua;
using Rhisis.Core.Extensions;
using Rhisis.Core.Structures.Game.Quests;
using Rhisis.Scripting.Extensions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Rhisis.Scripting.Quests
{
    [DebuggerDisplay("{Name}")]
    internal class QuestScript : ScriptBase, IQuestScript
    {
        /// <inheritdoc />
        public int Id { get; }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public string Title { get; }

        /// <inheritdoc />
        public string StartCharacter { get; }

        public string EndCharacter { get; }

        /// <inheritdoc />
        public IQuestRewards Rewards { get; }

        /// <inheritdoc />
        public IQuestStartRequirements StartRequirements { get; }

        /// <inheritdoc />
        public IQuestEndConditions EndConditions { get; }

        /// <inheritdoc />
        public IEnumerable<QuestItemDrop> Drops { get; }

        /// <inheritdoc />
        public IEnumerable<string> BeginDialogs { get; }

        /// <inheritdoc />
        public IEnumerable<string> AcceptedDialogs { get; }

        /// <inheritdoc />
        public IEnumerable<string> DeclinedDialogs { get; }

        /// <inheritdoc />
        public IEnumerable<string> CompletedDialogs { get; }

        /// <inheritdoc />
        public IEnumerable<string> NotFinishedDialogs { get; }

        /// <summary>
        /// Creates a new <see cref="QuestScript"/> instance.
        /// </summary>
        /// <param name="questId">Quest id.</param>
        /// <param name="questName">Quest name.</param>
        /// <param name="luaScriptTable">Lua script table.</param>
        public QuestScript(int questId, string questName, LuaTable luaScriptTable)
            : base(luaScriptTable)
        {
            Id = questId;
            Name = questName;
            Title = luaScriptTable.GetValue<string>(QuestScriptConstants.Title);
            StartCharacter = luaScriptTable.GetValue<string>(QuestScriptConstants.StartCharacter);
            EndCharacter = luaScriptTable.GetValue<string>(QuestScriptConstants.EndCharacter);

            if (string.IsNullOrEmpty(EndCharacter))
            {
                EndCharacter = StartCharacter;
            }

            Rewards = new QuestRewards(luaScriptTable, ScriptTable[QuestScriptConstants.Rewards] as LuaTable);
            StartRequirements = new QuestStartRequirements(ScriptTable[QuestScriptConstants.StartRequirements] as LuaTable);
            EndConditions = new QuestEndConditions(ScriptTable[QuestScriptConstants.EndConditions] as LuaTable);
            Drops = LoadQuestItemDrops(ScriptTable[QuestScriptConstants.Drops] as LuaTable);

            if (ScriptTable[QuestScriptConstants.Dialogs] is LuaTable dialogsTable)
            {
                BeginDialogs = GetDialogs(dialogsTable, QuestScriptConstants.BeginDialogs);
                AcceptedDialogs = GetDialogs(dialogsTable, QuestScriptConstants.BeginYesDialogs);
                DeclinedDialogs = GetDialogs(dialogsTable, QuestScriptConstants.BeginNoDialogs);
                CompletedDialogs = GetDialogs(dialogsTable, QuestScriptConstants.CompletedDialogs);
                NotFinishedDialogs = GetDialogs(dialogsTable, QuestScriptConstants.NotFinishedDialogs);
            }
        }

        /// <summary>
        /// Gets the dialogs values.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private IEnumerable<string> GetDialogs(LuaTable table, string path) => (table[path] as LuaTable)?.Values.ToArray<string>();

        /// <summary>
        /// Loads the quest item drops.
        /// </summary>
        /// <param name="table">Main lua table.</param>
        /// <returns>Collection of <see cref="QuestItemDrop"/> objects.</returns>
        private IEnumerable<QuestItemDrop> LoadQuestItemDrops(LuaTable table)
        {
            var questItemDrops = new List<QuestItemDrop>();

            if (table != null)
            {
                IEnumerable<LuaTable> values = table.Values.ToArray<LuaTable>().AsEnumerable();

                foreach (LuaTable dropItem in values)
                {
                    IEnumerable<string> monsterIds;

                    if (dropItem[QuestScriptConstants.Monsters] is LuaTable dropItemMonstersTable && dropItemMonstersTable.Values.Count > 0)
                    {
                        monsterIds = dropItemMonstersTable.Values.ToArray<string>().AsEnumerable();
                    }
                    else
                    {
                        monsterIds = new[]
                        {
                            dropItem.GetValue<string>(QuestScriptConstants.MonsterId)
                        };
                    }

                    string itemId = dropItem.GetValue<string>(QuestScriptConstants.ItemId);
                    long probability = dropItem.GetValue<long>(QuestScriptConstants.Probability);
                    int quantity = dropItem.GetValueOrDefault<int>(QuestScriptConstants.Quantity, 1);

                    foreach (string monsterId in monsterIds)
                    {
                        questItemDrops.Add(new QuestItemDrop
                        {
                            ItemId = itemId,
                            MonsterId = monsterId,
                            Probability = probability,
                            Quantity = quantity
                        });
                    }
                }
            }

            return questItemDrops;
        }
    }
}
