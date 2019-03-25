using McMaster.Extensions.CommandLineUtils;
using Rhisis.Database;
using Rhisis.Database.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Rhisis.CLI.Commands.User
{
    [Command("show", Description = "Show an user.")]
    public sealed class UserShowCommand : IDisposable
    {
        private IDatabase _database;

        [Required]
        [Argument(0)]
        public string Username { get; set; }

        [Option(CommandOptionType.SingleValue, ShortName = "c", LongName = "configuration", Description = "Specify the database configuration file path.")]
        public string DatabaseConfigurationFile { get; set; }

        public void OnExecute(CommandLineApplication app, IConsole console)
        {
            if (string.IsNullOrEmpty(DatabaseConfigurationFile))
                this.DatabaseConfigurationFile = Application.DefaultDatabaseConfigurationFile;

            DatabaseFactory.Instance.Initialize(this.DatabaseConfigurationFile);
            this._database = new Rhisis.Database.Database();

            DbUser user = this._database.Users.Get(x => x.Username.Equals(this.Username, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                Console.WriteLine($"Cannot find user with username: '{this.Username}'.");
            }
            else
            {
                Console.WriteLine("#########################");
                Console.WriteLine("#   User informations   #");
                Console.WriteLine("#########################");
                Console.WriteLine($"Username: {user.Username}");
                Console.WriteLine($"Email: {user.Email}");
                Console.WriteLine($"Authority: {user.Authority.ToString()}");
                Console.WriteLine($"Deleted: {user.IsDeleted}");
                Console.WriteLine($"Last connection: {user.LastConnectionTime.ToString("yyyy/MM/dd HH:mm:ss")}");
                Console.WriteLine($"Play time: {TimeSpan.FromSeconds(user.PlayTime).ToString(@"hh\:mm\:ss")}");
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

        public void Dispose() => this._database?.Dispose();
    }
}
