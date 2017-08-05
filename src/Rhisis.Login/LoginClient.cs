using Ether.Network;
using Ether.Network.Packets;
using System;

namespace Rhisis.Login
{
    public sealed class LoginClient : NetConnection
    {
        public override void HandleMessage(NetPacketBase packet)
        {
            throw new NotImplementedException();
        }
    }
}