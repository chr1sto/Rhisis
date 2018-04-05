using System;
using Rhisis.Core.Common;
using Rhisis.Core.Helpers;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Regions;

namespace Rhisis.World.Game.Maps
{
    public class MapLayer : Context, IMapLayer
    {
        /// <inheritdoc />
        public int Id { get; }

        /// <inheritdoc />
        public IMapInstance Parent { get; }

        public MapLayer(IMapInstance parent, int id)
        {
            this.Id = id;
            this.Parent = parent;
            
            foreach (IRegion region in this.Parent.Regions)
            {
                if (region is RespawnerRegion respawner && respawner.ObjectType == (int)WorldObjectType.Mover)
                {
                    for (var i = 0; i < respawner.Count; i++)
                        this.CreateMonster(respawner);
                }
            }
        }
        
        private void CreateMonster(RespawnerRegion respawner)
        {
            var monster = this.CreateEntity<MonsterEntity>();

            monster.Object = new ObjectComponent
            {
                MapId = this.Parent.Id,
                LayerId = this.Id,
                ModelId = respawner.MoverId,
                Type = WorldObjectType.Mover,
                Position = respawner.GetRandomPosition(),
                Angle = RandomHelper.FloatRandom(0, 360f),
                Name = WorldServer.Movers[respawner.MoverId].Name,
                Size = ObjectComponent.DefaultObjectSize,
                Spawned = true,
                Level = WorldServer.Movers[respawner.MoverId].Level
            };
        }
    }
}
