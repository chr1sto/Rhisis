using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Services;
using Rhisis.Database;
using System;

namespace Rhisis.Core.Business.Services
{
    [Injectable]
    public class StatisticsService : IStatisticsService
    {
        private readonly IDatabase _database;
        
        /// <summary>
        /// Creates a new <see cref="StatisticsService"/> instance.
        /// </summary>
        /// <param name="database"></param>
        public StatisticsService(IDatabase database)
        {
            this._database = database;
        }

        /// <inheritdoc />
        public int GetNumberOfCharacters() => this._database.Characters.Count();

        /// <inheritdoc />
        public int GetOnlinePlayers()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public int GetRegisteredUsers() => this._database.Users.Count();
    }
}
