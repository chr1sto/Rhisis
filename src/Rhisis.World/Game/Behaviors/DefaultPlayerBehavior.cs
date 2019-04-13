using Rhisis.Core.Data;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;
using System;

namespace Rhisis.World.Game.Behaviors
{
    [Behavior(BehaviorType.Player, IsDefault: true)]
    public sealed class DefaultPlayerBehavior : IBehavior<IPlayerEntity>
    {
        /// <inheritdoc />
        public void Update(IPlayerEntity entity)
        {
            // TODO
        }

        /// <inheritdoc />
        public void OnArrived(IPlayerEntity entity)
        {
            if (entity.Follow.IsFollowing && entity.Follow.Target.Type == WorldEntityType.Drop)
            {
                this.PickUpDroppedItem(entity, entity.Follow.Target as IItemEntity);
            }
        }

        /// <summary>
        /// Verify all conditions to pickup a dropped item.
        /// </summary>
        /// <param name="entity">The player trying to pick-up the dropped item.</param>
        /// <param name="droppedItem">The dropped item.</param>
        private void PickUpDroppedItem(IPlayerEntity entity, IItemEntity droppedItem)
        {
            Console.WriteLine($"{entity.Object.Name} picking up {droppedItem.Drop.Item.Quantity} {droppedItem.Drop.Item.Data.Name}");

            // TODO: check if drop belongs to a party.

            if (droppedItem.Drop.HasOwner && droppedItem.Drop.Owner != entity)
            {
                WorldPacketFactory.SendDefinedText(entity, DefineText.TID_GAME_PRIORITYITEMPER, $"\"{droppedItem.Object.Name}\"");
                return;
            }

            if (droppedItem.Drop.IsGold)
            {
                int droppedGoldAmount = droppedItem.Drop.Item.Quantity;
                long gold = entity.PlayerData.Gold + droppedGoldAmount;

                if (gold > int.MaxValue || gold < 0) // Check gold overflow
                {
                    WorldPacketFactory.SendDefinedText(entity, DefineText.TID_GAME_TOOMANYMONEY_USE_PERIN);
                    return;
                }
                else
                {
                    entity.PlayerData.Gold = (int)gold;
                    WorldPacketFactory.SendUpdateAttributes(entity, DefineAttributes.GOLD, entity.PlayerData.Gold);
                    WorldPacketFactory.SendDefinedText(entity, DefineText.TID_GAME_REAPMONEY, droppedGoldAmount.ToString("###,###,###,###"), gold.ToString("###,###,###,###"));
                    WorldPacketFactory.SendMotion(entity, ObjectMessageType.OBJMSG_PICKUP);
                    droppedItem.Delete();
                }
            }
            else
            {
                // TODO: pickup items
            }
        }
    }
}
