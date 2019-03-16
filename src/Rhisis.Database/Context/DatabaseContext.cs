using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DataEncryption;
using Microsoft.EntityFrameworkCore.DataEncryption.Providers;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Rhisis.Core.Exceptions;
using Rhisis.Database.Entities;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Rhisis.Database.Context
{
    public sealed class DatabaseContext : DbContext
    {
        private const string MigrationConfigurationEnv = "DB_CONFIG";
        private const string MySqlConnectionString = "server={0};userid={1};pwd={2};port={4};database={3};sslmode=none;";
        private const string MsSqlConnectionString = "Server={0};Database={1};User Id={2};Password={3};";

        private readonly DatabaseConfiguration _configuration;
        private readonly IEncryptionProvider _encryptionProvider;

        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        public DbSet<DbUser> Users { get; set; }

        /// <summary>
        /// Gets or sets the characters.
        /// </summary>
        public DbSet<DbCharacter> Characters { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public DbSet<DbItem> Items { get; set; }

        /// <summary>
        /// Gets or sets the mails.
        /// </summary>
        public DbSet<DbMail> Mails { get; set; }

        /// <summary>
        /// Gets or sets the taskbar shortcuts.
        /// </summary>
        public DbSet<DbShortcut> TaskbarShortcuts { get; set; }

        /// <summary>
        /// Create a new <see cref="DatabaseContext"/> instance.
        /// </summary>
        /// <param name="options"></param>
        public DatabaseContext(DbContextOptions options)
            : base(options)
        {
        }

        /// <summary>
        /// Creates a new <see cref="DatabaseContext"/> instance.
        /// </summary>
        /// <param name="configuration"></param>
        public DatabaseContext(DatabaseConfiguration configuration)
        {
            if (string.IsNullOrEmpty(configuration.EncryptionKey))
                throw new RhisisConfigurationException($"Database configuration doesn't contain a valid encryption key.");

            this._configuration = configuration;
            this._encryptionProvider = new AesProvider(
                Convert.FromBase64String(this._configuration.EncryptionKey), 
                Enumerable.Repeat<byte>(0, 16).ToArray(), 
                CipherMode.CBC, 
                PaddingMode.Zeros);
        }

        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;

            switch (this._configuration.Provider)
            {
                case DatabaseProvider.MySql:
                    optionsBuilder.UseMySql(
                        string.Format(MySqlConnectionString,
                            this._configuration.Host,
                            this._configuration.Username,
                            this._configuration.Password,
                            this._configuration.Database,
                            this._configuration.Port));
                    break;

                case DatabaseProvider.MsSql:
                    optionsBuilder.UseSqlServer(
                        string.Format(MsSqlConnectionString,
                            this._configuration.Host,
                            this._configuration.Database,
                            this._configuration.Username,
                            this._configuration.Password));
                    break;

                default: throw new NotImplementedException($"Provider {this._configuration.Provider} not implemented yet.");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseEncryption(this._encryptionProvider);
            modelBuilder.Entity<DbUser>()
                .HasIndex(c => new { c.Username, c.Email })
                .IsUnique();
            modelBuilder.Entity<DbCharacter>()
                .HasMany(x => x.ReceivedMails).WithOne(x => x.Receiver);
            modelBuilder.Entity<DbCharacter>()
                .HasMany(x => x.SentMails).WithOne(x => x.Sender);
        }

        /// <summary>
        /// Migrates the database schema.
        /// </summary>
        public void Migrate() => this.Database.Migrate();

        /// <summary>
        /// Migrates the database schema asynchronously.
        /// </summary>
        public async Task MigrateAsync() => await this.Database.MigrateAsync();

        /// <summary>
        /// Check if the database exists.
        /// </summary>
        /// <returns></returns>
        public bool DatabaseExists() => (this.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists();

        /// <summary>
        /// Opens the connection.
        /// </summary>
        public void OpenConnection() => this.Database.OpenConnection();

        /// <summary>
        /// Closes the connection.
        /// </summary>
        public void CloseConnection() => this.Database.CloseConnection();
    }
}
