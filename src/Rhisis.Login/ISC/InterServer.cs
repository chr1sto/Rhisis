using Ether.Network;
using Rhisis.Core.IO;
using Rhisis.Core.Structures.Configuration;

namespace Rhisis.Login.ISC
{
    public sealed class InterServer : NetServer<InterClient>
    {
        public InterServer(InterServerConfiguration configuration)
        {
            this.Configuration.Host = configuration.Host;
            this.Configuration.Port = configuration.Port;
            this.Configuration.MaximumNumberOfConnections = 100;
        }

        protected override void Initialize()
        {
            // Nothing to do yet.
        }

        protected override void OnClientConnected(InterClient connection)
        {
            Logger.Info("A new server is connected to the InterServer.");
        }

        protected override void OnClientDisconnected(InterClient connection)
        {
            Logger.Info("Server {0} disconnected from InterServer.", connection);
        }
    }
}
