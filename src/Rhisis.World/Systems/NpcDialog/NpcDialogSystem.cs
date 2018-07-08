using System;
using System.Linq;
using System.Linq.Expressions;
using Rhisis.Core.IO;
using Rhisis.Core.Structures.Game;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;

namespace Rhisis.World.Systems.NpcDialog
{
    [System]
    public class NpcDialogSystem : NotifiableSystemBase
    {
        /// <inheritdoc />
        protected override Expression<Func<IEntity, bool>> Filter => x => x.Type == WorldEntityType.Player;

        /// <inheritdoc />
        public NpcDialogSystem(IContext context) : base(context)
        {
        }

        /// <inheritdoc />
        public override void Execute(IEntity entity, SystemEventArgs e)
        {
            if (!(entity is IPlayerEntity player) || !(e is NpcDialogOpenEventArgs dialogEvent))
            {
                Logger.Error("DialogSystem: Invalid event arguments.");
                return;
            }

            if (!dialogEvent.CheckArguments())
            {
                Logger.Error("DialogSystem: Invalid event action arguments.");
                return;
            }

            this.OpenDialog(player, dialogEvent);
        }

        /// <summary>
        /// Open a NPC dialog box.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="e"></param>
        private void OpenDialog(IPlayerEntity player, NpcDialogOpenEventArgs e)
        {
            var npcEntity = player.Context.FindEntity<INpcEntity>(e.NpcObjectId);

            if (npcEntity == null)
            {
                Logger.Error("DialogSystem: Cannot find NPC with id: {0}", e.NpcObjectId);
                return;
            }

            if (!npcEntity.Data.HasDialog)
            {
                Logger.Error("DialogSystem: NPC '{0}' doesn't have a dialog.", npcEntity.Object.Name);
                return;
            }

            string dialogText = npcEntity.Data.Dialog.IntroText;

            if (!string.IsNullOrEmpty(e.DialogKey))
            {
                if (e.DialogKey == "BYE")
                {
                    WorldPacketFactory.SendChatTo(npcEntity, player, npcEntity.Data.Dialog.ByeText);
                    WorldPacketFactory.SendCloseDialog(player);
                    return;
                }
                else
                {
                    DialogLink dialogLink = npcEntity.Data.Dialog.Links?.FirstOrDefault(x => x.Id == e.DialogKey);

                    if (dialogLink == null)
                    {
                        Logger.Error("DialogSystem: Cannot find dialog key: '{0}' for NPC '{1}'", e.DialogKey, npcEntity.Object.Name);
                        return;
                    }

                    dialogText = dialogLink.Text;
                }
            }

            WorldPacketFactory.SendDialog(player, dialogText, npcEntity.Data.Dialog.Links);
        }
    }
}