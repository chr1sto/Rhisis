using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NLua;
using Rhisis.Core.Data;
using Rhisis.Core.Extensions;
using Rhisis.Core.Resources;
using Rhisis.Core.Structures.Game.Quests;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rhisis.Scripting.Quests
{
    public class QuestLoader : IGameResourceLoader
    {
        private static readonly string QuestDefinitionPath = Path.Combine(GameResourcesConstants.Paths.QuestsPath, QuestScriptConstants.QuestDefinitionFile);

        private readonly ILogger<QuestLoader> _logger;
        private readonly IMemoryCache _cache;
        private readonly IDictionary<string, int> _defines;

        /// <summary>
        /// Creates a new <see cref="QuestLoader"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="cache">Memory cache.</param>
        public QuestLoader(ILogger<QuestLoader> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
            _defines = _cache.Get<IDictionary<string, int>>(GameResourcesConstants.Defines);
        }

        /// <inheritdoc />
        public void Load()
        {
            _logger.LogLoading("Loading quests...");

            using var lua = new Lua();

            lua["Jobs"] = Enum.GetValues(typeof(DefineJob.Job))
                                           .Cast<DefineJob.Job>()
                                           .ToDictionary(x => x.ToString(), x => (int)x);

            var quests = new ConcurrentDictionary<int, IQuestScript>();
            IEnumerable<string> questsDefinition = LoadQuestsDefinitions();
            IEnumerable<string> questFiles = Directory.GetFiles(GameResourcesConstants.Paths.QuestsPath, "*.*", SearchOption.AllDirectories).Where(x => x != QuestDefinitionPath);

            foreach (string questFile in questFiles)
            {
                lua.DoFile(questFile);
            }

            foreach (var questName in questsDefinition)
            {
                var questTable = lua[questName] as LuaTable;

                if (!_defines.TryGetValue(questName, out int questId))
                {
                    if (questName.StartsWith(QuestScriptConstants.QuestPrefix) && !int.TryParse(questName.Replace(QuestScriptConstants.QuestPrefix, ""), out questId))
                    {
                        _logger.LogWarning($"Cannot find quest id for quest: '{questName}'.");
                        continue;
                    }
                }

                quests.TryAdd(questId, new QuestScript(questId, questName, questTable));
            }

            _logger.LogInformation($"-> {quests.Count} quests loaded.");
            _cache.Set(GameResourcesConstants.Quests, quests);
        }

        /// <summary>
        /// Load quests definition file.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> LoadQuestsDefinitions()
        {
            using var lua = new Lua();

            lua.DoFile(QuestDefinitionPath);

            var quests = lua[QuestScriptConstants.QuestDefinitionKey] as LuaTable;

            return quests.Values.ToArray<string>();
        }
    }
}
