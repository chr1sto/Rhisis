using Rhisis.Core.Helpers;
using Rhisis.Core.IO;
using Rhisis.Core.Structures;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;

namespace Rhisis.World.Game.Behaviors
{
    /// <summary>
    /// Default behavior for all monsters.
    /// </summary>
    [Behavior(BehaviorType.Monster, IsDefault: true)]
    public class DefaultMonsterBehavior : IBehavior<IMonsterEntity>
    {
        public virtual void Update(IMonsterEntity entity)
        {
            this.UpdateMoves(entity);
        }

        private void UpdateMoves(IMonsterEntity entity)
        {
            if (entity.TimerComponent.LastMoveTimer <= Time.TimeInSeconds())
            {
                entity.TimerComponent.LastMoveTimer = Time.TimeInSeconds() + RandomHelper.LongRandom(8, 20);
                entity.MovableComponent.DestinationPosition = entity.Region.GetRandomPosition();
                entity.Object.Angle = Vector3.AngleBetween(entity.Object.Position, entity.MovableComponent.DestinationPosition);
                WorldPacketFactory.SendDestinationPosition(entity);
            }
        }
    }
}
