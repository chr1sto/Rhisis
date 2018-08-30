using Ether.Network.Packets;
using Ether.Network.Server;
using NLog;
using Rhisis.Business;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Helpers;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Database;
using Rhisis.Database.Exceptions;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Maps;
using Rhisis.World.ISC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World
{
    public sealed partial class WorldServer : NetServer<WorldClient>, IWorldServer
    {
        private const string WorldConfigFile = "config/world.json";
        private const string DatabaseConfigFile = "config/database.json";

        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private static readonly IDictionary<int, IMapInstance> _maps = new Dictionary<int, IMapInstance>();

        /// <summary>
        /// Gets the World server maps.
        /// </summary>
        public static IReadOnlyDictionary<int, IMapInstance> Maps => _maps as IReadOnlyDictionary<int, IMapInstance>;

        /// <summary>
        /// Gets the ISC client.
        /// </summary>
        public static ISCClient InterClient { get; private set; }

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
            this.LoadConfiguration();
        }

        /// <summary>
        /// Load the world server's configuration.
        /// </summary>
        private void LoadConfiguration()
        {
            Logger.Debug("Loading server configuration from '{0}'...", WorldConfigFile);
            this.WorldConfiguration = ConfigurationHelper.Load<WorldConfiguration>(WorldConfigFile, true);

            this.Configuration.Host = this.WorldConfiguration.Host;
            this.Configuration.Port = this.WorldConfiguration.Port;
            this.Configuration.MaximumNumberOfConnections = 1000;
            this.Configuration.Backlog = 100;
            this.Configuration.BufferSize = 4096;

            Logger.Trace("Host: {0}, Port: {1}, MaxNumberOfConnections: {2}, Backlog: {3}, BufferSize: {4}",
                this.Configuration.Host,
                this.Configuration.Port,
                this.Configuration.MaximumNumberOfConnections,
                this.Configuration.Backlog,
                this.Configuration.BufferSize);
        }

        /// <inheritdoc />
        protected override void Initialize()
        {
            PacketHandler<ISCClient>.Initialize();
            PacketHandler<WorldClient>.Initialize();

            Logger.Debug("Loading database configuration from '{0}'...", DatabaseConfigFile);
            DatabaseFactory.Instance.Initialize(DatabaseConfigFile);

            if (!DatabaseFactory.Instance.DatabaseExists())
                throw new RhisisDatabaseException($"The database '{DatabaseFactory.Instance.Configuration.Database}' doesn't exists.");

            Logger.Trace($"Database config -> {DatabaseFactory.Instance.Configuration}");

            BusinessLayer.Initialize();
            DependencyContainer.Instance.Initialize();
            this.LoadResources();

            Logger.Info("Connection to ISC server on {0}:{1}...", this.WorldConfiguration.ISC.Host, this.WorldConfiguration.ISC.Port);
            InterClient = new ISCClient(this.WorldConfiguration);
            InterClient.Connect();

            //TODO: Implement this log inside OnStarted method when will be available.
            Logger.Info("'{0}' world server is started and listen on {1}:{2}.",
                InterClient.WorldConfiguration.Name, this.Configuration.Host, this.Configuration.Port);
        }

        /// <inheritdoc />
        protected override void OnClientConnected(WorldClient client)
        {
            client.InitializeClient(this);
            Logger.Info("New client connected from {0}.", client.RemoteEndPoint);
            CommonPacketFactory.SendWelcome(client, client.SessionId);
        }

        /// <inheritdoc />
        protected override void OnClientDisconnected(WorldClient client)
        {
            Logger.Info("Client disconnected from {0}.", client.RemoteEndPoint);
        }

        /// <inheritdoc />
        protected override void OnError(Exception exception)
        {
            Logger.Error("WorldServer Error: {0}", exception.Message);
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
