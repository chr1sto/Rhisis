using Microsoft.AspNetCore.Mvc;
using Rhisis.Core.Services;
using System.Threading.Tasks;

namespace Rhisis.Admin.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login()
        {
            await Task.Delay(10);

            return Ok();
        }

        [Route("register")]
        public async Task<IActionResult> Register()
        {
            await Task.Delay(10);

            return Ok();
        }
    }
}
