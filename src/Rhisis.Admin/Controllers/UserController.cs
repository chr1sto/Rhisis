using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rhisis.Admin.Options;
using Rhisis.Core.Services;
using Rhisis.Database;

namespace Rhisis.Admin.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IAuthenticationService _authenticationService;
        private readonly IWritableOptions<DatabaseConfiguration> _databaseConfiguration;

        public UserController(
            ILogger<UserController> logger,
            IAuthenticationService authenticationService, 
            IWritableOptions<DatabaseConfiguration> dbOptions)
        {
            // INFO: automatic dependency injection by ASP.NET Core's container.
            this._logger = logger;
            this._authenticationService = authenticationService;
            this._databaseConfiguration = dbOptions;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            this._logger.LogDebug("Requesting users");
            return Ok(new[] { "user1", "user2" });
        }
    }
}
