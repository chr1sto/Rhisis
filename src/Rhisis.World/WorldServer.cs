using Ether.Network.Packets;
using Ether.Network.Server;
using Rhisis.Core.Helpers;
using Rhisis.Core.IO;
using Rhisis.Core.Network;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Database;
using Rhisis.Database.Exceptions;
using Rhisis.World.Game;
using Rhisis.World.Game.Entities;
using Rhisis.World.ISC;
using System;
using System.Collections.Generic;
using System.Linq;
using Rhisis.World.Game.Maps;

namespace Rhisis.World
{
    public sealed partial class WorldServer : NetServer<WorldClient>, IWorldServer
    {
        private const string WorldConfigFile = "config/world.json";
        private const string DatabaseConfigFile = "config/database.json";

        private static readonly IDictionary<int, IMapInstance> _maps = new Dictionary<int, IMapInstance>();
        private static ISCClient _client;

        /// <summary>
        /// Gets the World server maps.
        /// </summary>
        public static IReadOnlyDictionary<int, IMapInstance> Maps => _maps as IReadOnlyDictionary<int, IMapInstance>;

        /// <summary>
        /// Gets the Inter client.
        /// </summary>
        public static ISCClient InterClient => _client;

        /// <summary>
        /// Gets the world server's configuration.
        /// </summary>
        public WorldConfiguration WorldConfiguration { get; private set; }

        /// <inheritdoc />
        protected override IPacketProcessor PacketProcessor { get; } = new FlyffPacketProcessor();

        /// <summary>
        /// Creates a new <see cref="WorldServer"/> instance.
        /// </summary>
        public WorldServer()
        {
            Logger.Initialize("world");
            this.LoadConfiguration();
        }

        /// <inheritdoc />
        protected override void Initialize()
        {
            PacketHandler<WorldClient>.Initialize();
            PacketHandler<ISCClient>.Initialize();

            var databaseConfiguration = ConfigurationHelper.Load<DatabaseConfiguration>(DatabaseConfigFile, true);

            DatabaseService.Configure(databaseConfiguration);

            using (var context = DatabaseService.GetContext())
            {
                if (!context.DatabaseExists())
                    throw new RhisisDatabaseException($"The database '{databaseConfiguration.Database}' doesn't exists yet.");
            }

            this.LoadResources();

            _client = new ISCClient(this.WorldConfiguration);
            _client.Connect();

            Logger.Info("Rhisis world server is up");
        }

        /// <inheritdoc />
        protected override void OnClientConnected(WorldClient connection)
        {
            connection.InitializeClient(this);

            Logger.Info("New client connected: {0}", connection.Id);
        }

        /// <inheritdoc />
        protected override void OnClientDisconnected(WorldClient connection)
        {
            Logger.Info("Client {0} disconnected.", connection.Id);
        }

        /// <inheritdoc />
        protected override void OnError(Exception exception)
        {
            Logger.Error("WorldServer Error: {0}", exception.Message);
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

        /// <summary>
        /// Gets a player entity by his id.
        /// </summary>
        /// <param name="id">Player id</param>
        /// <returns></returns>
        public IPlayerEntity GetPlayerEntity(int id)
        {
            WorldClient client =  this.Clients.FirstOrDefault(x => x.Player.Id == id);

            return client?.Player;
        }
    }
}
