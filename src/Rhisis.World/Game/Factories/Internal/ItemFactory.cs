using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rhisis.Core.Common;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Resources;
using Rhisis.Core.Structures.Game;
using Rhisis.Database.Entities;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Entities.Internal;
using Rhisis.World.Game.Maps;
using Rhisis.World.Game.Structures;
using System;
using System.Linq;

namespace Rhisis.World.Game.Factories.Internal
{
    [Injectable(ServiceLifetime.Singleton)]
    public sealed class ItemFactory : IItemFactory
    {
        private readonly ILogger<ItemFactory> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IGameResources _gameResources;
        private readonly ObjectFactory _itemFactory;
        private readonly ObjectFactory _itemDatabaseFactory;
        private readonly ObjectFactory _itemEntityFactory;

        /// <summary>
        /// Creates a new <see cref="ItemFactory"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="serviceProvider">Service provider.</param>
        /// <param name="gameResources">Game resources.</param>
        public ItemFactory(ILogger<ItemFactory> logger, IServiceProvider serviceProvider, IGameResources gameResources)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _gameResources = gameResources;
            _itemFactory = ActivatorUtilities.CreateFactory(typeof(Item), new[] { typeof(int), typeof(byte), typeof(ElementType), typeof(byte), typeof(ItemData), typeof(int) });
            _itemDatabaseFactory = ActivatorUtilities.CreateFactory(typeof(Item), new[] { typeof(DbItem), typeof(ItemData) });
            _itemEntityFactory = ActivatorUtilities.CreateFactory(typeof(ItemEntity), Type.EmptyTypes);
        }

        /// <inheritdoc />
        public Item CreateItem(int id, byte refine, ElementType element, byte elementRefine, int creatorId = -1)
        {            
            if (!_gameResources.Items.TryGetValue(id, out ItemData itemData))
            {
                _logger.LogWarning($"Cannot find item data for item id: '{id}'.");
                return null;
            }

            return _itemFactory(_serviceProvider, new object[] { id, refine, element, elementRefine, itemData, creatorId }) as Item;
        }

        public Item CreateItem(string name, byte refine, ElementType element, byte elementRefine, int creatorId = -1)
        {
            var itemData = _gameResources.Items.FirstOrDefault(x => x.Value.Name == name);
            if (itemData.Value is null)
            {
                _logger.LogWarning($"Cannot find item data for item name: '{name}'.");
                return null;
            }
            return _itemFactory(_serviceProvider, new object[] { itemData.Value.Id, refine, element, elementRefine, itemData.Value, creatorId }) as Item;
        }

        /// <inheritdoc />
        public Item CreateItem(DbItem databaseItem)
        {
            if (!_gameResources.Items.TryGetValue(databaseItem.ItemId, out ItemData itemData))
            {
                _logger.LogWarning($"Cannot find item data for item id: '{databaseItem.ItemId}'.");
                return null;
            }

            return _itemDatabaseFactory(_serviceProvider, new object[] { databaseItem, itemData }) as Item;
        }

        /// <inheritdoc />
        public IItemEntity CreateItemEntity(IMapInstance currentMapContext, IMapLayer currentMapLayerContext, ItemDescriptor item, IWorldEntity owner = null)
        {
            var itemEntity = _itemEntityFactory(_serviceProvider, null) as IItemEntity;

            itemEntity.Object = new ObjectComponent
            {
                CurrentMap = currentMapContext,
                MapId = currentMapContext.Id,
                LayerId = currentMapLayerContext.Id,
                ModelId = item.Id,
                Name = item.Data.Name,
                Spawned = true,
                Type = WorldObjectType.Item
            };

            itemEntity.Drop = new DropComponent
            {
                Owner = owner,
                Item = CreateItem(item.Id, item.Refine, item.Element, item.ElementRefine)
            };

            return itemEntity;
        }
    }
}
