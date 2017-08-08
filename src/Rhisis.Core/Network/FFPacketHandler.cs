using Ether.Network.Packets;
using Rhisis.Core.Exceptions;
using Rhisis.Core.Network.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Rhisis.Core.Network
{
    public static class FFPacketHandler<T>
    {
        private static readonly Dictionary<PacketType, Action<T, FFPacket>> _handlers = new Dictionary<PacketType, Action<T, FFPacket>>();

        public static void Initialize()
        {
            Type type = typeof(T);
            TypeInfo typeInfo = type.GetTypeInfo();
            MethodInfo[] methodsInfo = typeInfo.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

            IEnumerable<MethodInfo> methodsWithAttribute = from x in methodsInfo
                                                           where x.GetCustomAttribute<FFIncomingPacketAttribute>() != null
                                                           select x;

            foreach (MethodInfo method in methodsWithAttribute)
            {
                var attribute = method.GetCustomAttribute<FFIncomingPacketAttribute>();
                
                var action = method.CreateDelegate(typeof(Action<T, FFPacket>)) as Action<T, FFPacket>;

                _handlers.Add(attribute.Header, action);
            }
        }

        public static void Invoke(T invoker, FFPacket packet)
        {
            var packetHeaderNumber = packet.Read<uint>();
            var packetHeader = (PacketType)packetHeaderNumber;

            if (_handlers.ContainsKey(packetHeader))
                _handlers[packetHeader]?.Invoke(invoker, packet);
        }
    }
}
