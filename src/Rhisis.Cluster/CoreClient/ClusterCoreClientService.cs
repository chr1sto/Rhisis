using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Rhisis.Cluster.CoreClient
{
    public sealed class ClusterCoreClientService : IHostedService
    {
        private readonly IClusterCoreClient _clusterCoreClient;

        /// <summary>
        /// Creates a new <see cref="ClusterCoreClientService"/>
        /// </summary>
        /// <param name="clusterCoreClient"></param>
        public ClusterCoreClientService(IClusterCoreClient clusterCoreClient)
        {
            _clusterCoreClient = clusterCoreClient;
        }

        /// <summary>
        /// Starts the <see cref="ClusterCoreClient"/>.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _clusterCoreClient.Connect();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Stops the <see cref="ClusterCoreClient"/>.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _clusterCoreClient.Disconnect();

            return Task.CompletedTask;
        }
    }
}
