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
        /// <param name="buttons">Dialog buttons.</param>
        /// <param name="questId">Quest id.</param>
        void SendDialog(IPlayerEntity player, IEnumerable<string> dialogTexts, IEnumerable<DialogLink> dialogLinks, IEnumerable<DialogLink> buttons = null, int questId = 0);

        /// <summary>
        /// Sends a npc quest list (new or in progress) to a player.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="newQuests">New quests.</param>
        /// <param name="currentQuests">Quests in progress.</param>
        void SendQuestDialogs(IPlayerEntity player, IEnumerable<DialogLink> newQuests, IEnumerable<DialogLink> currentQuests);

        /// <summary>
        /// Send a packet to close the NPC dialog box.
        /// </summary>
        /// <param name="player"></param>
        void SendCloseDialog(IPlayerEntity player);
    }
}
