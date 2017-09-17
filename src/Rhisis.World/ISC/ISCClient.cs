using Ether.Network;
using System.Collections.Generic;
using Ether.Network.Packets;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Core.IO;
using Rhisis.Core.Network;
using Rhisis.Core.Exceptions;
using Rhisis.Core.ISC.Packets;

namespace Rhisis.World.ISC
{
    public sealed class ISCClient : NetClient
    {
        private WorldConfiguration _worldConfiguration;

        /// <summary>
        /// Gets the world server's configuration.
        /// </summary>
        public WorldConfiguration Configuration => this._worldConfiguration;

        public ISCClient(WorldConfiguration worldConfiguration) 
            : base(worldConfiguration.IPC.Host, worldConfiguration.IPC.Port, 1024)
        {
            this._worldConfiguration = worldConfiguration;
        }

        protected override void HandleMessage(NetPacketBase packet)
        {
            var packetHeaderNumber = packet.Read<uint>();

            try
            {
                PacketHandler<ISCClient>.Invoke(this, packet, (InterPacketType)packetHeaderNumber);
            }
            catch (KeyNotFoundException)
            {
                Logger.Warning("Unknown inter-server packet with header: 0x{0}", packetHeaderNumber.ToString("X2"));
            }
            catch (RhisisPacketException packetException)
            {
                Logger.Error(packetException.Message);
#if DEBUG
                Logger.Debug("STACK TRACE");
                Logger.Debug(packetException.InnerException?.StackTrace);
#endif
            }
        }

        protected override void OnConnected()
        {
        }

        protected override void OnDisconnected()
        {
            Logger.Info("Disconnected from Inter-Server.");
        }
    }
}
