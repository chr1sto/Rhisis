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
            var handlers = from type in typeof(T).Assembly.GetTypes()
                           let typeInfo = type.GetTypeInfo()
                           let methodsInfo = typeInfo.GetMethods(BindingFlags.Static | BindingFlags.Public)
                           let handler = (from x in methodsInfo
                                          let attribute = x.GetCustomAttribute<PacketHandlerAttribute>()
                                          where attribute != null
                                          select new MethodHandler(x, attribute)).ToArray()
                           select handler;

            foreach (var handler in handlers)
            {
                foreach (var methodHandler in handler)
                {
                    ParameterInfo[] parameters = methodHandler.Method.GetParameters();

                    if (parameters.Count() < 2)
                        continue;
                    
                    if (parameters.First().ParameterType != typeof(T))
                        continue;

                    var action = methodHandler.Method.CreateDelegate(typeof(Action<T, NetPacketBase>)) as Action<T, NetPacketBase>;

                    _handlers.Add(methodHandler.Attribute.Header, action);
                }
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
