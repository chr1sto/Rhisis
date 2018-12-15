using NLog;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Systems;
using Rhisis.World.Game.Entities;
using System;

namespace Rhisis.World.Systems
{
    [System]
    public class MobilitySystem : ISystem
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        /// <inheritdoc />
        public WorldEntityType Type => WorldEntityType.Player | WorldEntityType.Monster;

        /// <summary>
        /// Executes the <see cref="MobilitySystem"/> logic.
        /// </summary>
        /// <param name="entity">Current entity</param>
        public void Execute(IEntity entity, SystemEventArgs args)
        {
            var movableEntity = entity as IMovableEntity;
            
            if (movableEntity.MovableComponent.DestinationPosition.IsZero())
                return;

            this.Walk(movableEntity);
        }

        /// <summary>
        /// Process the walk algorithm.
        /// </summary>
        /// <param name="entity">Current entity</param>
        private void Walk(IMovableEntity entity)
        {
            if (entity.MovableComponent.DestinationPosition.IsInCircle(entity.Object.Position, 0.1f))
            {
                entity.MovableComponent.DestinationPosition.Reset();
            }
            else
            {
                double entitySpeed = entity.MovableComponent.Speed * entity.MovableComponent.SpeedFactor;
                double speed = ((entitySpeed * 100f) * entity.Context.GameTime);
                float distanceX = entity.MovableComponent.DestinationPosition.X - entity.Object.Position.X;
                float distanceZ = entity.MovableComponent.DestinationPosition.Z - entity.Object.Position.Z;
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

                entity.Object.Position.X += (float)offsetX;
                entity.Object.Position.Z += (float)offsetZ;
            }
        }
    }
}
