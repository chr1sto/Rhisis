using Ether.Network.Common;
using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.Database.Entities;
using System.Collections.Generic;
using System.Linq;
using Rhisis.Core.IO;

namespace Rhisis.Cluster.Packets
{
    public static class ClusterPacketFactory
    {
        /// <summary>
        /// Send welcome packet to new client.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="sessionId"></param>
        public static void SendWelcome(NetUser client, uint sessionId)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.WELCOME);
                packet.Write(sessionId);

                client.Send(packet);
            }
        }

        /// <summary>
        /// Send pong packet to client.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="time"></param>
        public static void SendPong(NetUser client, int time)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.PING);
                packet.Write(time);

                client.Send(packet);
            }
        }

        /// <summary>
        /// Send an error packet to the client
        /// </summary>
        /// <param name="client"></param>
        /// <param name="error"></param>
        public static void SendError(NetUser client, ErrorType error)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.ERROR);
                packet.Write((int)error);

                client.Send(packet);
            }
        }

        /// <summary>
        /// Sends the client's player list.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="authenticationKey"></param>
        /// <param name="characters"></param>
        public static void SendPlayerList(NetUser client, int authenticationKey, IEnumerable<DbCharacter> characters)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.PLAYER_LIST);
                packet.Write(authenticationKey);

                packet.Write(characters.Count()); // player count

                foreach (var character in characters)
                {
                    packet.Write(character.Slot);
                    packet.Write(1); // this number represents the selected character in the window
                    packet.Write(character.MapId);
                    packet.Write(0x0B + character.Gender); // Model id
                    packet.Write(character.Name);
                    packet.Write(character.PosX);
                    packet.Write(character.PosY);
                    packet.Write(character.PosZ);
                    packet.Write(character.Id);
                    packet.Write(0); // Party id
                    packet.Write(0); // Guild id
                    packet.Write(0); // War Id
                    packet.Write(character.SkinSetId);
                    packet.Write(character.HairId);
                    packet.Write((uint)character.HairColor);
                    packet.Write(character.FaceId);
                    packet.Write(character.Gender);
                    packet.Write(character.ClassId);
                    packet.Write(character.Level);
                    packet.Write(0); // Job Level (Maybe master or hero ?)
                    packet.Write(character.Strength);
                    packet.Write(character.Stamina);
                    packet.Write(character.Dexterity);
                    packet.Write(character.Intelligence);
                    packet.Write(0); // Mode ??
                    packet.Write(character.Items.Count(i => i.ItemSlot > 42));

                    foreach (var item in character.Items.Where(i => i.ItemSlot > 42))
                        packet.Write(item.ItemId);
                }

                packet.Write(0); // Messenger?

                client.Send(packet);
            }
        }

        /// <summary>
        /// Send the selected world server address.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="address"></param>
        public static void SendWorldAddress(NetUser client, string address)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.CACHE_ADDR);
                packet.Write(address);

                client.Send(packet);
            }
        }

        /// <summary>
        /// Send the Login num pad if the 2nd password option is enabled.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="loginProtectValue"></param>
        public static void SendLoginNumPad(NetUser client, int loginProtectValue)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.LOGIN_PROTECT_NUMPAD);
                packet.Write(loginProtectValue);

                client.Send(packet);
            }
        }

        /// <summary>
        /// Send the login protect data.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="loginProtectValue"></param>
        public static void SendLoginProtect(NetUser client, int loginProtectValue)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.LOGIN_PROTECT_CERT);
                packet.Write(0);
                packet.Write(loginProtectValue);

                client.Send(packet);
            }
        }

        /// <summary>
        /// Sends a request telling the player can join the world.
        /// </summary>
        /// <param name="client"></param>
        public static void SendJoinWorld(NetUser client)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.PRE_JOIN);

                client.Send(packet);
            }
        }

        public static void SendQueryTickCount(NetUser client, uint time)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.QUERYTICKCOUNT);

                packet.Write(time);
                packet.Write(Time.GetElapsedTime());

                client.Send(packet);
            }
        }
    }
}
