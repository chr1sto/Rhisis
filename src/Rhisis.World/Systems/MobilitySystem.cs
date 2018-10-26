using NLog;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Entities;
using System;

namespace Rhisis.World.Systems
{
    [System]
    public class MobilitySystem : SystemBase
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        /// <inheritdoc />
        protected override WorldEntityType Type => WorldEntityType.Player | WorldEntityType.Monster;

        /// <summary>
        /// Creates a new <see cref="MobilitySystem"/> instance.
        /// </summary>
        /// <param name="context"></param>
        public MobilitySystem(IContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Executes the <see cref="MobilitySystem"/> logic.
        /// </summary>
        /// <param name="entity">Current entity</param>
        public override void Execute(IEntity entity)
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
            if (entity.MovableComponent.DestinationPosition.IsInCircle(entity.Object.Position, 0.5f))
            {
                entity.MovableComponent.DestinationPosition.Reset();
                Logger.Debug($"Player {entity.Object.Name} has arrived.");
            }
            else
            {
                double speed = ((entity.MovableComponent.Speed * 100f) * this.Context.GameTime);
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
