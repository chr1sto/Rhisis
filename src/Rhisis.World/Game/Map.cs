using Rhisis.Core.Common;
using Rhisis.Core.Helpers;
using Rhisis.Core.Resources;
using Rhisis.Core.Resources.Dyo;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Interfaces;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Regions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Rhisis.Core.Structures.Game;
using Rhisis.World.Game.Structures;

namespace Rhisis.World.Game
{
    /// <summary>
    /// Defines a map with it's own entities and context.
    /// </summary>
    public sealed class Map : IDisposable
    {
        private static readonly short DefaultMoverSize = 100;

        private bool _isDisposed;
        
        private readonly ICollection<IRegion> _regions;

        /// <summary>
        /// Gets the map id.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the map name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the map context.
        /// </summary>
        public IContext Context { get; }

        /// <summary>
        /// Creates and initializes a new <see cref="Map"/> instance.
        /// </summary>
        private Map(string name, int id)
        {
            this.Id = id;
            this.Name = name;
            this.Context = new Context();
            this._regions = new List<IRegion>();
        }

        /// <summary>
        /// Start the map update task.
        /// </summary>
        public void Start()
        {
            this.Context.StartSystemUpdate(100);
        }

        /// <summary>
        /// Dispose the map resources.
        /// </summary>
        public void Dispose()
        {
            if (!this._isDisposed)
            {
                this.Context.Dispose();
                this._isDisposed = true;
            }
        }

        /// <summary>
        /// Loads a new map.
        /// </summary>
        /// <param name="mapPath">Map path</param>
        /// <returns>New map</returns>
        public static Map Load(string mapPath, string mapName, int mapId)
        {
            string wld = Path.Combine(mapPath, mapName + ".wld");
            string dyo = Path.Combine(mapPath, mapName + ".dyo");
            string rgn = Path.Combine(mapPath, mapName + ".rgn");
            var map = new Map(mapName, mapId);

            // Load NPC
            using (var dyoFile = new DyoFile(dyo))
            {
                IEnumerable<NpcDyoElement> npcElements = dyoFile.GetElements<NpcDyoElement>();

                foreach (NpcDyoElement element in npcElements)
                    CreateNpc(map, element);
            }

            // Load monsters
            using (var rgnFile = new RgnFile(rgn))
            {
                foreach (RgnRespawn7 rgnElement in rgnFile.Elements.Where(r => r is RgnRespawn7))
                {
                    var respawner = new RespawnerRegion(rgnElement.Left, rgnElement.Top, rgnElement.Right, rgnElement.Bottom, rgnElement.Time);

                    if (rgnElement.Type == (int)WorldObjectType.Mover)
                    {
                        for (int i = 0; i < rgnElement.Count; ++i)
                        {
                            var monster = map.Context.CreateEntity<MonsterEntity>();

                            monster.Object = new ObjectComponent
                            {
                                MapId = mapId,
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
                    }

                    map._regions.Add(respawner);
                }
            }

            // TODO: load map informations
            // TODO: load heights
            // TODO: load revival zones

            return map;
        }

        /// <summary>
        /// Creates and initializes a new NPC.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="element"></param>
        private static void CreateNpc(Map map, NpcDyoElement element)
        {
            var npc = map.Context.CreateEntity<NpcEntity>();

            npc.Object = new ObjectComponent
            {
                MapId = map.Id,
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

                    for (int i = 0; i < npcData.Shop.Items.Length; i++)
                    {
                        npc.Shop[i] = new ItemContainerComponent(100);

                        for (int j = 0; j < npcData.Shop.Items[i].Count && j < npc.Shop[i].MaxCapacity; j++)
                        {
                            ItemBase item = npcData.Shop.Items[i][j];

                            npc.Shop[i].Items[j] = new Item(item.Id, WorldServer.Items[item.Id].PackMax, -1, j, j, item.Refine, item.Element, item.ElementRefine);
                        }
                    }
                }
            }
        }
    }
}
