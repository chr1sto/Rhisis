using Rhisis.Core.Models;

namespace Rhisis.Core.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Check if the user with the given Id exists in the database.
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>True if the user exists, false otherwise</returns>
        bool HasUser(int id);

        /// <summary>
        /// Check if the user with the given username exists in the database.
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>True if the user exists, false otherwise</returns>
        bool HasUser(string username);

        /// <summary>
        /// Check if the email address exists in the database.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        bool HasUserWithEmail(string email);

        /// <summary>
        /// Creates a new user in the database.
        /// </summary>
        /// <param name="registerModel">Registrer model</param>
        void CreateUser(UserRegisterModel registerModel);
    }
}
