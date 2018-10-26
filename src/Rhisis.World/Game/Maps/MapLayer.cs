using NLog;
using Rhisis.Core.Common;
using Rhisis.Core.Helpers;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Maps.Regions;
using System.Collections.Generic;

namespace Rhisis.World.Game.Maps
{
    public class MapLayer : Context, IMapLayer
    {
        private readonly IList<IMapRegion> _regions;
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        /// <inheritdoc />
        public int Id { get; }

        /// <inheritdoc />
        public IMapInstance Parent { get; }

        /// <inheritdoc />
        public ICollection<IMapRegion> Regions => this._regions;

        /// <summary>
        /// Creates a new <see cref="MapLayer"/> instance.
        /// </summary>
        /// <param name="parent">Parent map instance</param>
        /// <param name="id">Map layer id</param>
        public MapLayer(IMapInstance parent, int id)
        {
            this.Id = id;
            this.Parent = parent;
            this._regions = new List<IMapRegion>();

            foreach (IMapRegion region in this.Parent.Regions)
            {
                if (region is IMapRespawnRegion respawner && respawner.ObjectType == WorldObjectType.Mover)
                {
                    var respawnerRegion = respawner.Clone() as IMapRespawnRegion;

                    for (var i = 0; i < respawnerRegion.Count; i++)
                        this.CreateMonster(respawner);

                    this._regions.Add(respawner);
                }
            }
        }

        /// <inheritdoc />
        public override void Update()
        {
            foreach (var entity in this.Entities)
            {
                foreach (var system in this.Systems)
                {
                    if (!(system is INotifiableSystem) && system.Match(entity))
                        system.Execute(entity);
                }
            }
            
            foreach (var region in this._regions)
            {
                if (region.IsActive && region is IMapRespawnRegion respawnRegion)
                {
                    foreach (var entity in respawnRegion.Entities)
                    {
                        foreach (var system in this.Systems)
                        {
                            if (!(system is INotifiableSystem) && system.Match(entity))
                                system.Execute(entity);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Create a new monster.
        /// </summary>
        /// <param name="respawner"></param>
        private void CreateMonster(IMapRespawnRegion respawner)
        {
            var monster = new MonsterEntity(this);
            var moverData = WorldServer.Movers[respawner.ModelId];

            monster.Object = new ObjectComponent
            {
                MapId = this.Parent.Id,
                LayerId = this.Id,
                ModelId = moverData.Id,
                Type = WorldObjectType.Mover,
                Position = respawner.GetRandomPosition(),
                Angle = RandomHelper.FloatRandom(0, 360f),
                Name = moverData.Name,
                Size = ObjectComponent.DefaultObjectSize,
                Spawned = true,
                Level = moverData.Level
            };
            monster.TimerComponent = new TimerComponent
            {
                LastMoveTimer = RandomHelper.LongRandom(8, 20)
            };
            monster.Behavior = WorldServer.MonsterBehaviors.GetBehavior(monster.Object.ModelId);
            monster.Region = respawner;

            respawner.Entities.Add(monster);
        }
    }
}
