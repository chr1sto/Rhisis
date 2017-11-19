using Rhisis.World.Core;
using Rhisis.World.Core.Components;
using Rhisis.World.Core.Entities;
using Rhisis.World.Core.Systems;
using Rhisis.World.Packets;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Rhisis.World.Systems
{
    [System]
    public class VisibilitySystem : UpdateSystemBase
    {
        private static readonly float VisibilityRange = 75f;

        protected override Expression<Func<IEntity, bool>> Filter => x => x.HasComponent<PlayerComponent>() && x.HasComponent<ObjectComponent>();

        public VisibilitySystem(IContext context)
            : base(context)
        {
        }

        public override void Execute(IEntity entity)
        {
            var entityObjectComponent = entity.GetComponent<ObjectComponent>();
            var entityPlayerComponent = entity.GetComponent<PlayerComponent>();
            IEntity[] entitiesAround = this.GetEntitiesAround(entityObjectComponent);
            IEntity[] entitiesOut = this.GetEntitiesOut(entityObjectComponent, entitiesAround);

            for (int i = 0; i < entitiesAround.Length; i++)
            {
                if (!entityObjectComponent.Entities.Contains(entitiesAround[i]))
                {
                    WorldPacketFactory.SendSpawnObject(entityPlayerComponent.Connection, entitiesAround[i]);
                    entityObjectComponent.Entities.Add(entitiesAround[i]);
                }
            }

            if (entitiesOut.Any())
            {
                for (int i = entitiesOut.Length - 1; i >= 0; i--)
                {
                    WorldPacketFactory.SendDespawnObject(entityPlayerComponent.Connection, entitiesOut[0]);
                    entityObjectComponent.Entities.RemoveAt(0);
                }
            }
        }

        private IEntity[] GetEntitiesAround(ObjectComponent entityObjectComponent)
        {
            var entitiesAround = from x in this.Entities
                                 let otherEntityObjectComponent = x.GetComponent<ObjectComponent>()
                                 where otherEntityObjectComponent != null &&
                                       entityObjectComponent.Position.IsInCircle(otherEntityObjectComponent.Position, VisibilityRange) &&
                                       entityObjectComponent.ObjectId != otherEntityObjectComponent.ObjectId &&
                                       otherEntityObjectComponent.Spawned
                                 select x;

            return entitiesAround.ToArray();
        }

        private IEntity[] GetEntitiesOut(ObjectComponent entityObjectComponent, IEntity[] entitiesAround)
        {
            var entitiesOut = from x in entityObjectComponent.Entities
                              where !entitiesAround.Contains(x)
                              select x;

            return entitiesOut.ToArray();
        }
    }
}
