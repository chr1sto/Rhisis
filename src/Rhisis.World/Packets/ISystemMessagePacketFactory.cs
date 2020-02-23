using Rhisis.World.Game.Entities;

namespace Rhisis.World.Packets
{
    public interface ISystemMessagePacketFactory
    {
        /// <summary>
        /// Sends a system message packet to all the worldserver's players.
        /// </summary>
        /// <param name="message">Message</param>
        void SendSystemMessage(IPlayerEntity player, string message);
    }
}
