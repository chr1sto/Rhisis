using McMaster.Extensions.CommandLineUtils;
using Rhisis.CLI.Services;
using Rhisis.Core.Helpers;
using Rhisis.Core.Structures.Configuration;
using System;
using System.Collections.Generic;

namespace Rhisis.CLI.Commands.Configure
{
    [Command("login", Description = "Configures the database access")]
    public class LoginServerConfigurationCommand
    {
        private readonly ConsoleHelper _consoleHelper;

        /// <summary>
        /// Creates a new <see cref="LoginServerConfigurationCommand"/> instance.
        /// </summary>
        /// <param name="consoleHelper"></param>
        public LoginServerConfigurationCommand(ConsoleHelper consoleHelper)
        {
            this._consoleHelper = consoleHelper;
        }

        /// <summary>
        /// Executes the "configure login" command.
        /// </summary>
        public void OnExecute()
        {
            var loginConfiguration = new ObjectConfigurationFiller<LoginConfiguration>();
            var coreConfiguration = new ObjectConfigurationFiller<CoreConfiguration>();

            Console.WriteLine("----- Login Server -----");
            loginConfiguration.Fill();
            Console.WriteLine("----- Core Server -----");
            coreConfiguration.Fill();

            Console.WriteLine("##### Configuration review #####");
            loginConfiguration.Show("Login Server configuration");
            coreConfiguration.Show("Core server configuration");

            bool response = this._consoleHelper.AskConfirmation("Save this configuration?");

            if (response)
            {
                var loginServerConfiguration = new Dictionary<string, object>
                {
                    { ConfigurationConstants.LoginServer, loginConfiguration.Value },
                    { ConfigurationConstants.CoreServer, coreConfiguration.Value }
                };

                ConfigurationHelper.Save(ConfigurationConstants.LoginServerPath, loginServerConfiguration);
                Console.WriteLine($"Login Server configuration saved in {ConfigurationConstants.LoginServerPath}!");
            }
        }
    }
}
