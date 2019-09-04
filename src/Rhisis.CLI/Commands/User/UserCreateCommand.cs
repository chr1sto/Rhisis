using System;
using McMaster.Extensions.CommandLineUtils;
using Rhisis.CLI.Services;
using Rhisis.Core.Common;
using Rhisis.Core.Cryptography;
using Rhisis.Core.Extensions;
using Rhisis.Core.Helpers;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Database;
using Rhisis.Database.Entities;

namespace Rhisis.CLI.Commands.User
{
    [Command("create", Description = "Created a new user")]
    public sealed class UserCreateCommand
    {
        private readonly DatabaseFactory _databaseFactory;
        private readonly ConsoleHelper _consoleHelper;

        [Option(CommandOptionType.SingleValue, ShortName = "c", LongName = "configuration", Description = "Specify the database configuration file path.")]
        public string DatabaseConfigurationFile { get; set; }

        public UserCreateCommand(DatabaseFactory databaseFactory, ConsoleHelper consoleHelper)
        {
            this._databaseFactory = databaseFactory;
            this._consoleHelper = consoleHelper;
        }

        public void OnExecute()
        {
            if (string.IsNullOrEmpty(this.DatabaseConfigurationFile))
                this.DatabaseConfigurationFile = ConfigurationConstants.DatabasePath;

            var dbConfig = ConfigurationHelper.Load<DatabaseConfiguration>(this.DatabaseConfigurationFile);
            var user = new DbUser();

            Console.Write("Username: ");
            user.Username = Console.ReadLine();

            Console.Write("Email: ");
            user.Email = Console.ReadLine();

            Console.Write("Password: ");
            user.Password = _consoleHelper.ReadPassword();

            Console.Write("Password confirmation: ");
            string passwordConfirmation = _consoleHelper.ReadPassword();

            Console.Write("Password salt: ");
            string passwordSalt = _consoleHelper.ReadStringOrDefault();

            Console.WriteLine("Authority: ");
            _consoleHelper.DisplayEnum<AuthorityType>();
            user.Authority = (int)_consoleHelper.ReadEnum<AuthorityType>();

            Console.WriteLine("--------------------------------");
            Console.WriteLine("User account information:");
            Console.WriteLine($"Username: {user.Username}");
            Console.WriteLine($"Email: {user.Email}");
            Console.WriteLine($"Authority: {(AuthorityType)user.Authority}");
            Console.WriteLine("--------------------------------");

            bool response = _consoleHelper.AskConfirmation("Create user?");

            if (response)
            {
                using (IDatabase database = this._databaseFactory.GetDatabase(dbConfig))
                {
                    if (database.Users.HasAny(x => x.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase)))
                    {
                        Console.WriteLine($"User '{user.Username}' is already used.");
                        return;
                    }

                    if (!user.Email.IsValidEmail())
                    {
                        Console.WriteLine($"Email '{user.Email}' is not valid.");
                        return;
                    }

                    if (database.Users.HasAny(x => x.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
                    {
                        Console.WriteLine($"Email '{user.Email}' is already used.");
                        return;
                    }

                    if (!user.Password.Equals(passwordConfirmation))
                    {
                        Console.WriteLine("Passwords doesn't match.");
                        return;
                    }

                    user.Password = MD5.GetMD5Hash(passwordSalt, user.Password);

                    database.Users.Create(user);
                    database.Complete();
                    Console.WriteLine($"User '{user.Username}' created.");
                }
            }
        }
    }
}
