using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rhisis.Cluster.WorldCluster.Server;

namespace Rhisis.Cluster.WorldCluster
{
    internal sealed class WorldClusterServerService: IHostedService
    {
        private readonly ILogger<WorldClusterServerService> _logger;
        private readonly IWorldClusterServer _worldClusterServer;

        /// <summary>
        /// Creates a new <see cref="WorldClusterServerService"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="worldClusterServer">Core server.</param>
        public WorldClusterServerService(ILogger<WorldClusterServerService> logger, IWorldClusterServer worldClusterServer)
        {
            _logger = logger;
            _worldClusterServer = worldClusterServer;
        }

        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting {nameof(WorldClusterServer)}.");
            _worldClusterServer.Start();

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Stopping {nameof(WorldClusterServer)}.");
            _worldClusterServer.Stop();

            return Task.CompletedTask;
        }
    }
}