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
        private const short DefaultMoverSize = 100;
        private readonly string _mapPath;
        private readonly IList<IRegion> _regions;

        /// <inheritdoc />
        public int Id { get; }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public IList<IMapLayer> Layers { get; }

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
            this._regions = new List<IRegion>();
            this.Layers = new List<IMapLayer>();
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
                IEnumerable<RgnRespawn7> monsterRegions = rgnFile.Elements.Where(r => r is RgnRespawn7).Cast<RgnRespawn7>();

                foreach (RgnRespawn7 rgnElement in monsterRegions)
                {
                    var respawner = new RespawnerRegion(rgnElement.Left, rgnElement.Top, rgnElement.Right, rgnElement.Bottom, rgnElement.Time);

                    if (rgnElement.Type == (int)WorldObjectType.Mover)
                    {
                        for (var i = 0; i < rgnElement.Count; ++i)
                            this.CreateMonster(rgnElement, respawner);
                    }

                    this._regions.Add(respawner);
                }
            }
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
                Size = (short)(DefaultMoverSize * element.Scale.X),
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
        /// Creates a monster.
        /// </summary>
        /// <param name="rgnElement"></param>
        /// <param name="respawner"></param>
        private void CreateMonster(RgnRespawn7 rgnElement, Region respawner)
        {
            var monster = this.CreateEntity<MonsterEntity>();

            monster.Object = new ObjectComponent
            {
                MapId = this.Id,
                ModelId = rgnElement.Model,
                Type = WorldObjectType.Mover,
                Position = respawner.GetRandomPosition(),
                Angle = RandomHelper.FloatRandom(0, 360f),
                Name = WorldServer.Movers[rgnElement.Model].Name,
                Size = DefaultMoverSize,
                Spawned = true,
                Level = WorldServer.Movers[rgnElement.Model].Level
            };
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

            map.LoadDyo();
            map.LoadRgn();

            return map;
        }
    }
}
