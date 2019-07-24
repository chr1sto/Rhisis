using Rhisis.Database.Entities;

namespace Rhisis.Database.Repositories
{
    public interface IUserRepository : IRepository<DbUser>
    {
        /// <summary>
        /// Gets a user by it's username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        DbUser GetUserByUsername(string username);
    }
}
