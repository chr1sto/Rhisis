using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rhisis.Database.Context;

namespace Rhisis.Database
{
    public static class ConfigureDatabase
    {
        private const string MySqlConnectionString =
            "server={0};userid={1};pwd={2};port={4};database={3};sslmode=none;";

        public static DbContextOptionsBuilder ConfigureCorrectDatabase(this DbContextOptionsBuilder optionsBuilder,
            DatabaseConfiguration configuration)
        {
            optionsBuilder.UseMySql(string.Format(MySqlConnectionString,
                            configuration.Host,
                            configuration.Username,
                            configuration.Password,
                            configuration.Database,
                            configuration.Port));

            return optionsBuilder;
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
                .AddDbContext<DatabaseContext>(options => options.ConfigureCorrectDatabase(configuration))
                .AddTransient<IDatabase, Database>();
        }
    }
}