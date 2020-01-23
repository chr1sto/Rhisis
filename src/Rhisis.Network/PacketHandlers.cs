using Sylver.Network.Data;
using Rhisis.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhisis.Network
{
    public static class PacketHandler<T>
    {
        private static readonly Dictionary<object, Action<T, INetPacketStream>> Handlers = new Dictionary<object, Action<T, INetPacketStream>>();

        private struct PacketMethodHandler : IEquatable<PacketMethodHandler>
        {
            public readonly PacketHandlerAttribute Attribute;
            public readonly MethodInfo Method;

            public PacketMethodHandler(MethodInfo method, PacketHandlerAttribute attribute)
            {
                Method = method;
                Attribute = attribute;
            }

            public bool Equals(PacketMethodHandler other)
            {
                return Attribute.Header == other.Attribute.Header
                    && Attribute.TypeId == other.Attribute.TypeId
                    && Method == other.Method;
            }
        }

        public static void Initialize()
        {
            IEnumerable<PacketMethodHandler[]> handlers = from type in typeof(T).Assembly.GetTypes()
                                                          let typeInfo = type.GetTypeInfo()
                                                          let methodsInfo = typeInfo.GetMethods(BindingFlags.Static | BindingFlags.Public)
                                                          let handler = (from x in methodsInfo
                                                                         let attribute = x.GetCustomAttribute<PacketHandlerAttribute>()
                                                                         where attribute != null
                                                                         select new PacketMethodHandler(x, attribute)).ToArray()
                                                          select handler;

            foreach (PacketMethodHandler[] handler in handlers)
            {
                foreach (PacketMethodHandler methodHandler in handler)
                {
                    ParameterInfo[] parameters = methodHandler.Method.GetParameters();

                    if (parameters.Count() < 2 || parameters.First().ParameterType != typeof(T))
                        continue;

                    var action = methodHandler.Method.CreateDelegate(typeof(Action<T, INetPacketStream>)) as Action<T, INetPacketStream>;

                    Handlers.Add(methodHandler.Attribute.Header, action);
                }
            }
        }

        public static bool Invoke(T invoker, INetPacketStream packet, object packetHeader)
        {
            if (!Handlers.TryGetValue(packetHeader, out Action<T, INetPacketStream> packetHandler))
                return false;

            try
            {
                packetHandler.Invoke(invoker, packet);
            }
            catch (Exception e)
            {
                throw new RhisisPacketException($"An error occured during the execution of packet handler: {packetHeader}", e);
            }

            return true;
        }
    }
}
