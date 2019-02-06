using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Rhisis.Core.Common;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Models;
using Rhisis.Core.Services;
using Rhisis.Database;
using Rhisis.Database.Entities;
using System;

namespace Rhisis.Core.Business.Services
{
    [Injectable]
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IDatabase _database;
        private readonly IConfiguration _configuration;
        private readonly ICryptographyService _cryptographyService;

        /// <summary>
        /// Creates a new <see cref="UserService"/> instance.
        /// </summary>
        /// <param name="logger"
        /// <param name="database"></param>
        /// <param name="configuration"></param>
        /// <param name="cryptographyService"></param>
        public UserService(ILogger<UserService> logger, IDatabase database, IConfiguration configuration, ICryptographyService cryptographyService)
        {
            this._logger = logger;
            this._database = database;
            this._configuration = configuration;
            this._cryptographyService = cryptographyService;
        }

        /// <inheritdoc />
        public bool HasUser(int id) 
            => this._database.Users.HasAny(x => x.Id == id);

        /// <inheritdoc />
        public bool HasUser(string username) 
            => this._database.Users.HasAny(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

        /// <inheritdoc />
        public void CreateUser(UserRegisterModel registerModel)
        {
            if (string.IsNullOrEmpty(registerModel.Username) || string.IsNullOrEmpty(registerModel.Password) ||
                string.IsNullOrEmpty(registerModel.PasswordConfirmation))
            {
                this._logger.LogCritical($"Cannot create user. Reason: One or more fields are null or empty.");
                throw new ArgumentNullException();
            }

            if (this.HasUser(registerModel.Username))
            {
                this._logger.LogError($"Cannot register user {registerModel.Username}. Reason: Username already taken");
                throw new InvalidOperationException("username already taken");
            }

            if (!registerModel.Password.Equals(registerModel.PasswordConfirmation))
            {
                this._logger.LogError($"Cannot register user {registerModel.Username}. Reason: Passwords doesn't match.");
                throw new InvalidOperationException("passwords doesn't match");
            }

            string passwordSalt = this._configuration.GetSection("Salt").Value;

            var user = new DbUser
            {
                Username = registerModel.Username,
                Password = this._cryptographyService.GetMD5Hash(passwordSalt + registerModel.Password),
                Authority = (int)AuthorityType.Player
            };

            this._database.Users.Create(user);
            this._database.Complete();
        }
    }
}
