using Rhisis.World.Game.Entities;

namespace Rhisis.World.Game.Behaviors
{
    [Behavior(BehaviorType.Player, IsDefault: true)]
    public sealed class DefaultPlayerBehavior : IBehavior<IPlayerEntity>
    {
        public void Update(IPlayerEntity entity)
        {
            this.UpdateFollowState(entity);
        }

        private void UpdateFollowState(IPlayerEntity entity)
        {
            if (entity.Follow.IsFollowing)
            {
                entity.MovableComponent.DestinationPosition = entity.Follow.Target.Object.Position.Clone();
            }
        }
    }
}
