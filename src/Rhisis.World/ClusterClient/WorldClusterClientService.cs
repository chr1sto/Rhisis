using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Rhisis.World.ClusterClient
{
    public sealed class WorldClusterClientService : IHostedService
    {
        private readonly IWorldClusterClient _worldClusterClient;

        /// <summary>
        /// Creates a new <see cref="WorldClusterClientService"/> instance.
        /// </summary>
        /// <param name="worldClusterClient">World core client.</param>
        public WorldClusterClientService(IWorldClusterClient worldClusterClient)
        {
            _worldClusterClient = worldClusterClient;
        }

        /// <summary>
        /// Starts the world core client service.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _worldClusterClient.Connect();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Stops the world core client service.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _worldClusterClient.Disconnect();

            return Task.CompletedTask;
        }
    }
}
