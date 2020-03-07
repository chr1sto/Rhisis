using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Core.Resources;
using Rhisis.Core.Resources.Loaders;
using Rhisis.Core.Structures.Configuration.World;
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
    public sealed partial class WorldServer : NetServer<WorldClient>, IWorldServer
    {
        private const int ClientBufferSize = 128;
        private const int ClientBacklog = 50;

        private readonly ILogger<WorldServer> _logger;
        private readonly WorldConfiguration _worldConfiguration;
        private readonly IGameResources _gameResources;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapManager _mapManager;
        private readonly IBehaviorManager _behaviorManager;
        private readonly IChatCommandManager _chatCommandManager;

        /// <summary>
        /// Creates a new <see cref="WorldServer"/> instance.
        /// </summary>
        public WorldServer(ILogger<WorldServer> logger, IOptions<WorldConfiguration> worldConfiguration, IGameResources gameResources, IServiceProvider serviceProvider, IMapManager mapManager, IBehaviorManager behaviorManager, IChatCommandManager chatCommandManager)
        {
            _logger = logger;
            _worldConfiguration = worldConfiguration.Value;
            _gameResources = gameResources;
            _serviceProvider = serviceProvider;
            _mapManager = mapManager;
            _behaviorManager = behaviorManager;
            _chatCommandManager = chatCommandManager;
            PacketProcessor = new FlyffPacketProcessor();
            ServerConfiguration = new NetServerConfiguration(_worldConfiguration.Host, _worldConfiguration.Port, ClientBacklog, ClientBufferSize);
        }

        /// <inheritdoc />
        protected override void OnBeforeStart()
        {
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
            _logger.LogInformation("'{0}' world server is started and listen on {1}:{2}.",
                _worldConfiguration.Name, ServerConfiguration.Host, ServerConfiguration.Port);
        }

        /// <inheritdoc />
        protected override void OnClientConnected(WorldClient client)
        {
            client.Initialize(_serviceProvider.GetRequiredService<ILogger<WorldClient>>(),
                _serviceProvider.GetRequiredService<IHandlerInvoker>(),
                _serviceProvider.GetRequiredService<IWorldServerPacketFactory>());

            _logger.LogInformation("New client connected from {0}.", client.Socket.RemoteEndPoint);
        }

        /// <inheritdoc />
        protected override void OnClientDisconnected(WorldClient client)
        {
            _logger.LogInformation("Client disconnected from {0}.", client.Socket.RemoteEndPoint);
        }

        /// <inheritdoc />
        //protected override void OnError(Exception exception)
        //{
        //    this._logger.LogError("WorldServer Error: {0}", exception.Message);
        //}

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
