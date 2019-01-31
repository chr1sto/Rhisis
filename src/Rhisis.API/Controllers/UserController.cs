using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rhisis.Business.Models;

namespace Rhisis.API.Controllers
{
    /// <summary>
    /// Provides API routes to manage users.
    /// </summary>
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        /// <summary>
        /// Creates a new <see cref="UserController"/> instance.
        /// </summary>
        /// <param name="logger">Logger</param>
        public UserController(ILogger<UserController> logger)
        {
            this._logger = logger;
        }
        
        /// <summary>
        /// Registers an user.
        /// </summary>
        /// <param name="registerModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult RegisterUser([FromBody] UserRegisterModel registerModel)
        {
            return Ok();
        }
    }
}
