using NLog;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;

namespace Rhisis.World.Systems.Follow
{
    [System]
    public sealed class FollowSystem : NotifiableSystemBase
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

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
            {
                Logger.Error("FollowSystem: Invalid arguments");
                return;
            }

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
            {
                Logger.Error($"Cannot find entity with object id: {e.TargetId} around {entity.Object.Name}");
                return;
            }

            entity.Follow.Target = entityToFollow;
            WorldPacketFactory.SendFollowTarget(entity, entityToFollow, e.Distance);
        }
    }
}
