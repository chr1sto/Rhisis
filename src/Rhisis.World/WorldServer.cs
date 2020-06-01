using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Core.Resources;
using Rhisis.Core.Resources.Loaders;
using Rhisis.Core.Structures.Configuration.World;
using Rhisis.Database;
using Rhisis.Network;
using Rhisis.Scripting.Quests;
using Rhisis.World.Client;
using Rhisis.World.Game.Behaviors;
using Rhisis.World.Game.Chat;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Maps;
using Rhisis.World.Packets;
using Sylver.HandlerInvoker;
using Sylver.Network.Server;
using System;
using System.Linq;

namespace Rhisis.World
{
    public sealed partial class WorldServer : NetServer<WorldServerClient>, IWorldServer
    {
        private const int ClientBufferSize = 128;
        private const int ClientBacklog = 50;

        private readonly ILogger<WorldServer> _logger;
        private readonly IWorldServerTaskManager _worldServerTaskManager;
        private readonly WorldConfiguration _worldConfiguration;
        private readonly IGameResources _gameResources;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapManager _mapManager;
        private readonly IBehaviorManager _behaviorManager;
        private readonly IChatCommandManager _chatCommandManager;
        private readonly IRhisisDatabase _database;

        /// <summary>
        /// Creates a new <see cref="WorldServer"/> instance.
        /// </summary>
        public WorldServer(ILogger<WorldServer> logger, IOptions<WorldConfiguration> worldConfiguration, 
            IWorldServerTaskManager worldServerTaskManager,
            IGameResources gameResources, IServiceProvider serviceProvider, 
            IMapManager mapManager, IBehaviorManager behaviorManager, IChatCommandManager chatCommandManager, IRhisisDatabase database)
        {
            _logger = logger;
            _worldServerTaskManager = worldServerTaskManager;
            _worldConfiguration = worldConfiguration.Value;
            _gameResources = gameResources;
            _serviceProvider = serviceProvider;
            _mapManager = mapManager;
            _behaviorManager = behaviorManager;
            _chatCommandManager = chatCommandManager;
            _database = database;
            PacketProcessor = new FlyffPacketProcessor();
            ServerConfiguration = new NetServerConfiguration(_worldConfiguration.Host, _worldConfiguration.Port, ClientBacklog, ClientBufferSize);
        }

        /// <inheritdoc />
        protected override void OnBeforeStart()
        {
            if (!_database.IsAlive())
            {
                throw new InvalidProgramException($"Cannot start {nameof(WorldServer)}. Failed to reach database.");
            }

            _gameResources.Load(typeof(DefineLoader),
                typeof(TextLoader),
                typeof(MoverLoader),
                typeof(ItemLoader),
                typeof(DialogLoader),
                typeof(ShopLoader),
                typeof(JobLoader),
                typeof(SkillLoader),
                typeof(ExpTableLoader),
                typeof(PenalityLoader),
                typeof(NpcLoader),
                typeof(QuestLoader));

            _chatCommandManager.Load();
            _behaviorManager.Load();
            _mapManager.Load();
        }

        /// <inheritdoc />
        protected override void OnAfterStart()
        {
            _worldServerTaskManager.Start();
            _logger.LogInformation("'{0}' world server is started and listenening on {1}:{2}.",
                _worldConfiguration.Name, ServerConfiguration.Host, ServerConfiguration.Port);
        }

        /// <inheritdoc />
        protected override void OnBeforeStop()
        {
            _worldServerTaskManager.Stop();
        }

        /// <inheritdoc />
        protected override void OnClientConnected(WorldServerClient serverClient)
        {
            serverClient.Initialize(_serviceProvider.GetRequiredService<ILogger<WorldServerClient>>(),
                _serviceProvider.GetRequiredService<IHandlerInvoker>(),
                _serviceProvider.GetRequiredService<IWorldServerPacketFactory>());

            _logger.LogInformation("New client connected from {0}.", serverClient.Socket.RemoteEndPoint);
        }

        /// <inheritdoc />
        protected override void OnClientDisconnected(WorldServerClient serverClient)
        {
            _logger.LogInformation("Client disconnected from {0}.", serverClient.Socket.RemoteEndPoint);
        }

        /// <inheritdoc />
        public IPlayerEntity GetPlayerEntity(uint id) => Clients.FirstOrDefault(x => x.Player.Id == id)?.Player;

        /// <inheritdoc />
        public IPlayerEntity GetPlayerEntity(string name) 
            => Clients.FirstOrDefault(x => x.Player.Object.Name.Equals(name, StringComparison.OrdinalIgnoreCase))?.Player;

        /// <inheritdoc />
        public IPlayerEntity GetPlayerEntityByCharacterId(uint id) 
            => Clients.FirstOrDefault(x => x.Player.PlayerData.Id == id)?.Player;

        /// <inheritdoc />
        public uint GetOnlineConnectedPlayerNumber() 
            => (uint)Clients.Count();
    }
}
