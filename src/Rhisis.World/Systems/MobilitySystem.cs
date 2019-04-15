using Rhisis.Core.Data;
using Rhisis.Core.IO;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Systems;
using Rhisis.World.Game.Entities;
using System;

namespace Rhisis.World.Systems
{
    [System]
    public class MobilitySystem : ISystem
    {
        private const float ArrivalRangeRadius = 1f;

        /// <inheritdoc />
        public WorldEntityType Type => WorldEntityType.Player | WorldEntityType.Monster;

        /// <summary>
        /// Executes the <see cref="MobilitySystem"/> logic.
        /// </summary>
        /// <param name="entity">Current entity</param>
        public void Execute(IEntity entity, SystemEventArgs args)
        {
            var movableEntity = entity as IMovableEntity;

            if (movableEntity.MovableComponent.NextMoveTime > Time.GetElapsedTime())
                return;

            movableEntity.MovableComponent.NextMoveTime = Time.GetElapsedTime() + 10;

            if (movableEntity.MovableComponent.DestinationPosition.IsZero())
                return;

            if (movableEntity.Object.MovingFlags.HasFlag(ObjectState.OBJSTA_STAND))
                return;

            if (movableEntity.Follow.IsFollowing)
            {
                if (!movableEntity.Object.Position.IsInCircle(movableEntity.Follow.Target.Object.Position, movableEntity.Follow.FollowDistance))
                {
                    movableEntity.MovableComponent.DestinationPosition = movableEntity.Follow.Target.Object.Position.Clone();
                    movableEntity.Object.MovingFlags &= ~ObjectState.OBJSTA_STAND;
                    movableEntity.Object.MovingFlags |= ObjectState.OBJSTA_FMOVE;
                }
                if (movableEntity.Object.Position.IsInCircle(movableEntity.Follow.Target.Object.Position, movableEntity.Follow.FollowDistance) &&
                    !movableEntity.Object.MovingFlags.HasFlag(ObjectState.OBJSTA_STAND))
                {
                    // Arrived
                    movableEntity.Object.MovingFlags = ObjectState.OBJSTA_STAND;
                }
            }

            this.Walk(movableEntity);
        }

        /// <summary>
        /// Process the walk algorithm.
        /// </summary>
        /// <param name="entity">Current entity</param>
        private void Walk(IMovableEntity entity)
        {
            if (entity.Object.Position.IsInCircle(entity.MovableComponent.DestinationPosition, ArrivalRangeRadius))
            {
                entity.MovableComponent.HasArrived = true;
                entity.MovableComponent.DestinationPosition = entity.Object.Position.Clone();
                entity.Object.MovingFlags &= ~ObjectState.OBJSTA_FMOVE;
                entity.Object.MovingFlags |= ObjectState.OBJSTA_STAND;

                if (entity is IMonsterEntity monster)
                    monster.Behavior.OnArrived(monster);
                else if (entity is IPlayerEntity player)
                    player.Behavior.OnArrived(player);
            }
            else
            {
                entity.MovableComponent.HasArrived = false;
                float entitySpeed = entity.MovableComponent.Speed * entity.MovableComponent.SpeedFactor; // TODO: Add speed bonuses
                float distanceX = entity.MovableComponent.DestinationPosition.X - entity.Object.Position.X;
                float distanceZ = entity.MovableComponent.DestinationPosition.Z - entity.Object.Position.Z;
                double distance = Math.Sqrt(distanceX * distanceX + distanceZ * distanceZ);
                
                // Normalize
                double deltaX = distanceX / distance;
                double deltaZ = distanceZ / distance;
                double offsetX = deltaX * entitySpeed;
                double offsetZ = deltaZ * entitySpeed;

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
