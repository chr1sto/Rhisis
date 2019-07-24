using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rhisis.Core.IO;
using Rhisis.Core.Structures.Game;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Rhisis.Core.Resources
{
    internal class NewGameResources : IGameResources
    {
        private readonly ILogger<NewGameResources> _logger;
        private readonly IMemoryCache _cache;
        private readonly IServiceProvider _serciceProvider;
        private ConcurrentDictionary<int, MoverData> _movers;
        private ConcurrentDictionary<int, ItemData> _items;
        private ConcurrentDictionary<int, JobData> _jobs;

        /// <inheritdoc />
        public IReadOnlyDictionary<int, MoverData> Movers
        {
            get
            {
                if (this._movers == null)
                {
                    this._cache.TryGetValue(GameResourcesConstants.Movers, out this._movers);
                }

                return this._movers;
            }
        }

        /// <inheritdoc />
        public IReadOnlyDictionary<int, ItemData> Items
        {
            get
            {
                if (this._items == null)
                {
                    this._cache.TryGetValue(GameResourcesConstants.Items, out this._items);
                }

                return this._items;
            }
        }

        /// <inheritdoc />
        public IReadOnlyDictionary<int, JobData> Jobs
        {
            get
            {
                if (this._jobs == null)
                {
                    this._cache.TryGetValue(GameResourcesConstants.Jobs, out this._jobs);
                }

                return this._jobs;
            }
        }

        public NewGameResources(ILogger<NewGameResources> logger, IMemoryCache cache, IServiceProvider serviceProvider)
        {
            this._logger = logger;
            this._cache = cache;
            this._serciceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public void Load(params Type[] loaders)
        {
            Profiler.Start("LoadResources");
            this._logger.LogInformation("Loading server resources...");

            foreach (Type loaderType in loaders)
            {
                var loader = (IGameResourceLoader)ActivatorUtilities.CreateInstance(this._serciceProvider, loaderType);

                if (loader != null)
                {
                    loader.Load();
                }
            }

            this._logger.LogInformation("Resources loaded in {0} ms.", Profiler.Stop("LoadResources").ElapsedMilliseconds);
        }
    }
}
