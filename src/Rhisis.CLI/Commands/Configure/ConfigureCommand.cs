using McMaster.Extensions.CommandLineUtils;
using Rhisis.CLI.Services;
using Rhisis.Core.Helpers;
using Rhisis.Core.Structures.Configuration;
using System;
using System.Collections.Generic;

namespace Rhisis.CLI.Commands.Configure
{
    [Command("configure", Description = "Configures a Rhisis login server.")]
    [Subcommand(typeof(LoginServerConfigurationCommand))]
    public class ConfigureCommand
    {
        public void OnExecute(CommandLineApplication app) => app.ShowHelp();
    }

    [Command("login", Description = "Configures the database access")]
    public class LoginServerConfigurationCommand
    {
        private const string LoginServerConfigurationPath = "config/login.json";
        private readonly ConsoleHelper _consoleHelper;

        public LoginServerConfigurationCommand(ConsoleHelper consoleHelper)
        {
            _consoleHelper = consoleHelper;
        }

        public void OnExecute()
        {
            var loginConfiguration = new LoginConfiguration();
            var iscConfiguration = new ISCConfiguration();

            Console.WriteLine("### Configuring Login Server ###");

            Console.Write("Host (127.0.0.1): ");
            loginConfiguration.Host = _consoleHelper.ReadStringOrDefault("127.0.0.1");

            Console.Write("Port (23000): ");
            loginConfiguration.Port = _consoleHelper.ReadIntegerOrDefault(23000);

            Console.Write("Client Build Version (20100412): ");
            loginConfiguration.BuildVersion = _consoleHelper.ReadStringOrDefault("20100412");

            loginConfiguration.AccountVerification = _consoleHelper.AskConfirmation("Use account verification?");
            loginConfiguration.PasswordEncryption = _consoleHelper.AskConfirmation("Use password encryption?");

            if (loginConfiguration.PasswordEncryption)
            {
                Console.Write("Encryption key: (dldhsvmflvm): ");
                loginConfiguration.EncryptionKey = _consoleHelper.ReadStringOrDefault("dldhsvmflvm");
            }

            Console.Write("ISC Host (127.0.0.1): ");
            iscConfiguration.Host = _consoleHelper.ReadStringOrDefault("127.0.0.1");

            Console.Write("ISC Port (15000): ");
            iscConfiguration.Port = _consoleHelper.ReadIntegerOrDefault(15000);

            Console.Write("ISC Password: ");
            iscConfiguration.Password = _consoleHelper.ReadPassword();

            Console.WriteLine("--------------------------------");
            Console.WriteLine("Login Server Configuration:");
            Console.WriteLine($"Host: {loginConfiguration.Host}");
            Console.WriteLine($"Port: {loginConfiguration.Port}");
            Console.WriteLine($"Client build version: {loginConfiguration.BuildVersion}");
            Console.WriteLine($"Use account verification: {loginConfiguration.AccountVerification}");
            Console.WriteLine($"Use password encryption: {loginConfiguration.PasswordEncryption}");
            Console.WriteLine($"ISC Host: {loginConfiguration.ISC.Host}");
            Console.WriteLine($"ISC Port: {loginConfiguration.ISC.Port}");

            bool response = _consoleHelper.AskConfirmation("Save this configuration?");

            if (response)
            {
                var loginServerConfiguration = new Dictionary<string, object>
                {
                    { "loginServer", loginConfiguration },
                    { "isc", iscConfiguration }
                };

                ConfigurationHelper.Save(LoginServerConfigurationPath, loginServerConfiguration);
                Console.WriteLine($"Login Server configuration saved in {LoginServerConfigurationPath}!");
            }
        }
    }
}
