using Rhisis.Core.Common;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Structures;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Maps;
using Rhisis.World.Packets;
using System;

namespace Rhisis.World.Systems.Mobility
{
    [Injectable]
    public sealed class MobilitySystem : IMobilitySystem
    {
        private const float ArrivalRangeRadius = 1f;
        private readonly float _updateRate = MapInstance.UpdateRate / MapInstance.FrameRate;
        private readonly IMoverPacketFactory _moverPacketFactory;

        public MobilitySystem(IMoverPacketFactory moverPacketFactory)
        {
            _moverPacketFactory = moverPacketFactory;
        }

        /// <inheritdoc />
        public void CalculatePosition(ILivingEntity entity)
        {
            if (!entity.Object.Spawned)
            {
                return;
            }

            if (entity.Moves.DestinationPosition.IsZero() || entity.Object.MovingFlags.HasFlag(ObjectState.OBJSTA_STAND))
            {
                return;
            }

            if (entity.Follow.IsFollowing)
            {
                if (entity.Object.Position.IsInCircle(entity.Follow.Target.Object.Position, entity.Follow.FollowDistance))
                {
                    entity.Moves.DestinationPosition.Reset();
                    entity.Object.MovingFlags &= ~ObjectState.OBJSTA_FMOVE;
                    entity.Object.MovingFlags |= ObjectState.OBJSTA_STAND;
                    entity.Behavior?.OnArrived();
                }
                else
                {
                    entity.Moves.DestinationPosition.Copy(entity.Follow.Target.Object.Position);
                    entity.Object.MovingFlags &= ~ObjectState.OBJSTA_STAND;
                    entity.Object.MovingFlags |= ObjectState.OBJSTA_FMOVE;
                }
            }

            Walk(entity);
        }

        /// <summary>
        /// Process the walk algorithm.
        /// </summary>
        /// <param name="entity">Current entity</param>
        private void Walk(ILivingEntity entity)
        {
            if (entity.Object.Position.IsInCircle(entity.Moves.DestinationPosition, ArrivalRangeRadius))
            {
                entity.Object.Position.Copy(entity.Moves.DestinationPosition);
                entity.Moves.DestinationPosition.Reset();

                if (!entity.Battle.IsFighting)
                {
                    entity.Object.MovingFlags &= ~ObjectState.OBJSTA_FMOVE;
                    entity.Object.MovingFlags |= ObjectState.OBJSTA_STAND;
                    _moverPacketFactory.SendMotion(entity, ObjectMessageType.OBJMSG_STAND);
                }

                entity.Behavior.OnArrived();
            }
            else
            {
                entity.Object.Angle = Vector3.AngleBetween(entity.Object.Position, entity.Moves.DestinationPosition);
                float entitySpeed = GetEntitySpeed(entity);

                if (entity.Object.MotionFlags.HasFlag(StateFlags.OBJSTAF_WALK))
                {
                    entitySpeed /= 4f;
                }
                else if (entity.Object.MovingFlags.HasFlag(ObjectState.OBJSTA_BMOVE))
                {
                    entitySpeed /= 5f;
                }

                float distanceX = entity.Moves.DestinationPosition.X - entity.Object.Position.X;
                float distanceZ = entity.Moves.DestinationPosition.Z - entity.Object.Position.Z;
                double distance = Math.Sqrt(distanceX * distanceX + distanceZ * distanceZ);

                // Normalize
                double offsetX = (distanceX / distance) * entitySpeed;
                double offsetZ = (distanceZ / distance) * entitySpeed;

                if (Math.Abs(offsetX) > Math.Abs(distanceX))
                {
                    offsetX = distanceX;
                }

                if (Math.Abs(offsetZ) > Math.Abs(distanceZ))
                {
                    offsetZ = distanceZ;
                }

                UpdatePosition(entity, (float)offsetX, (float)offsetZ);
            }
        }

        private float GetEntitySpeed(ILivingEntity entity)
        {
            float entitySpeed = entity.Moves.Speed * entity.Moves.SpeedFactor; // TODO: Add speed bonuses

            if (entity.Type == WorldEntityType.Monster)
            {
                entitySpeed /= 2f;
            }

            entitySpeed *= _updateRate;

            return entitySpeed;
        }

        private void UpdatePosition(ILivingEntity entity, float x, float z)
        {
            if (entity is IPlayerEntity && entity.Moves.IsMovingWithKeyboard)
            {
                float angle = entity.Object.Angle;
                var movementX = (float)(Math.Sin(angle * (Math.PI / 180)) * Math.Sqrt(x * x + z * z));
                var movementZ = (float)(Math.Cos(angle * (Math.PI / 180)) * Math.Sqrt(x * x + z * z));

                if (entity.Object.MovingFlags.HasFlag(ObjectState.OBJSTA_FMOVE))
                {
                    entity.Object.Position.X += movementX;
                    entity.Object.Position.Z -= movementZ;
                }
                else if (entity.Object.MovingFlags.HasFlag(ObjectState.OBJSTA_BMOVE))
                {
                    entity.Object.Position.X -= movementX;
                    entity.Object.Position.Z += movementZ;
                }
            }
            else
            {
                entity.Object.Position.X += x;
                entity.Object.Position.Z += z;
            }
        }
    }
}
