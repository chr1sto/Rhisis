using Rhisis.Core.Data;
using Rhisis.Core.Structures.Game;
using Rhisis.Database.Entities;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Factories;
using Rhisis.World.Game.Maps;
using Rhisis.World.Game.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.World.Tests.Mocks.Factories
{
    public class ItemFactoryMock : IItemFactory
    {
        public Item CreateItem(int id, byte refine, ElementType element, byte elementRefine, int creatorId = -1)
        {
            var itemData = new ItemData
            {
                Id = id,
                Name = $"Item n°{id}"
            };

            return new Item(id, refine, element, elementRefine, itemData, creatorId);
        }

        public Item CreateItem(string name, byte refine, ElementType element, byte elementRefine, int creatorId = -1)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Item CreateItem(DbItem databaseItem)
        {
            var itemData = new ItemData
            {
                Id = databaseItem.ItemId,
                Name = $"Item n°{databaseItem.ItemId}"
            };

            return new Item(databaseItem, itemData);
        }

        public IItemEntity CreateItemEntity(IMapInstance currentMapContext, IMapLayer currentMapLayerContext, ItemDescriptor item, IWorldEntity owner = null)
        {
            throw new NotImplementedException();
        }
    }
}
