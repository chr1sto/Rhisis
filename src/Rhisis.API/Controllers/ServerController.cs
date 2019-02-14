using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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

        /// <summary>
        /// Creates a new <see cref="ServerController"/> instance.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="configuration">Configuration</param>
        public ServerController(ILogger<ServerController> logger, IConfiguration configuration)
        {
            this._logger = logger;
            this._configuration = configuration;
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
    }
}
