using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Rhisis.Database.Contexts
{
    internal class MySQLContext : DatabaseContext
    {
        private static readonly string MySQLConnectionString = "server={0};userid={1};pwd={2};port=3306;database={3};sslmode=none;";

        public MySQLContext(DatabaseConfiguration configuration) 
            : base(configuration)
        {
        }

        public override void Migrate()
        {
            throw new NotImplementedException();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = string.Format(MySQLConnectionString,
                this.Configuration.Host,
                this.Configuration.Username,
                this.Configuration.Password,
                this.Configuration.Database);

            optionsBuilder.UseMySql(connectionString);
        }
    }
}
