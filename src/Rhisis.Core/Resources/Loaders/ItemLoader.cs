using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Rhisis.Core.Data;
using Rhisis.Core.Extensions;
using Rhisis.Core.Structures.Game;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace Rhisis.Core.Resources.Loaders
{
    public sealed class ItemLoader : IGameResourceLoader
    {
        private readonly ILogger<ItemLoader> _logger;
        private readonly IMemoryCache _cache;
        private readonly IDictionary<string, int> _defines;
        private readonly IDictionary<string, string> _texts;

        /// <summary>
        /// Creates a new <see cref="ItemLoader"/> instance.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="defines">Defines</param>
        /// <param name="texts">Texts</param>
        public ItemLoader(ILogger<ItemLoader> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
            _defines = _cache.Get<IDictionary<string, int>>(GameResourcesConstants.Defines);
            _texts = _cache.Get<IDictionary<string, string>>(GameResourcesConstants.Texts);
        }

        /// <inheritdoc />
        public void Load()
        {
            string propItemPath = GameResourcesConstants.Paths.ItemsPropPath;

            if (!File.Exists(propItemPath))
            {
                _logger.LogWarning("Unable to load items. Reason: cannot find '{0}' file.", propItemPath);
                return;
            }

            var itemsData = new ConcurrentDictionary<int, ItemData>();

            using (var propItem = new ResourceTableFile(propItemPath, 1, _defines, _texts))
            {
                var items = propItem.GetRecords<ItemData>();

                foreach (ItemData item in items)
                {
                    TransformItem(item);

                    if (itemsData.ContainsKey(item.Id))
                    {
                        itemsData[item.Id] = item;
                        _logger.LogWarning(GameResourcesConstants.Errors.ObjectOverridedMessage, "Item", item.Id, "already declared");
                    }
                    else
                        itemsData.TryAdd(item.Id, item);
                }
            }

            _cache.Set(GameResourcesConstants.Items, itemsData);
            _logger.LogInformation("-> {0} items loaded.", itemsData.Count);
        }

        /// <summary>
        /// Applies custom transformation to an item.
        /// </summary>
        /// <param name="item">Item to transform.</param>
        private void TransformItem(ItemData item)
        {
            var itemParams = new Dictionary<DefineAttributes, int>();

            if (item.DestParam1 != null && item.DestParam1 != "0")
            {
                itemParams.Add(item.DestParam1.Replace("DST_", string.Empty).ToEnum<DefineAttributes>(), item.AdjustParam1);
            }
            if (item.DestParam2 != null && item.DestParam2 != "0")
            {
                itemParams.Add(item.DestParam2.Replace("DST_", string.Empty).ToEnum<DefineAttributes>(), item.AdjustParam2);
            }
            if (item.DestParam3 != null && item.DestParam3 != "0")
            {
                itemParams.Add(item.DestParam3.Replace("DST_", string.Empty).ToEnum<DefineAttributes>(), item.AdjustParam3);
            }

            item.Params = itemParams;
        }
    }
}
