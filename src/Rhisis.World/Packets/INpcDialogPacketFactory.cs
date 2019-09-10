using System.Collections.Generic;
using Rhisis.World.Game.Entities;
using Rhisis.Core.Structures.Game.Dialogs;

namespace Rhisis.World.Packets
{
    public interface INpcDialogPacketFactory
    {
        /// <summary>
        /// Sends a NPC dialog to a player.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="dialogTexts">Npc dialog texts.</param>
        /// <param name="dialogLinks">Npc dialog links.</param>
        /// <param name="questButtons">Quest buttons.</param>
        /// <param name="questId">Quest id.</param>
        void SendDialog(IPlayerEntity player, IEnumerable<string> dialogTexts, IEnumerable<DialogLink> dialogLinks, IEnumerable<DialogLink> questButtons = null, int questId = 0);

        /// <summary>
        /// Send a packet to close the NPC dialog box.
        /// </summary>
        /// <param name="player"></param>
        void SendCloseDialog(IPlayerEntity player);
    }
}
