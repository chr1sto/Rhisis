using Rhisis.Core.Common;

namespace Rhisis.Core.Services
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Authenticate an user with username and password credentials.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        AuthenticationResult Authenticate(string username, string password);
    }
}
