using Rhisis.Core.IO;
using Rhisis.World.Core.Systems;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Interfaces;
using Rhisis.World.Game.Entities;
using System;
using System.Linq.Expressions;

namespace Rhisis.World.Systems
{
    [System]
    public class MobilitySystem : SystemBase
    {
        protected override Expression<Func<IEntity, bool>> Filter => x => x.Type == WorldEntityType.Player || x.Type == WorldEntityType.Monster;

        public MobilitySystem(IContext context)
            : base(context)
        {
        }

        public override void Execute(IEntity entity)
        {
            var movableEntity = entity as IMovableEntity;

            if (movableEntity.MovableComponent.DestinationPosition.IsZero())
                return;

            this.Walk(movableEntity);
        }

        private void Walk(IMovableEntity entity)
        {
            if (entity.MovableComponent.DestinationPosition.IsInCircle(entity.ObjectComponent.Position, 0.1f))
            {
                Logger.Debug("{0} arrived to destination", entity.ObjectComponent.Name);
                entity.MovableComponent.DestinationPosition.Reset();
            }
            else
            {
                double speed = ((entity.MovableComponent.Speed * 100) * this.Context.Time);
                float distanceX = entity.MovableComponent.DestinationPosition.X - entity.ObjectComponent.Position.X;
                float distanceZ = entity.MovableComponent.DestinationPosition.Z - entity.ObjectComponent.Position.Z;
                double distance = Math.Sqrt(distanceX * distanceX + distanceZ * distanceZ);

                // Normalize
                double deltaX = distanceX / distance;
                double deltaZ = distanceZ / distance;
                double offsetX = deltaX * speed;
                double offsetZ = deltaZ * speed;

                if (Math.Abs(offsetX) > Math.Abs(distanceX))
                    offsetX = distanceX;
                if (Math.Abs(offsetZ) > Math.Abs(distanceZ))
                    offsetZ = distanceZ;

                entity.ObjectComponent.Position.X += (float)offsetX;
                entity.ObjectComponent.Position.Z += (float)offsetZ;

                Logger.Debug("Moving: {0}, {1}", offsetX, offsetZ);
                Logger.Debug("DestinationPosition: {0}", entity.MovableComponent.DestinationPosition);
                Logger.Debug("CurrentPosition: {0}", entity.ObjectComponent.Position);
            }
        }
    }
}
