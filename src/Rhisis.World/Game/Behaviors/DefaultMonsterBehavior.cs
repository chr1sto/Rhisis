using Rhisis.Core.Helpers;
using Rhisis.Core.IO;
using Rhisis.Core.Structures;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Follow;

namespace Rhisis.World.Game.Behaviors
{
    /// <summary>
    /// Default behavior for all monsters.
    /// </summary>
    [Behavior(BehaviorType.Monster, IsDefault: true)]
    public class DefaultMonsterBehavior : IBehavior<IMonsterEntity>
    {
        /// <inheritdoc />
        public virtual void Update(IMonsterEntity entity)
        {
            this.UpdateArivalState(entity);

            if (!entity.Follow.IsFollowing)
                this.UpdateMoves(entity);
            else
                this.Follow(entity);
            
            this.Fight(entity);
        }

        /// <summary>
        /// Update monster moves.
        /// </summary>
        /// <param name="monster"></param>
        private void UpdateMoves(IMonsterEntity monster)
        {
            if (monster.TimerComponent.LastMoveTimer <= Time.TimeInSeconds() && monster.MovableComponent.HasArrived)
            {
                this.MoveToPosition(monster, monster.Region.GetRandomPosition());
            }
        }

        /// <summary>
        /// Update monster's arrival state when it arrives at Destination position.
        /// </summary>
        /// <param name="monster"></param>
        private void UpdateArivalState(IMonsterEntity monster)
        {
            if (monster.MovableComponent.HasArrived && monster.MovableComponent.ReturningToOriginalPosition)
            {
                if (monster.MovableComponent.SpeedFactor >= 2)
                {
                    monster.MovableComponent.SpeedFactor = 1f;
                    WorldPacketFactory.SendSpeedFactor(monster, monster.MovableComponent.SpeedFactor);
                }

                monster.MovableComponent.ReturningToOriginalPosition = false;
            }
        }

        /// <summary>
        /// Update the follow state.
        /// </summary>
        /// <param name="monster"></param>
        private void Follow(IMonsterEntity monster)
        {
            if (!monster.Object.Position.IsInCircle(monster.MovableComponent.BeginPosition, 30f))
            {
                monster.Follow.Target = null;
                monster.MovableComponent.ReturningToOriginalPosition = true;
                monster.MovableComponent.SpeedFactor = 2.66f;

                WorldPacketFactory.SendSpeedFactor(monster, monster.MovableComponent.SpeedFactor);
                this.MoveToPosition(monster, monster.MovableComponent.BeginPosition);
                return;
            }

            monster.MovableComponent.DestinationPosition = monster.Follow.Target.Object.Position.Clone();
            monster.NotifySystem<FollowSystem>(new FollowEventArgs(monster.Follow.Target.Id, 1f));
        }

        /// <summary>
        /// Moves the monster to a position.
        /// </summary>
        /// <param name="monster"></param>
        /// <param name="destPosition"></param>
        private void MoveToPosition(IMonsterEntity monster, Vector3 destPosition)
        {
            monster.TimerComponent.LastMoveTimer = Time.TimeInSeconds() + RandomHelper.LongRandom(8, 20);
            monster.MovableComponent.DestinationPosition = destPosition.Clone();
            monster.Object.Angle = Vector3.AngleBetween(monster.Object.Position, destPosition);

            WorldPacketFactory.SendDestinationPosition(monster);
        }

        private void Fight(IMonsterEntity monster)
        {
            // TODO
        }
    }
}
