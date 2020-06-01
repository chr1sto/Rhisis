using Sylver.Network.Data;
using Rhisis.Core.Common;
using Rhisis.Core.Common.Game.Structures;
using System.Collections.Generic;

namespace Rhisis.Network.Packets.World.Taskbar
{
    public class TaskbarSkillPacket : IPacketDeserializer
    {
        public List<Shortcut> Skills { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            Skills = new List<Shortcut>(new Shortcut[packet.Read<int>()]);

            for(int i = 0; i < Skills.Count; i++)
            {
                var index = packet.Read<byte>();
                if (index >= Skills.Count)
                    return;

                var type = (ShortcutType)packet.Read<uint>();
                var objId = packet.Read<uint>();
                var objType = (ShortcutObjectType)packet.Read<uint>();
                var objIndex = packet.Read<uint>();
                var userId = packet.Read<uint>();
                var objData = packet.Read<uint>();
                string text = null;

                if (type == ShortcutType.Chat)
                    text = packet.Read<string>();

                Skills[i] = new Shortcut(index, type, objId, objType, objIndex, userId, objData, text);
            }
        }
    }
}