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
        DbUser GetUser(string username);

        /// <summary>
        /// Gets a user by it's username and password.
        /// </summary>
        /// <param name="username">User name.</param>
        /// <param name="password">User password.</param>
        /// <returns></returns>
        DbUser GetUser(string username, string password);
    }
}
