using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rhisis.Database.Context;

namespace Rhisis.Database
{
    public static class ConfigureDatabase
    {
        private const string MySqlConnectionString = "server={0};userid={1};pwd={2};port={4};database={3};sslmode=none;";
        private const string MsSqlConnectionString = "Server={0};Database={1};User Id={2};Password={3};";

        public static DbContextOptionsBuilder ConfigureCorrectDatabase(this DbContextOptionsBuilder optionsBuilder, 
            DatabaseConfiguration configuration)
        {
            
            switch (configuration.Provider)
            {
                case DatabaseProvider.MySql:
                    optionsBuilder.UseMySql(
                        string.Format(MySqlConnectionString,
                            configuration.Host,
                            configuration.Username,
                            configuration.Password,
                            configuration.Database,
                            configuration.Port));
                    break;

                case DatabaseProvider.MsSql:
                    optionsBuilder.UseSqlServer(
                        string.Format(MsSqlConnectionString,
                            configuration.Host,
                            configuration.Database,
                            configuration.Username,
                            configuration.Password));
                    break;

                default: throw new NotImplementedException($"Provider {configuration.Provider} not implemented yet.");
            }

            return optionsBuilder;
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