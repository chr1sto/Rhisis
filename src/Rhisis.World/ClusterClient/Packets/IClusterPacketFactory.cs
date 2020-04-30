namespace Rhisis.World.ClusterClient.Packets
{
    /// <summary>
    /// Provides methods to send packets to the cluster server.
    /// </summary>
    public interface IClusterPacketFactory
    {
        /// <summary>
        /// Sends an authentication request to the cluster server.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <param name="worldConfiguration">World server configuration.</param>
        void SendAuthentication(IWorldClusterClient client);
    }
}
