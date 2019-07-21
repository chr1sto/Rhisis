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
            this._logger = logger;
            this._coreServer = coreServer;
        }

        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellationToken)
        {
            this._logger.LogInformation($"Starting {nameof(CoreServer)}.");
            this._coreServer.Start();

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task StopAsync(CancellationToken cancellationToken)
        {
            this._logger.LogInformation($"Stopping {nameof(CoreServer)}.");
            this._coreServer.Stop();

            return Task.CompletedTask;
        }
    }
}
