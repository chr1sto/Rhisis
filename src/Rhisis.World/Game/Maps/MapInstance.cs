using System;
using Rhisis.Core.Common;
using Rhisis.Core.Helpers;
using Rhisis.Core.Resources;
using Rhisis.Core.Resources.Dyo;
using Rhisis.Core.Structures.Game;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Regions;
using Rhisis.World.Game.Structures;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rhisis.World.Game.Maps
{
    /// <inheritdoc />
    public class MapInstance : Context, IMapInstance
    {
        private readonly string _mapPath;
        private readonly List<IMapLayer> _layers;
        private readonly List<IRegion> _regions;

        /// <inheritdoc />
        public int Id { get; }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public IReadOnlyList<IMapLayer> Layers => this._layers;

        /// <inheritdoc />
        public IReadOnlyList<IRegion> Regions => this._regions;

        /// <summary>
        /// Creates a new <see cref="MapInstance"/>.
        /// </summary>
        /// <param name="id">Map Id</param>
        /// <param name="name">Map Name</param>
        /// <param name="mapPath">Map path</param>
        private MapInstance(int id, string name, string mapPath)
        {
            this.Id = id;
            this.Name = name;
            this._mapPath = mapPath;
            this._layers = new List<IMapLayer>();
            this._regions = new List<IRegion>();
        }

        /// <inheritdoc />
        public void LoadDyo()
        {
            string dyo = Path.Combine(this._mapPath, this.Name + ".dyo");

            using (var dyoFile = new DyoFile(dyo))
            {
                IEnumerable<NpcDyoElement> npcElements = dyoFile.GetElements<NpcDyoElement>();

                foreach (NpcDyoElement element in npcElements)
                    this.CreateNpc(element);
            }
        }

        /// <inheritdoc />
        public void LoadRgn()
        {
            string rgn = Path.Combine(this._mapPath, this.Name + ".rgn");

            using (var rgnFile = new RgnFile(rgn))
            {
                IEnumerable<IRegion> monsterRegions = rgnFile.Elements
                        .Where(x => x is RgnRespawn7)
                        .Cast<RgnRespawn7>()
                        .Select(x => new RespawnerRegion(x.Left, x.Top, x.Right, x.Bottom, x.Time, x.Type, x.Model, x.Count));

                this._regions.AddRange(monsterRegions);
            }
        }

        /// <inheritdoc />
        public IMapLayer CreateMapLayer()
        {
            var mapLayer = new MapLayer(this, 1);

            this._layers.Add(mapLayer);

            return mapLayer;
        }

        /// <inheritdoc />
        public IMapLayer CreateMapLayer(int id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IMapLayerInstance CreaMapLayerInstance(int id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IMapLayer GetMapLayer(int id) => this._layers.FirstOrDefault(x => x.Id == id);

        /// <inheritdoc />
        public void DeleteMapLayer(int id)
        {
            IMapLayer layer = this.GetMapLayer(id);

            if (layer == null)
                return;

            layer.Dispose();
            this._layers.Remove(layer);
        }

        /// <summary>
        /// Creates a NPC.
        /// </summary>
        /// <param name="element"></param>
        private void CreateNpc(NpcDyoElement element)
        {
            var npc = this.CreateEntity<NpcEntity>();

            npc.Object = new ObjectComponent
            {
                MapId = this.Id,
                ModelId = element.Index,
                Name = element.Name,
                Angle = element.Angle,
                Type = WorldObjectType.Mover,
                Position = element.Position.Clone(),
                Size = (short)(ObjectComponent.DefaultObjectSize * element.Scale.X),
                Spawned = true,
                Level = 1
            };

            if (WorldServer.Npcs.TryGetValue(npc.Object.Name, out NpcData npcData))
            {
                npc.Data = npcData;

                if (npcData.HasShop)
                {
                    npc.Shop = new ItemContainerComponent[npcData.Shop.Items.Length];

                    for (var i = 0; i < npcData.Shop.Items.Length; i++)
                    {
                        npc.Shop[i] = new ItemContainerComponent(100);

                        for (var j = 0; j < npcData.Shop.Items[i].Count && j < npc.Shop[i].MaxCapacity; j++)
                        {
                            ItemBase item = npcData.Shop.Items[i][j];

                            npc.Shop[i].Items[j] = new Item(item.Id, WorldServer.Items[item.Id].PackMax, -1, j, j, item.Refine, item.Element, item.ElementRefine);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Creates and loads a new map.
        /// </summary>
        /// <param name="mapPath">Map path</param>
        /// <param name="mapName">Map name</param>
        /// <param name="mapId">Map id</param>
        /// <returns></returns>
        public static IMapInstance Create(string mapPath, string mapName, int mapId)
        {
            IMapInstance map = new MapInstance(mapId, mapName, mapPath);

            // TODO: Load map heights, revival zones
            map.LoadDyo();
            map.LoadRgn();
            map.CreateMapLayer();

            return map;
        }
    }
}
