using Rhisis.Core.IO;
using Rhisis.World.Core;
using Rhisis.World.Core.Components;
using Rhisis.World.Core.Entities;
using Rhisis.World.Core.Systems;
using Rhisis.World.Packets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Systems
{
    [System]
    public class VisibilitySystem : UpdateSystemBase
    {
        private static readonly float VisbilityRange = 75f;

        public override Func<IEntity, bool> Filter => x => x.HasComponent<PlayerComponent>();

        public VisibilitySystem(IContext context)
            : base(context)
        {
        }
        
        public override void Execute(IEntity entity)
        {
            var entityPlayerComponent = entity.GetComponent<PlayerComponent>();
            var entityObjectComponent = entity.GetComponent<ObjectComponent>();
            IEnumerable<IEntity> otherEntitiesAround = this.Context.Entities.Where(x => this.CanSee(entity, x));
            IEnumerable<IEntity> otherEntitiesOut = entityObjectComponent.Entities.Where(x => !otherEntitiesAround.Contains(x));

            foreach (IEntity entityInRange in otherEntitiesAround)
            {
                if (!entityObjectComponent.Entities.Contains(entityInRange))
                {
                    entityObjectComponent.Entities.Add(entityInRange);

                    if (entityPlayerComponent != null)
                        WorldPacketFactory.SendSpawnObject(entityPlayerComponent.Connection, entityInRange);
                }
            }

            for (int i = otherEntitiesOut.Count(); i > 0; i--)
            {
                if (entityPlayerComponent != null)
                    WorldPacketFactory.SendDespawnObject(entityPlayerComponent.Connection, otherEntitiesOut.ElementAt(0));
                entityObjectComponent.Entities.RemoveAt(0);
            }
        }

        private bool CanSee(IEntity entity, IEntity otherEntity)
        {
            var entityObjectComponent = entity.GetComponent<ObjectComponent>();
            var otherEntityObjectComponent = otherEntity.GetComponent<ObjectComponent>();

            return entityObjectComponent.Position.IsInCircle(otherEntityObjectComponent.Position, VisbilityRange)
                && entityObjectComponent.ObjectId != otherEntityObjectComponent.ObjectId;
        }
    }
}
