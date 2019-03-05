using McMaster.Extensions.CommandLineUtils;
using Rhisis.CLI.Helpers;
using Rhisis.Core.Common;
using Rhisis.Core.Extensions;
using Rhisis.Database;
using Rhisis.Database.Entities;
using System;

namespace Rhisis.CLI.Commands.User
{
    [Command("create", Description = "Created a new user")]
    public sealed class UserCreateCommand : IDisposable
    {
        private IDatabase _database;

        [Option(CommandOptionType.SingleValue, ShortName = "c", LongName = "configuration", Description = "Specify the database configuration file path.")]
        public string DatabaseConfigurationFile { get; set; }

        public void OnExecute(CommandLineApplication app, IConsole console)
        {
            if (string.IsNullOrEmpty(DatabaseConfigurationFile))
                DatabaseConfigurationFile = Application.DefaultDatabaseConfigurationFile;

            var user = new DbUser();

            Console.Write("Username: ");
            user.Username = Console.ReadLine();

            Console.Write("Email: ");
            user.Email = Console.ReadLine();

            Console.Write("Password: ");
            user.Password = ConsoleHelper.ReadPassword();

            Console.Write("Password confirmation: ");
            string passwordConfirmation = ConsoleHelper.ReadPassword();

            Console.WriteLine("Authority: ");
            ConsoleHelper.DisplayEnum<AuthorityType>();
            user.Authority = (int)ConsoleHelper.ReadEnum<AuthorityType>();

            Console.WriteLine("--------------------------------");
            Console.WriteLine("User account informations:");
            Console.WriteLine($"Username: {user.Username}");
            Console.WriteLine($"Email: {user.Email}");
            Console.WriteLine($"Authority: {(AuthorityType)user.Authority}");
            Console.WriteLine("--------------------------------");

            bool response = ConsoleHelper.AskConfirmation("Create user?");

            if (response)
            {
                DatabaseFactory.Instance.Initialize(this.DatabaseConfigurationFile);
                this._database = new Rhisis.Database.Database();

                if (this._database.Users.HasAny(x => x.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine($"User '{user.Username}' is already used.");
                    return;
                }

                if (!user.Email.IsValidEmail())
                {
                    Console.WriteLine($"Email '{user.Email}' is not valid.");
                    return;
                }

                if (_database.Users.HasAny(x => x.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine($"Email '{user.Email}' is already used.");
                    return;
                }

                if (!user.Password.Equals(passwordConfirmation))
                {
                    Console.WriteLine("Passwords doesn't match.");
                    return;
                }

                this._database.Users.Create(user);
                this._database.Complete();
                Console.WriteLine($"User '{user.Username}' created.");
            }
        }

        public void Dispose() => this._database?.Dispose();
    }
}
