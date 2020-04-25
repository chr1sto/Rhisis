using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.EntityFrameworkCore;
using Rhisis.Core.Helpers;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Database;
using Rhisis.Database.Entities;

namespace Rhisis.CLI.Commands.User
{
    [Command("show", Description = "Show an user.")]
    public sealed class UserShowCommand
    {
        private readonly DatabaseFactory _databaseFactory;

        [Required]
        [Argument(0)]
        public string Username { get; set; }

        [Option(CommandOptionType.SingleValue, ShortName = "c", LongName = "configuration", Description = "Specify the database configuration file path.")]
        public string DatabaseConfigurationFile { get; set; }

        public UserShowCommand(DatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public void OnExecute()
        {
            if (string.IsNullOrEmpty(DatabaseConfigurationFile))
                DatabaseConfigurationFile = ConfigurationConstants.DatabasePath;

            var dbConfig = ConfigurationHelper.Load<DatabaseConfiguration>(DatabaseConfigurationFile, ConfigurationConstants.DatabaseConfiguration);
            if (dbConfig is null)
            {
                Console.WriteLine("Couldn't load database configuration file during execution of user show command.");
                return;
            }

            using IRhisisDatabase database = _databaseFactory.CreateDatabaseInstance(dbConfig);
            
            DbUser user = database.Users.Include(x => x.Characters).FirstOrDefault(x => x.Username.Equals(Username, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                Console.WriteLine($"Cannot find user with username: '{Username}'.");
            }
            else
            {
                Console.WriteLine("#########################");
                Console.WriteLine("#   User information   #");
                Console.WriteLine("#########################");
                Console.WriteLine($"Username: {user.Username}");
                Console.WriteLine($"Email: {user.Email}");
                Console.WriteLine($"Authority: {user.Authority.ToString()}");
                Console.WriteLine($"Deleted: {user.IsDeleted}");
                Console.WriteLine($"Last connection: {user.LastConnectionTime:yyyy/MM/dd HH:mm:ss}");
                Console.WriteLine($"Play time: {TimeSpan.FromSeconds(user.PlayTime):hh\\:mm\\:ss}");
                Console.WriteLine($"Number of characters: {user.Characters.Count}");

                if (user.Characters.Any())
                {
                    for (int i = 0; i < user.Characters.Count; i++)
                    {
                        DbCharacter character = user.Characters.ElementAt(i);

                        Console.WriteLine("-------------------------");
                        Console.WriteLine($"Character name: {character.Name} (id: {character.Id})");
                        Console.WriteLine($"Deleted: {character.IsDeleted}");
                    }
                }
            }
        }
    }
}
