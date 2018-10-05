using Rhisis.Core.Common;

namespace Rhisis.Core.Services
{
    public interface IAuthenticationService
    {
        AuthenticationResult Authenticate(string username, string password);
    }
}
