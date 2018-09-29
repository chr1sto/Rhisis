using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Services;
using Rhisis.Database;

namespace Rhisis.Core.Business.Services
{
    [Injectable]
    public class UserService : IUserService
    {
        private readonly IDatabase _database;

        public UserService(IDatabase database)
        {
            this._database = database;
        }
    }
}
