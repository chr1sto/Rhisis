using Ether.Network.Packets;
using NLog;
using Rhisis.Network.ISC.Packets;
using Rhisis.Network;
using System;

namespace Rhisis.World.ISC
{
    public static class ISCHandler
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        [PacketHandler(ISCPacketType.WELCOME)]
        public static void OnWelcome(ISCClient client, INetPacketStream packet)
        {
            ISCPacketFactory.SendAuthentication(client, client.WorldConfiguration);
        }

        [PacketHandler(ISCPacketType.AUTHENT_RESULT)]
        public static void OnAuthenticationResult(ISCClient client, INetPacketStream packet)
        {
            var authenticationResult = (ISCPacketCode)(packet.Read<uint>());

            switch (authenticationResult)
            {
                case ISCPacketCode.AUTH_SUCCESS:
                    Logger.Info("ISC client authenticated succesfully.");
                    return;
                case ISCPacketCode.AUTH_FAILED_NO_CLUSTER:
                    Logger.Fatal("Unable to authenticate ISC client. Reason: parent cluster is not connected.");
                    break;
                case ISCPacketCode.AUTH_FAILED_WORLD_EXISTS:
                    Logger.Fatal("Unable to authenticate ISC client. Reason: an other World server (with the same id) is already connected.");
                    break;
                case ISCPacketCode.AUTH_FAILED_UNKNOWN_SERVER:
                    Logger.Fatal("Unable to authenticate ISC client. Reason: ISC server doesn't recognize this server. You probably have to update all servers.");
                    break;
                default:
                    Logger.Trace("ISC authentification result: {0}", authenticationResult);
                    Logger.Fatal("Unable to authenticate ISC client. Reason: Cannot recognize ISC server. You probably have to update all servers.");
                    break;
            }

            //TODO: implement a peacefully shutdown.
            Console.ReadLine();
            Environment.Exit((int)authenticationResult);
        }
    }
}
