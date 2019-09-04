using McMaster.Extensions.CommandLineUtils;
using Rhisis.CLI.Core;
using Rhisis.CLI.Services;
using Rhisis.Core.Helpers;
using Rhisis.Core.Structures.Configuration;
using System;
using System.Collections.Generic;

namespace Rhisis.CLI.Commands.Configure
{
    [Command("cluster", Description = "Configures the Cluster Server")]
    public class ClusterServerConfigurationCommand
    {
        private readonly ConsoleHelper _consoleHelper;

        /// <summary>
        /// Gets or sets the cluster configuration file to use.
        /// </summary>
        [Option(CommandOptionType.SingleValue, ShortName = "c", LongName = "configuration", Description = "Specify the cluster server configuration file path.")]
        public string ClusterConfigurationFile { get; set; }

        /// <summary>
        /// Gets the real configuration file.
        /// </summary>
        private string ConfigurationFile => !string.IsNullOrEmpty(this.ClusterConfigurationFile) ? this.ClusterConfigurationFile : ConfigurationConstants.ClusterServerPath;

        /// <summary>
        /// Creates a new <see cref="ClusterServerConfigurationCommand"/> instance.
        /// </summary>
        /// <param name="consoleHelper">Console helpers.</param>
        public ClusterServerConfigurationCommand(ConsoleHelper consoleHelper)
        {
            this._consoleHelper = consoleHelper;
        }

        /// <summary>
        /// Executes the "configure cluster" command.
        /// </summary>
        public void OnExecute()
        {
            var clusterServerConfiguration = ConfigurationHelper.Load<ClusterConfiguration>(this.ConfigurationFile, ConfigurationConstants.ClusterServer);
            var coreServerConfiguratinon = ConfigurationHelper.Load<CoreConfiguration>(this.ConfigurationFile, ConfigurationConstants.CoreServer);
            var clusterConfiguration = new ObjectConfigurationFiller<ClusterConfiguration>(clusterServerConfiguration);
            var coreConfiguration = new ObjectConfigurationFiller<CoreConfiguration>(coreServerConfiguratinon);

            Console.WriteLine("----- Cluster Server -----");
            clusterConfiguration.Fill();
            Console.WriteLine("----- Core Server -----");
            coreConfiguration.Fill();

            Console.WriteLine("##### Configuration review #####");
            clusterConfiguration.Show("Cluster Server configuration");
            coreConfiguration.Show("Core server configuration");

            bool response = this._consoleHelper.AskConfirmation("Save this configuration?");

            if (response)
            {
                var configuration = new Dictionary<string, object>
                {
                    { ConfigurationConstants.ClusterServer, clusterConfiguration.Value },
                    { ConfigurationConstants.CoreServer, coreConfiguration.Value }
                };

                ConfigurationHelper.Save(this.ConfigurationFile, configuration);
                Console.WriteLine($"Cluster Server configuration saved in {this.ConfigurationFile}!");
            }
        }
    }
}
