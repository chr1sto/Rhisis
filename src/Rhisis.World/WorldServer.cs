using Ether.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Ether.Network.Packets;
using Rhisis.Core.Network;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Core.Helpers;
using Rhisis.Core.IO;
using Rhisis.World.ISC;
using Rhisis.Database;
using Rhisis.Database.Exceptions;

namespace Rhisis.World
{
    public sealed class WorldServer : NetServer<WorldClient>
    {
        private static readonly string WorldConfigFile = "config/world.json";
        private static readonly string DatabaseConfigFile = "config/database.json";
        private static ISCClient _client;

        /// <summary>
        /// Gets the Inter client.
        /// </summary>
        public static ISCClient InterClient => _client;

        /// <summary>
        /// Gets the world server's configuration.
        /// </summary>
        public WorldConfiguration WorldConfiguration { get; private set; }

        /// <summary>
        /// Creates a new <see cref="WorldServer"/> instance.
        /// </summary>
        public WorldServer()
        {
            Logger.Initialize();
            this.LoadConfiguration();
        }

        /// <summary>
        /// Initialize the world server's resources.
        /// </summary>
        protected override void Initialize()
        {
            PacketHandler<WorldClient>.Initialize();
            PacketHandler<ISCClient>.Initialize();

            var databaseConfiguration = ConfigurationHelper.Load<DatabaseConfiguration>(DatabaseConfigFile, true);

            DatabaseService.Configure(databaseConfiguration);
            if (!DatabaseService.GetContext().DatabaseExists())
                throw new RhisisDatabaseException($"The database '{databaseConfiguration.Database}' doesn't exists yet.");

            // TODO: Load resources

            _client = new ISCClient(this.WorldConfiguration);
            _client.Connect();

            Logger.Info("Rhisis world server is up");
        }

        /// <summary>
        /// Fired when a client is connected to the world server.
        /// </summary>
        /// <param name="connection"></param>
        protected override void OnClientConnected(WorldClient connection)
        {
            Logger.Info("New client connected: {0}", connection.Id);
            connection.InitializeClient();
        }

        /// <summary>
        /// Fired when a client disconnects from this cluster server.
        /// </summary>
        /// <param name="connection"></param>
        protected override void OnClientDisconnected(WorldClient connection)
        {
            Logger.Info("Client {0} disconnected.", connection.Id);
        }

        /// <summary>
        /// Fired when an error occurs.
        /// </summary>
        /// <param name="exception"></param>
        protected override void OnError(Exception exception)
        {
            // TODO: handle
        }

        /// <summary>
        /// Split the incoming network data into flyff packets.
        /// </summary>
        /// <param name="buffer">Incoming buffer</param>
        /// <returns></returns>
        protected override IReadOnlyCollection<NetPacketBase> SplitPackets(byte[] buffer)
        {
            return FFPacket.SplitPackets(buffer);
        }

        /// <summary>
        /// Load the world server's configuration.
        /// </summary>
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
