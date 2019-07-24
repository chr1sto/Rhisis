using Microsoft.Extensions.Logging;
using Rhisis.Cluster.Client;
using Rhisis.Core.Handlers.Attributes;
using Rhisis.Database;
using Rhisis.Network.Packets;
using Rhisis.Network.Packets.Cluster;

namespace Rhisis.Cluster.Handlers
{
    [Handler]
    public class CharacterHandler
    {
        private readonly ILogger<CharacterHandler> _logger;
        private readonly IDatabase _database;
        private readonly IClusterServer _clusterServer;

        public CharacterHandler(ILogger<CharacterHandler> logger, IDatabase database, IClusterServer clusterServer)
        {
            this._logger = logger;
            this._database = database;
            this._clusterServer = clusterServer;
        }

        [HandlerAction(PacketType.GETPLAYERLIST)]
        public void OnGetPlayerList(ClusterClient client, GetPlayerListPacket packet)
        {
            // TODO
        }

        [HandlerAction(PacketType.CREATE_PLAYER)]
        public void OnCreatePlayer(ClusterClient client, CreatePlayerPacket packet)
        {
            // TODO
        }

        [HandlerAction(PacketType.DEL_PLAYER)]
        public void OnDeletePlayer(ClusterClient client, DeletePlayerPacket packet)
        {
            // TODO
        }

        [HandlerAction(PacketType.PRE_JOIN)]
        public void OnPreJoin(ClusterClient client, PreJoinPacket packet)
        {
            // TODO
        }
    }
}
