using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using Rhisis.Database.Context;

namespace Rhisis.Database
{
    public static class ConfigureDatabase
    {
        public static DbContextOptionsBuilder ConfigureCorrectDatabase(this DbContextOptionsBuilder optionsBuilder,
            DatabaseConfiguration configuration)
        {
            var sqlConnectionStringBuilder = new MySqlConnectionStringBuilder
            {
                Server = configuration.Host,
                UserID = configuration.Username,
                Password = configuration.Password,
                Port = (uint)configuration.Port,
                Database = configuration.Database,
                SslMode = MySqlSslMode.None
            };

            return optionsBuilder.UseMySql(sqlConnectionStringBuilder.ToString());
        }

        public static IServiceCollection RegisterDatabaseFactory(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddTransient<DatabaseFactory>();
        }

        public static IServiceCollection RegisterDatabaseServices(this IServiceCollection serviceCollection,
            DatabaseConfiguration configuration)
        {
            return serviceCollection
                .AddSingleton(configuration)
                .AddDbContext<DatabaseContext>(options => options.ConfigureCorrectDatabase(configuration), ServiceLifetime.Transient)
                .AddTransient<IDatabase, Database>();
        }
    }
}