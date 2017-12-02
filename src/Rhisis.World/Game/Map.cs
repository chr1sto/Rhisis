using Rhisis.Core.Common;
using Rhisis.Core.Helpers;
using Rhisis.Core.Resources;
using Rhisis.Core.Resources.Dyo;
using Rhisis.World.Core;
using Rhisis.World.Core.Components;
using Rhisis.World.Game.Regions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rhisis.World.Game
{
    /// <summary>
    /// Defines a map with it's own entities and context.
    /// </summary>
    public sealed class Map : IDisposable
    {
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
            this.Context.StartSystemUpdate(60);
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

            using (var dyoFile = new DyoFile(dyo))
            {
            }

            using (var rgnFile = new RgnFile(rgn))
            {
                foreach (RgnRespawn7 rgnElement in rgnFile.Elements.Where(r => r is RgnRespawn7))
                {
                    var respawner = new RespawnerRegion(rgnElement.Left, rgnElement.Top, rgnElement.Right, rgnElement.Bottom, rgnElement.Time);

                    if (rgnElement.Type == (int)WorldObjectType.Mover)
                    {
                        for (int i = 0; i < rgnElement.Count; ++i)
                        {
                            var monster = map.Context.CreateEntity(WorldEntityType.Monster);

                            var objectComponent = new ObjectComponent
                            {
                                MapId = mapId,
                                ModelId = rgnElement.Model,
                                Type = WorldObjectType.Mover,
                                Position = respawner.GetRandomPosition(),
                                Angle = RandomHelper.FloatRandom(0, 360f),
                                Name = WorldServer.Movers[rgnElement.Model].Name,
                                Size = 100,
                                Spawned = true,
                            };

                            monster.AddComponent(objectComponent);
                        }
                    }

                    map._regions.Add(respawner);
                }
            }

            // TODO: load map informations
            // TODO: load objects
            // TODO: load heights
            // TODO: load revival zones

            return map;
        }
    }
}
