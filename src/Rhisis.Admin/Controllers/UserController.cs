using Microsoft.AspNetCore.Mvc;
using Rhisis.Core.Services;

namespace Rhisis.Admin.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public UserController(IAuthenticationService authenticationService)
        {
            // INFO: automatic dependency injection by ASP.NET Core's container.
            this._authenticationService = authenticationService;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(new[] { "user1", "user2" });
        }
    }
}
