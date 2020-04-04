using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rhisis.Core.Data;
using Rhisis.Core.IO;
using Rhisis.Core.Structures.Game;
using Rhisis.Core.Structures.Game.Dialogs;
using Rhisis.Core.Structures.Game.Quests;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Rhisis.Core.Resources
{
    internal class GameResources : IGameResources
    {
        private readonly ILogger<GameResources> _logger;
        private readonly IMemoryCache _cache;
        private readonly IServiceProvider _serciceProvider;
        private ConcurrentDictionary<string, int> _defines;
        private ConcurrentDictionary<string, string> _texts;
        private ConcurrentDictionary<int, MoverData> _movers;
        private ConcurrentDictionary<int, ItemData> _items;
        private ConcurrentDictionary<string, DialogSet> _dialogs;
        private ConcurrentDictionary<string, ShopData> _shops;
        private ConcurrentDictionary<DefineJob.Job, JobData> _jobs;
        private ConcurrentDictionary<string, NpcData> _npcs;
        private ConcurrentDictionary<int, IQuestScript> _quests;
        private ConcurrentDictionary<int, SkillData> _skills;
        private ExpTableData _expTableData;
        private DeathPenalityData _penalities;

        /// <inheritdoc />
        public IReadOnlyDictionary<string, int> Defines => GetCacheValue(GameResourcesConstants.Defines, ref _defines);

        /// <inheritdoc />
        public IReadOnlyDictionary<string, string> Texts => GetCacheValue(GameResourcesConstants.Texts, ref _texts);

        /// <inheritdoc />
        public IReadOnlyDictionary<int, MoverData> Movers => GetCacheValue(GameResourcesConstants.Movers, ref _movers);

        /// <inheritdoc />
        public IReadOnlyDictionary<int, ItemData> Items => GetCacheValue(GameResourcesConstants.Items, ref _items);

        /// <inheritdoc />
        public IReadOnlyDictionary<string, DialogSet> Dialogs => GetCacheValue(GameResourcesConstants.Dialogs, ref _dialogs);

        /// <inheritdoc />
        public IReadOnlyDictionary<string, ShopData> Shops => GetCacheValue(GameResourcesConstants.Shops, ref _shops);

        /// <inheritdoc />
        public IReadOnlyDictionary<DefineJob.Job, JobData> Jobs => GetCacheValue(GameResourcesConstants.Jobs, ref _jobs);

        /// <inheritdoc />
        public IReadOnlyDictionary<string, NpcData> Npcs => GetCacheValue(GameResourcesConstants.Npcs, ref _npcs);

        /// <inheritdoc />
        public IReadOnlyDictionary<int, IQuestScript> Quests => GetCacheValue(GameResourcesConstants.Quests, ref _quests);

        /// <inheritdoc />
        public IReadOnlyDictionary<int, SkillData> Skills => GetCacheValue(GameResourcesConstants.Skills, ref _skills);

        /// <inheritdoc />
        public ExpTableData ExpTables => GetCacheValue(GameResourcesConstants.ExpTables, ref _expTableData);

        /// <inheritdoc />
        public DeathPenalityData Penalities => GetCacheValue(GameResourcesConstants.PenalityData, ref _penalities);

        /// <summary>
        /// Creates a new <see cref="GameResources"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="cache">Memory cache.</param>
        /// <param name="serviceProvider">Service provider.</param>
        public GameResources(ILogger<GameResources> logger, IMemoryCache cache, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _cache = cache;
            _serciceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public void Load(params Type[] loaders)
        {
            Profiler.Start("LoadResources");
            _logger.LogInformation("Loading server resources...");

            foreach (Type loaderType in loaders)
            {
                var loader = (IGameResourceLoader)ActivatorUtilities.CreateInstance(_serciceProvider, loaderType);

                if (loader != null)
                {
                    loader.Load();
                }
            }

            _logger.LogInformation("Resources loaded in {0} ms.", Profiler.Stop("LoadResources").ElapsedMilliseconds);
        }

        /// <summary>
        /// Gets a cached value by it's key.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="key">Cached object key.</param>
        /// <param name="value">Object reference to store the cached value.</param>
        /// <returns></returns>
        private T GetCacheValue<T>(object key, ref T value)
        {
            if (Equals(value, default(T)))
            {
                _cache.TryGetValue(key, out value);
            }

            return value;
        }

        /// <inheritdoc />
        public string GetText(string textKey, string defaultText = null)
        {
            if (Texts.TryGetValue(textKey, out string text))
            {
                return text;
            }

            return string.IsNullOrEmpty(defaultText) ? textKey : defaultText;
        }

        /// <inheritdoc />
        public int GetDefinedValue(string defineKey, int defaultValue = 0)
        {
            if (string.IsNullOrEmpty(defineKey))
            {
                return defaultValue;
            }

            return Defines.TryGetValue(defineKey, out int value) ? value : defaultValue;
        }
    }
}
