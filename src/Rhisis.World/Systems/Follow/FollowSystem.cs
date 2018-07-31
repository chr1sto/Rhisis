using Rhisis.World.Game.Core;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;
using System;
using System.Linq.Expressions;

namespace Rhisis.World.Systems.Follow
{
    [System]
    public sealed class FollowSystem : NotifiableSystemBase
    {
        protected override Expression<Func<IEntity, bool>> Filter => 
            x => x.Type == WorldEntityType.Player || x.Type == WorldEntityType.Monster;

        public FollowSystem(IContext context) 
            : base(context)
        {
        }

        public override void Execute(IEntity entity, SystemEventArgs e)
        {
            if (!(entity is IMovableEntity movableEntity) || !e.CheckArguments())
                throw new SystemException("FollowSystem: Invalid arguments");

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
                throw new SystemException($"Cannot find entity with object id: {e.TargetId} around {entity.Object.Name}");

            entity.Follow.Target = entityToFollow;
            WorldPacketFactory.SendFollowTarget(entity, entityToFollow, e.Distance);
        }
    }
}
