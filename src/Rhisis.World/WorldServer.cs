using Ether.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Ether.Network.Packets;
using Rhisis.Core.Network;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Core.Helpers;
using Rhisis.Core.IO;
using Rhisis.World.IPC;
using Rhisis.Database;
using Rhisis.Database.Exceptions;

namespace Rhisis.World
{
    public sealed class WorldServer : NetServer<WorldClient>
    {
        private static readonly string WorldConfigFile = "config/world.json";
        private static readonly string DatabaseConfigFile = "config/database.json";

        private static IPCClient _client;

        public static IPCClient Client => _client;

        public WorldConfiguration WorldConfiguration { get; private set; }

        public WorldServer()
        {
            Console.Title = "Rhisis - World Server";
            Logger.Initialize();
            this.LoadConfiguration();
        }

        protected override void Initialize()
        {
            PacketHandler<WorldClient>.Initialize();
            PacketHandler<IPCClient>.Initialize();

            var databaseConfiguration = ConfigurationHelper.Load<DatabaseConfiguration>(DatabaseConfigFile, true);

            DatabaseService.Configure(databaseConfiguration);
            if (!DatabaseService.GetContext().DatabaseExists())
                throw new RhisisDatabaseException($"The database '{databaseConfiguration.Database}' doesn't exists yet.");

            _client = new IPCClient(this.WorldConfiguration);
            _client.Connect();

            Logger.Info("Rhisis cluster server is up");
        }

        protected override void OnClientConnected(WorldClient connection)
        {
        }

        protected override void OnClientDisconnected(WorldClient connection)
        {
        }

        protected override IReadOnlyCollection<NetPacketBase> SplitPackets(byte[] buffer)
        {
            return FFPacket.SplitPackets(buffer);
        }

        private void LoadConfiguration()
        {
            this.WorldConfiguration = ConfigurationHelper.Load<WorldConfiguration>(WorldConfigFile, true);

            this.Configuration.Host = this.WorldConfiguration.Host;
            this.Configuration.Port = this.WorldConfiguration.Port;
            this.Configuration.MaximumNumberOfConnections = 1000;
            this.Configuration.Backlog = 100;
            this.Configuration.BufferSize = 4096;
        }

    }
}
