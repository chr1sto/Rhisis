using McMaster.Extensions.CommandLineUtils;
using Rhisis.CLI.Core;
using Rhisis.CLI.Services;
using Rhisis.Core.Cryptography;
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
        private string ConfigurationFile => !string.IsNullOrEmpty(ClusterConfigurationFile) ? ClusterConfigurationFile : ConfigurationConstants.ClusterServerPath;

        /// <summary>
        /// Executes the "configure cluster" command.
        /// </summary>
        public void OnExecute()
        {
            var clusterServerConfiguration = ConfigurationHelper.Load<ClusterConfiguration>(ConfigurationFile, ConfigurationConstants.ClusterServer);
            var coreServerConfiguration = ConfigurationHelper.Load<CoreConfiguration>(ConfigurationFile, ConfigurationConstants.CoreServer);
            var worldClusterServerConfiguration = ConfigurationHelper.Load<WorldClusterConfiguration>(ConfigurationFile, ConfigurationConstants.WorldClusterServer);
            var clusterConfiguration = new ObjectConfigurationFiller<ClusterConfiguration>(clusterServerConfiguration);
            var coreConfiguration = new ObjectConfigurationFiller<CoreConfiguration>(coreServerConfiguration);
            var worldClusterConfiguration = new ObjectConfigurationFiller<WorldClusterConfiguration>(worldClusterServerConfiguration);

            Console.WriteLine("----- Core Server -----");
            coreConfiguration.Fill();
            coreConfiguration.Value.Password = MD5.GetMD5Hash(coreConfiguration.Value.Password);
            
            Console.WriteLine("----- Cluster Server -----");
            clusterConfiguration.Fill();
            worldClusterConfiguration.Fill();
            worldClusterConfiguration.Value.Password = MD5.GetMD5Hash(worldClusterConfiguration.Value.Password);

            Console.WriteLine("##### Configuration review #####");
            coreConfiguration.Show("Core server configuration");
            clusterConfiguration.Show("Cluster Server configuration");
            worldClusterConfiguration.Show("World cluster server configuration");

            bool response = _consoleHelper.AskConfirmation("Save this configuration?");

            if (response)
            {
                var configuration = new Dictionary<string, object>
                {
                    { ConfigurationConstants.ClusterServer, clusterConfiguration.Value },
                    { ConfigurationConstants.WorldClusterServer, worldClusterConfiguration.Value },
                    { ConfigurationConstants.CoreServer, coreConfiguration.Value }
                };

                ConfigurationHelper.Save(ConfigurationFile, configuration);
                Console.WriteLine($"Cluster Server configuration saved in {ConfigurationFile}!");
            }
        }

        /// <summary>
        /// Creates a new <see cref="ClusterServerConfigurationCommand"/> instance.
        /// </summary>
        /// <param name="consoleHelper">Console helpers.</param>
        public ClusterServerConfigurationCommand(ConsoleHelper consoleHelper)
        {
            _consoleHelper = consoleHelper;
        }
    }
}
