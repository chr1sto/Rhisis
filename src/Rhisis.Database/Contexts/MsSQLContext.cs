using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Rhisis.Database.Contexts
{
    internal class MsSQLContext : DatabaseContext
    {
        private static readonly string MsSQLConnectionString = "Server={0};Database={1};User Id={2};Password={3};";

        public MsSQLContext(DatabaseConfiguration configuration) 
            : base(configuration)
        {
        }

        public override bool CreateDatabase()
        {
            return this.Database.EnsureCreated();
        }

        public override bool DatabaseExists()
        {
            return (this.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists();
        }
        
        public override void Migrate()
        {
            this.Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = string.Format(MsSQLConnectionString,
               this.Configuration.Host,
               this.Configuration.Database,
               this.Configuration.Username,
               this.Configuration.Password);

            optionsBuilder.UseSqlServer(connectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
