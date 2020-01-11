using Rhisis.World.Game.Entities;

namespace Rhisis.World.Packets
{
    public interface ISkillPacketFactory
    {
        /// <summary>
        /// Sends the skill tree update to the given player.
        /// </summary>
        /// <param name="player">Current player.</param>
        void SendSkillTreeUpdate(IPlayerEntity player);
    }
}
