using System;
using System.Linq;
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
    [Command("update", Description = "Update an existing user")]
    public sealed class UserUpdateCommand
    {
        private readonly DatabaseFactory _databaseFactory;
        private readonly ConsoleHelper _consoleHelper;

        [Option(CommandOptionType.SingleValue, ShortName = "c", LongName = "configuration", Description = "Specify the database configuration file path.")]
        public string DatabaseConfigurationFile { get; set; }

        public UserUpdateCommand(DatabaseFactory databaseFactory, ConsoleHelper consoleHelper)
        {
            _databaseFactory = databaseFactory;
            _consoleHelper = consoleHelper;
        }

        public void OnExecute()
        {
            if (string.IsNullOrEmpty(DatabaseConfigurationFile))
                DatabaseConfigurationFile = ConfigurationConstants.DatabasePath;

            var dbConfig = ConfigurationHelper.Load<DatabaseConfiguration>(DatabaseConfigurationFile, ConfigurationConstants.DatabaseConfiguration);
            if (dbConfig is null)
            {
                Console.WriteLine("Couldn't load database configuration file during execution of user create command.");
                return;
            }

            Console.Write("Username to update: ");
            string username = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(username))
            {
                Console.WriteLine("You must type a username.");
                return;
            }

            using IRhisisDatabase database = _databaseFactory.CreateDatabaseInstance(dbConfig);
            DbUser user = database.Users.FirstOrDefault(x => x.Username ==  username);

            if (user == null)
            {
                Console.WriteLine($"Could not locate any username named '{username}'.");
                return;
            }

            bool response = _consoleHelper.AskConfirmation($"Are you sure you want to update the account '{user.Username}'");
            if (!response)
                return;

            bool changeEmail = _consoleHelper.AskConfirmation($"Would you like to change the email? '{user.Email}'");
            if (changeEmail)
            {
                Console.Write("Type the new email: ");
                user.Email = Console.ReadLine();
            }

            bool changePassword = _consoleHelper.AskConfirmation("Would you like to change the password?");
            string passwordConfirmation = string.Empty;
            string passwordSalt = string.Empty;
            if (changePassword)
            {
                Console.Write("New password: ");
                user.Password = _consoleHelper.ReadPassword();

                Console.Write("New password confirmation: ");
                passwordConfirmation = _consoleHelper.ReadPassword();

                Console.Write("Password salt: ");
                passwordSalt = _consoleHelper.ReadStringOrDefault();
            }

            bool changeAuthority = _consoleHelper.AskConfirmation("Would you like to change the account authority?");
            if (changeAuthority)
            {
                Console.WriteLine("New authority: ");
                _consoleHelper.DisplayEnum<AuthorityType>();
                user.Authority = (int)_consoleHelper.ReadEnum<AuthorityType>();
            }

            Console.WriteLine("--------------------------------");
            Console.WriteLine("User account information:");
            Console.WriteLine($"Username: {user.Username}");
            Console.WriteLine($"Email: {user.Email}");
            Console.WriteLine($"Authority: {(AuthorityType)user.Authority}");
            Console.WriteLine("--------------------------------");

            bool updateUser = (changeEmail || changePassword || changeAuthority) && _consoleHelper.AskConfirmation("Update user?");
            if (updateUser)
            {
                if (changeEmail)
                {
                    if (!user.Email.IsValidEmail())
                    {
                        Console.WriteLine($"Email '{user.Email}' is not valid.");
                        return;
                    }

                    if (database.Users.Any(x => x.Email.Equals(user.Email, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        Console.WriteLine($"Email '{user.Email}' is already used.");
                        return;
                    }
                }

                if (changePassword)
                {
                    if (!user.Password.Equals(passwordConfirmation))
                    {
                        Console.WriteLine("Passwords doesn't match.");
                        return;
                    }

                    user.Password = MD5.GetMD5Hash(passwordSalt, user.Password);
                }

                database.Users.Update(user);
                database.SaveChanges();
                Console.WriteLine($"User '{user.Username}' has been updated.");
            }
        }
    }
}
