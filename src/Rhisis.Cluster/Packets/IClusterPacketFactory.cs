using Rhisis.Network.Packets;
using Rhisis.Database.Entities;
using System.Collections.Generic;
using Rhisis.Cluster.Client;

namespace Rhisis.Cluster.Packets
{
    public interface IClusterPacketFactory
    {
        void SendWelcome(IClusterClient client);

        void SendPong(IClusterClient client, int time);

        void SendClusterError(IClusterClient client, ErrorType errorType);

        void SendPlayerList(IClusterClient client, int authenticationKey, IEnumerable<DbCharacter> characters);

        void SendWorldAddress(IClusterClient client, string address);

        void SendLoginNumPad(IClusterClient client, int loginProtectValue);

        void SendLoginProtect(IClusterClient client, int loginProtectValue);

        void SendJoinWorld(IClusterClient client);

        void SendQueryTickCount(IClusterClient client, uint time);
    }
}
