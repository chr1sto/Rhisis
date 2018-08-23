using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Rhisis.Core.Helpers;
using Rhisis.Database.Entities;
using Rhisis.Database.Interfaces;
using Rhisis.Database.Repositories;
using System;
using System.IO;

namespace Rhisis.Database
{
    public class OldDatabaseContext : DbContext
    {
        private const string MySqlConnectionString = "server={0};userid={1};pwd={2};port={4};database={3};sslmode=none;";
        private const string MsSqlConnectionString = "Server={0},{1};Database={2};User Id={3};Password={4};";
        private static readonly string DefaultConfigFile = Path.Combine(Environment.CurrentDirectory, "..", "..", "bin", "config", "database.json");
        private readonly DatabaseConfiguration _configuration;

        /// <summary>
        /// Gets the <see cref="DbUser"/> repository.
        /// </summary>
        public IRepository<DbUser> Users { get; }

        /// <summary>
        /// Gets the <see cref="DbCharacter"/> repository.
        /// </summary>
        public IRepository<DbCharacter> Characters { get; }

        /// <summary>
        /// Gets the <see cref="DbItem"/> repository.
        /// </summary>
        public IRepository<DbItem> Items { get; }

        internal DbSet<DbUser> UsersDbSet { get; set; }

        internal DbSet<DbCharacter> CharactersDbSet { get; set; }

        internal DbSet<DbItem> ItemsDbSet { get; set; }

        /// <summary>
        /// Creates a new <see cref="OldDatabaseContext"/> instance.
        /// </summary>
        public OldDatabaseContext()
            : this(ConfigurationHelper.Load<DatabaseConfiguration>(DefaultConfigFile))
        {
        }

        /// <summary>
        /// Creates a new <see cref="OldDatabaseContext"/> instance.
        /// </summary>
        /// <param name="configuration"></param>
        internal OldDatabaseContext(DatabaseConfiguration configuration)
        {
            this.Users = new UserRepository(this);
            this.Characters = new CharacterRepository(this);
            this.Items = new ItemRepository(this);

            this._configuration = configuration;
        }

        /// <summary>
        /// Called when the configuration of the DbContext has begun.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            switch (this._configuration.Provider)
            {
                case DatabaseProvider.MySql:
                    optionsBuilder.UseMySql(this.GetConnectionString(this._configuration, MySqlConnectionString));
                    break;
                case DatabaseProvider.MsSql:
                    optionsBuilder.UseSqlServer(this.GetConnectionString(this._configuration, MsSqlConnectionString));
                    break;
                default:
                    throw new NotImplementedException();
            }
            
            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// Opens the connection.
        /// </summary>
        public virtual void OpenConnection() => this.Database.OpenConnection();

        /// <summary>
        /// Closes the connection.
        /// </summary>
        public virtual void CloseConnection() => this.Database.CloseConnection();

        /// <summary>
        /// Creates the database.
        /// </summary>
        /// <returns></returns>
        public virtual bool CreateDatabase() => this.Database.EnsureCreated();

        /// <summary>
        /// Check if the database exists.
        /// </summary>
        /// <returns></returns>
        public virtual bool DatabaseExists() => ((RelationalDatabaseCreator)this.GetService<IDatabaseCreator>()).Exists();

        /// <summary>
        /// Processes the database migration.
        /// </summary>
        public virtual void Migrate() => this.Database.Migrate();

        /// <summary>
        /// Format and gets a connection string.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        private string GetConnectionString(DatabaseConfiguration configuration, string connectionString)
        {
            switch (this._configuration.Provider)
            {
                case DatabaseProvider.MySql:
                    return string.Format(connectionString,
                        configuration.Host,
                        configuration.Username,
                        configuration.Password,
                        configuration.Database,
                        configuration.Port);
                case DatabaseProvider.MsSql:
                    return string.Format(connectionString,
                        configuration.Host,
                        configuration.Port,
                        configuration.Database,
                        configuration.Username,
                        configuration.Password);

                default: return null;
            }
        }
    }
}
