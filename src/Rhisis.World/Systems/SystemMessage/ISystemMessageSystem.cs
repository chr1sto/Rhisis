using Rhisis.World.Game.Entities;

namespace Rhisis.World.Systems.SystemMessage
{
    public interface ISystemMessageSystem
    {
        /// <summary>
        /// Sends a system message.
        /// </summary>
        /// <param name="player">player entity.</param>
        /// <param name="sysMessage">System message.</param>
        void SystemMessage(IPlayerEntity player, string sysMessage);
    }
}
