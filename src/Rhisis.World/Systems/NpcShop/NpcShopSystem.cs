using System;
using System.Linq.Expressions;
using Rhisis.Core.IO;
using Rhisis.Core.Structures.Game;
using Rhisis.World.Core.Systems;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Interfaces;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;

namespace Rhisis.World.Systems.NpcShop
{
    [System]
    public class NpcShopSystem : NotifiableSystemBase
    {
        /// <inheritdoc />
        protected override Expression<Func<IEntity, bool>> Filter => x => x.Type == WorldEntityType.Player;

        /// <inheritdoc />
        public NpcShopSystem(IContext context) 
            : base(context)
        {
        }

        /// <inheritdoc />
        public override void Execute(IEntity entity, EventArgs e)
        {
            if (!(entity is IPlayerEntity player) || !(e is NpcShopEventArgs shopEvent))
            {
                Logger.Error("Invalid arguments");
                return;
            }

            if (!shopEvent.CheckArguments())
            {
                return;
            }

            switch (shopEvent.ActionType)
            {
                case NpcShopActionType.Open:
                    this.OpenShop(player, shopEvent as NpcShopOpenEventArgs);
                    break;

                case NpcShopActionType.Buy:
                    break;
                case NpcShopActionType.Sell:
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Opens the NPC Shop.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="e"></param>
        private void OpenShop(IPlayerEntity player, NpcShopOpenEventArgs e)
        {
            var npc = player.Context.FindEntity(e.NpcObjectId) as INpcEntity;

            if (npc == null)
            {
                Logger.Error("Cannot find NPC with object id : {0}", e.NpcObjectId);
                return;
            }

            if (npc.Shop == null)
            {
                Logger.Error("NPC '{0}' doesn't have a shop.", npc.ObjectComponent.Name);
                return;
            }

            WorldPacketFactory.SendNpcShop(player, npc);
        }
    }
}
