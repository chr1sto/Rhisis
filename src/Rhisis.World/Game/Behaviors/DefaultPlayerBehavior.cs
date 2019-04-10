using Rhisis.World.Game.Entities;

namespace Rhisis.World.Game.Behaviors
{
    [Behavior(BehaviorType.Player, IsDefault: true)]
    public sealed class DefaultPlayerBehavior : IBehavior<IPlayerEntity>
    {
        /// <inheritdoc />
        public void Update(IPlayerEntity entity)
        {
            this.UpdateFollowState(entity);
        }

        /// <inheritdoc />
        public void OnArrived(IPlayerEntity entity)
        {
            // TODO
        }

        private void UpdateFollowState(IPlayerEntity entity)
        {
            if (entity.Follow.IsFollowing)
                entity.MovableComponent.DestinationPosition = entity.Follow.Target.Object.Position.Clone();
        }
    }
}
