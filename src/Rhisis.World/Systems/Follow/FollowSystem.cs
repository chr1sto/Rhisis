using Rhisis.Core.Exceptions;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;

namespace Rhisis.World.Systems.Follow
{
    [System]
    public sealed class FollowSystem : NotifiableSystemBase
    {
        /// <inheritdoc />
        protected override WorldEntityType Type => WorldEntityType.Player | WorldEntityType.Monster;

        /// <summary>
        /// Creates a new <see cref="FollowSystem"/> instance.
        /// </summary>
        /// <param name="context"></param>
        public FollowSystem(IContext context) 
            : base(context)
        {
        }

        /// <inheritdoc />
        public override void Execute(IEntity entity, SystemEventArgs e)
        {
            if (!(entity is IMovableEntity movableEntity) || !e.CheckArguments())
                throw new RhisisSystemException("FollowSystem: Invalid arguments");

            switch (e)
            {
                case FollowEventArgs followEvent:
                    this.OnFollow(movableEntity, followEvent);
                    break;
            }
        }

        private void OnFollow(IMovableEntity entity, FollowEventArgs e)
        {
            var entityToFollow = entity.FindEntity<IEntity>(e.TargetId);

            if (entityToFollow == null)
                throw new RhisisSystemException($"Cannot find entity with object id: {e.TargetId} around {entity.Object.Name}");

            entity.Follow.Target = entityToFollow;
            WorldPacketFactory.SendFollowTarget(entity, entityToFollow, e.Distance);
        }
    }
}
