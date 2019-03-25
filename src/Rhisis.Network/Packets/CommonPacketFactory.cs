using Ether.Network.Common;

namespace Rhisis.Network.Packets
{
    /// <summary>
    /// Packet Factory common to all servers.
    /// </summary>
    public static class CommonPacketFactory
    {
        /// <summary>
        /// Send a welcome packet to the client.
        /// </summary>
        /// <param name="client">Client</param>
        /// <param name="sessionId">Session id</param>
        public static void SendWelcome(INetUser client, uint sessionId)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.WELCOME);
                packet.Write(sessionId);

                client.Send(packet);
            }
        }

        /// <summary>
        /// Send a pong response to the client.
        /// </summary>
        /// <param name="client">Client</param>
        /// <param name="time">Current time</param>
        public static void SendPong(INetUser client, int time)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.PING);
                packet.Write(time);

                client.Send(packet);
            }
        }
    }
}
