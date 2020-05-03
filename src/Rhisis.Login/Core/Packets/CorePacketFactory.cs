using Rhisis.Network.Core;
using Sylver.Network.Data;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Login.Core.Packets
{
    public class CorePacketFactory : ICorePacketFactory
    {
        /// <inheritdoc />
        public void SendWelcome(ICoreServerClient client)
        {
            using (var packet = new NetPacket())
            {
                packet.Write((uint)CorePacketType.Welcome);
                client.Send(packet);
            }
        }

        /// <inheritdoc />
        public void SendAuthenticationResult(ICoreServerClient client, CoreAuthenticationResultType authenticationResultType)
        {
            using (var packet = new NetPacket())
            {
                packet.Write((uint)CorePacketType.AuthenticationResult);
                packet.Write((uint)authenticationResultType);
                client.Send(packet);
            }
        }
    }
}
