using Ether.Network;
using System;
using Ether.Network.Packets;
using Rhisis.Core.IO;
using Rhisis.Core.Helpers;
using Rhisis.World.Packets;

namespace Rhisis.World
{
    public sealed class WorldClient : NetConnection
    {
        private readonly uint _sessionId;
        private WorldServer _worldServer;

        public WorldClient()
        {
            this._sessionId = RandomHelper.GenerateSessionKey();
        }

        public void InitializeClient(WorldServer worldServer)
        {
            this._worldServer = worldServer;
            WorldPacketFactory.SendWelcome(this, this._sessionId);
        }

        public override void HandleMessage(NetPacketBase packet)
        {
            Logger.Info("Handle messages");
        }
    }
}
