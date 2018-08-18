using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.Core.Services
{
    public interface IAuthenticationService
    {
        bool Authenticate(string username, string password);
    }
}
