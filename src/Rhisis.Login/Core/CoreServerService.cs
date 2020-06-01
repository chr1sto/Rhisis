using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Rhisis.Login.Core
{
    /// <summary>
    /// Describes the <see cref="CoreServer"/> service.
    /// </summary>
    internal sealed class CoreServerService : IHostedService
    {
        private readonly ILogger<CoreServerService> _logger;
        private readonly ICoreServer _coreServer;

        /// <summary>
        /// Creates a new <see cref="CoreServerService"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="coreServer">Core server.</param>
        public CoreServerService(ILogger<CoreServerService> logger, ICoreServer coreServer)
        {
            _logger = logger;
            _coreServer = coreServer;
        }

        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting {nameof(CoreServer)}.");
            _coreServer.Start();

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Stopping {nameof(CoreServer)}.");
            _coreServer.Stop();

            return Task.CompletedTask;
        }
    }
}
