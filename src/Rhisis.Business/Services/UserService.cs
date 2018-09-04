using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Services;
using Rhisis.Database.Interfaces;

namespace Rhisis.Core.Business.Services
{
    [Service]
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }
    }
}
