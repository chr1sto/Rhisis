using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rhisis.Database;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rhisis.Login
{
    /// <summary>
    /// Service that manages the Login Server instance.
    /// </summary>
    internal sealed class LoginServerService : IHostedService
    {
        private readonly ILogger<LoginServerService> _logger;
        private readonly ILoginServer _loginServer;

        /// <summary>
        /// Creates a new <see cref="LoginServerService"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="loginServer">Login Server instance.</param>
        public LoginServerService(ILogger<LoginServerService> logger, ILoginServer loginServer)
        {
            _logger = logger;
            _loginServer = loginServer;
        }

        /// <summary>
        /// Starts the login server service.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting LoginServer");
            _loginServer.Start();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Stops the login server service.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _loginServer.Stop();

            return Task.CompletedTask;
        }
    }
}