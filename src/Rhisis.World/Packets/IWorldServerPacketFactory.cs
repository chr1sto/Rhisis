using Rhisis.World.Client;

namespace Rhisis.World.Packets
{
    public interface IWorldServerPacketFactory
    {
        /// <summary>
        /// Sends a welcome packet to the client.
        /// </summary>
        /// <param name="player">Current client.</param>
        void SendWelcome(IWorldServerClient serverClient, uint sessionId);
    }
}
