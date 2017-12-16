using Rhisis.Core.IO;
using Rhisis.World.Core;
using Rhisis.World.Core.Systems;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Interfaces;
using System;
using System.Linq.Expressions;

namespace Rhisis.World.Systems
{
    [System]
    public class MobilitySystem : SystemBase
    {
        protected override Expression<Func<IEntity, bool>> Filter => x => x.Type == WorldEntityType.Player || x.Type == WorldEntityType.Monster;

        public MobilitySystem(IContext context)
            : base(context)
        {
        }

        public override void Execute(IEntity entity)
        {
            //var entityObjectComponent = entity.GetComponent<ObjectComponent>();
            //var entityMovableComponent = entity.GetComponent<MovableComponent>();

            //if (entityMovableComponent.DestinationPosition.IsZero())
            //    return;

            //this.Walk(entityObjectComponent, entityMovableComponent);
        }

        //private void Walk(ObjectComponent objectComponent, MovableComponent movableComponent)
        //{
            //if (movableComponent.DestinationPosition.IsInCircle(objectComponent.Position, 0.1f))
            //{
            //    Logger.Debug("{0} arrived to destination", objectComponent.Name);
            //    movableComponent.DestinationPosition.Reset();
            //}
            //else
            //{
            //    double speed = ((movableComponent.Speed * 100) * this.Context.Time);
            //    float distanceX = movableComponent.DestinationPosition.X - objectComponent.Position.X;
            //    float distanceZ = movableComponent.DestinationPosition.Z - objectComponent.Position.Z;
            //    double distance = Math.Sqrt(distanceX * distanceX + distanceZ * distanceZ);

            //    // Normalize
            //    double deltaX = distanceX / distance;
            //    double deltaZ = distanceZ / distance;
            //    double offsetX = deltaX * speed;
            //    double offsetZ = deltaZ * speed;
                
            //    if (Math.Abs(offsetX) > Math.Abs(distanceX))
            //        offsetX = distanceX;
            //    if (Math.Abs(offsetZ) > Math.Abs(distanceZ))
            //        offsetZ = distanceZ;
                
            //    objectComponent.Position.X += (float)offsetX;
            //    objectComponent.Position.Z += (float)offsetZ;

            //    //Logger.Debug("Moving: {0}, {1}", offsetX, offsetZ);
            //    //Logger.Debug("DestinationPosition: {0}", movableComponent.DestinationPosition);
            //    //Logger.Debug("CurrentPosition: {0}", objectComponent.Position);
            //}
        //}
    }
}
