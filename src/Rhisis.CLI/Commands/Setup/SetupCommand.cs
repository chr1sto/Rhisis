using System;
using System.Collections.Generic;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Rhisis.CLI.Commands.Database;
using Rhisis.CLI.Services;
using Rhisis.Core.Helpers;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Database;

namespace Rhisis.CLI.Commands.Setup
{
    public class SetupCommand
    {
        private const string ClusterServerConfigurationPath = "config/cluster.json";
        private const string WorldServerConfigurationPath = "config/world.json";
        private readonly DatabaseInitializationCommand _databaseInitializationCommand;
        
        private readonly ConsoleHelper _consoleHelper;

        public SetupCommand(DatabaseFactory databaseFactory, ConsoleHelper consoleHelper)
        {
            _consoleHelper = consoleHelper;
            this._databaseInitializationCommand = new DatabaseInitializationCommand(databaseFactory, consoleHelper);
        }

        public void OnExecute()
        {
            Console.WriteLine("Welcome to Rhisis!");
            Console.WriteLine($"-------------------------------{Environment.NewLine}");
            Console.WriteLine($"This command will help you configure your Rhisis server instance.{Environment.NewLine}");
            
            this.ConfigureLoginServer();
            this.ConfigureClusterServer();
            this.ConfigureWorldServer();

            // Configure database access
            this._databaseInitializationCommand.OnExecute();
        }

        private void ConfigureLoginServer()
        {
            
        }

        private void ConfigureClusterServer()
        {
            var clusterConfiguration = new ClusterConfiguration();

            Console.WriteLine("### Configuring Cluster Server ###");
            Console.Write("Host (127.0.0.1): ");
            clusterConfiguration.Host = _consoleHelper.ReadStringOrDefault("127.0.0.1");

            Console.Write("Port (28000): ");
            clusterConfiguration.Port = _consoleHelper.ReadIntegerOrDefault(28000);

            Console.Write("Cluster Id: ");
            clusterConfiguration.Id = _consoleHelper.ReadIntegerOrDefault();

            Console.Write("Cluster name (Rhisis): ");
            clusterConfiguration.Name = _consoleHelper.ReadStringOrDefault("Rhisis");
            
            clusterConfiguration.EnableLoginProtect = _consoleHelper.AskConfirmation("Enable second password verification (LoginProtect)");

            Console.Write("ISC Server Host (127.0.0.1): ");
            clusterConfiguration.ISC.Host = _consoleHelper.ReadStringOrDefault("127.0.0.1");

            Console.Write("ISC Port (15000): ");
            clusterConfiguration.ISC.Port = _consoleHelper.ReadIntegerOrDefault(15000);

            Console.Write("ISC Password: ");
            clusterConfiguration.ISC.Password = _consoleHelper.ReadPassword();

            Console.WriteLine("--------------------------------");
            Console.WriteLine("Cluster Server Configuration:");
            Console.WriteLine($"Host: {clusterConfiguration.Host}");
            Console.WriteLine($"Port: {clusterConfiguration.Port}");
            Console.WriteLine($"Use login protect: {clusterConfiguration.EnableLoginProtect}");
            Console.WriteLine($"ISC Host: {clusterConfiguration.ISC.Host}");
            Console.WriteLine($"ISC Port: {clusterConfiguration.ISC.Port}");

            bool response = _consoleHelper.AskConfirmation("Save this configuration?");

            if (response)
            {
                ConfigurationHelper.Save(ClusterServerConfigurationPath, clusterConfiguration);
                Console.WriteLine($"Cluster Server configuration saved in {ClusterServerConfigurationPath}!");
            }
        }

        private void ConfigureWorldServer()
        {
            var worldConfiguration = new WorldConfiguration();

            Console.WriteLine("### Configuring World Server ###");
            Console.Write("Host (127.0.0.1): ");
            worldConfiguration.Host = _consoleHelper.ReadStringOrDefault("127.0.0.1");

            Console.Write("Port (5400): ");
            worldConfiguration.Port = _consoleHelper.ReadIntegerOrDefault(5400);

            Console.Write("Parent cluster Id: ");
            worldConfiguration.ClusterId = _consoleHelper.ReadIntegerOrDefault();

            Console.Write("World channel Id: ");
            worldConfiguration.Id = _consoleHelper.ReadIntegerOrDefault();

            Console.Write("World channel name (Channel 1): ");
            worldConfiguration.Name = _consoleHelper.ReadStringOrDefault("Channel 1");

            Console.Write("ISC Server Host (127.0.0.1): ");
            worldConfiguration.ISC.Host = _consoleHelper.ReadStringOrDefault("127.0.0.1");

            Console.Write("ISC Port (15000): ");
            worldConfiguration.ISC.Port = _consoleHelper.ReadIntegerOrDefault(15000);

            Console.Write("ISC Password: ");
            worldConfiguration.ISC.Password = _consoleHelper.ReadPassword();

            Console.WriteLine("----- Drops -----");
            Console.Write("Drop rate (1): ");
            worldConfiguration.Rates.Drop = _consoleHelper.ReadIntegerOrDefault(1);

            Console.Write("Gold drop rate (1): ");
            worldConfiguration.Rates.Gold = _consoleHelper.ReadIntegerOrDefault(1);

            Console.Write("Experience rate (1): ");
            worldConfiguration.Rates.Experience = _consoleHelper.ReadIntegerOrDefault(1);

            worldConfiguration.Maps = new List<string>
            {
                "WI_WORLD_MADRIGAL",
                "WI_DUNGEON_FL_MAS"
            };
            worldConfiguration.Language = "en";
            worldConfiguration.Drops.OwnershipTime = 7;
            worldConfiguration.Drops.DespawnTime = 120;

            Console.WriteLine("--------------------------------");
            Console.WriteLine("World Server Configuration:");
            Console.WriteLine($"Host: {worldConfiguration.Host}");
            Console.WriteLine($"Port: {worldConfiguration.Port}");
            Console.WriteLine($"ISC Host: {worldConfiguration.ISC.Host}");
            Console.WriteLine($"ISC Port: {worldConfiguration.ISC.Port}");

            Console.WriteLine("Rates:");
            Console.WriteLine($"Drop: x{worldConfiguration.Rates.Drop}");
            Console.WriteLine($"Gold: x{worldConfiguration.Rates.Gold}");
            Console.WriteLine($"Experience: x{worldConfiguration.Rates.Experience}");

            Console.WriteLine("Maps:");
            for (int i = 0; i < worldConfiguration.Maps.Count(); i++)
            {
                Console.WriteLine($"- {worldConfiguration.Maps.ElementAt(i)}");
            }

            bool response = _consoleHelper.AskConfirmation("Save this configuration?");

            if (response)
            {
                ConfigurationHelper.Save(WorldServerConfigurationPath, worldConfiguration);
                Console.WriteLine($"World Server configuration saved in {WorldServerConfigurationPath}!");
            }
        }
    }
}
