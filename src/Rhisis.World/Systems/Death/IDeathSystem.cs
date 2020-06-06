using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Maps.Regions;

namespace Rhisis.World.Systems.Death
{
    public interface IDeathSystem
    {
        /// <summary>
        /// Resurects the player to the nearst lodelight revival point.
        /// </summary>
        /// <param name="player">Current dead player.</param>
        void ResurectToLodelight(IPlayerEntity player);

        /// <summary>
        /// Applies death penality if enabled.
        /// </summary>
        /// <param name="player">Current dead player.</param>
        /// <param name="sendToPlayer">Optionnal parameter indicating if the experience should be sent to the player.</param>
        void ApplyDeathPenality(IPlayerEntity player, bool sendToPlayer = true);

        /// <summary>
        /// Applies revival health penality to the player.
        /// </summary>
        /// <param name="player">Current dead player.</param>
        void ApplyRevivalHealthPenality(IPlayerEntity player);

        /// <summary>
        /// Gets nearest revival region based on the player information.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <returns></returns>
        IMapRevivalRegion GetNearestRevivalRegion(IPlayerEntity player);
    }
}
