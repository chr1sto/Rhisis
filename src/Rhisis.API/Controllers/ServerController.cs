using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Rhisis.Core.Models;
using Rhisis.Core.Services;
using Rhisis.Core.Structures.Configuration;
using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Rhisis.API.Controllers
{
    /// <summary>
    /// Provides API routes to manage the server.
    /// </summary>
    [Route("api/[controller]")]
    public class ServerController : ControllerBase
    {
        private readonly ILogger<ServerController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IStatisticsService _statisticsService;

        /// <summary>
        /// Creates a new <see cref="ServerController"/> instance.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="configuration">Configuration</param>
        /// <param name="statisticsService">Statistics service</param>
        public ServerController(ILogger<ServerController> logger, IConfiguration configuration, IStatisticsService statisticsService)
        {
            this._logger = logger;
            this._configuration = configuration;
            this._statisticsService = statisticsService;
        }

        /// <summary>
        /// Gets the world sever's status.
        /// </summary>
        /// <returns></returns>
        [HttpGet("world/status")]
        public IActionResult GetWorldServerStatus()
        {
            bool worldServerConnected = false;

            try
            {
                var worldServerConfguration = this._configuration.GetSection("WorldServer").Get<BaseConfiguration>();

                using (var client = new TcpClient(worldServerConfguration.Host, worldServerConfguration.Port))
                    worldServerConnected = true;
            }
            catch (Exception e)
            {
                this._logger.LogError(e, "An error occured while reaching for world server.");
                worldServerConnected = false;
            }

            return Ok(worldServerConnected);
        }

        [HttpGet("stats")]
        public IActionResult GetServerStatistics()
        {
            var serverStats = new ServerStatisticsModel
            {
                NumberOfUsers = this._statisticsService.GetRegisteredUsers(),
                NumberOfCharacters = this._statisticsService.GetNumberOfCharacters(),
                NumberOfPlayersOnline = 0 // TODO: make request to server
            };

            return Ok(serverStats);
        }
    }
}
