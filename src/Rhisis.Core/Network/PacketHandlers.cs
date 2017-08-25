using Ether.Network.Packets;
using Rhisis.Core.Exceptions;
using Rhisis.Core.Network.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhisis.Core.Network
{
    public static class PacketHandler<T>
    {
        private static readonly Dictionary<object, Action<T, NetPacketBase>> _handlers = new Dictionary<object, Action<T, NetPacketBase>>();

        private struct MethodHandler
        {
            public PacketHandlerAttribute Attribute;
            public MethodInfo Method;

            public MethodHandler(MethodInfo method, PacketHandlerAttribute attribute)
            {
                this.Method = method;
                this.Attribute = attribute;
            }
        }

        public static void Initialize()
        {
            TypeInfo typeInfo = typeof(T).GetTypeInfo();
            MethodInfo[] methodsInfo = typeInfo.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            IEnumerable<MethodHandler> methods = from x in methodsInfo
                                                 let attribute = x.GetCustomAttribute<PacketHandlerAttribute>()
                                                 where attribute != null
                                                 select new MethodHandler(x, attribute);

            foreach (MethodHandler methodHandler in methods)
            {
                var action = methodHandler.Method.CreateDelegate(typeof(Action<T, NetPacketBase>)) as Action<T, NetPacketBase>;

                _handlers.Add(methodHandler.Attribute.Header, action);
            }
        }

        public static void Invoke(T invoker, NetPacketBase packet, object packetHeader)
        {
            if (!_handlers.ContainsKey(packetHeader))
                throw new KeyNotFoundException();

            try
            {
                _handlers[packetHeader].Invoke(invoker, packet);
            }
            catch (Exception e)
            {
                throw new RhisisPacketException($"An error occured during the execution of packet handler: {packetHeader}", e);
            }
        }
    }
}
