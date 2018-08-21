using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Services;

namespace Rhisis.Core.Business.Services
{
    [Service]
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserService _userService;

        public AuthenticationService(IUserService userService)
        {
            this._userService = userService;
        }

        public bool Authenticate(string username, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}
