using Ether.Network.Packets;
using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.Network.Packets.World.Taskbar;
using Rhisis.World.Systems.Taskbar;
using Rhisis.World.Systems.Taskbar.EventArgs;

namespace Rhisis.World.Handlers
{
    public static class TaskbarHandler
    {
        [PacketHandler(PacketType.ADDAPPLETTASKBAR)]
        public static void OnAddTaskbarApplet(WorldClient client, INetPacketStream packet)
        {
            var addTaskbarAppletPacket = new AddTaskbarAppletPacket(packet);
            var addTaskbarAppletEventArgs = new AddTaskbarAppletEventArgs(addTaskbarAppletPacket.SlotIndex, addTaskbarAppletPacket.Type, addTaskbarAppletPacket.ObjectId, addTaskbarAppletPacket.ObjectType, addTaskbarAppletPacket.ObjectIndex, addTaskbarAppletPacket.UserId, addTaskbarAppletPacket.ObjectData, addTaskbarAppletPacket.Text);
            
            client.Player.NotifySystem<TaskbarSystem>(addTaskbarAppletEventArgs);
        }

        [PacketHandler(PacketType.REMOVEAPPLETTASKBAR)]
        public static void OnRemoveTaskbarApplet(WorldClient client, INetPacketStream packet)
        {
            var removeTaskbarAppletPacket = new RemoveTaskbarAppletPacket(packet);
            var removeTaskbarAppletEventArgs = new RemoveTaskbarAppletEventArgs(removeTaskbarAppletPacket.SlotIndex);

            client.Player.NotifySystem<TaskbarSystem>(removeTaskbarAppletEventArgs);
        }

        [PacketHandler(PacketType.ADDITEMTASKBAR)]
        public static void OnAddTaskbarItem(WorldClient client, INetPacketStream packet)
        {
            var addTaskbarItemPacket = new AddTaskbarItemPacket(packet);
            var addTaskbarItemEventArgs = new AddTaskbarItemEventArgs(addTaskbarItemPacket.SlotLevelIndex, addTaskbarItemPacket.SlotIndex, addTaskbarItemPacket.Type, addTaskbarItemPacket.ObjectId, addTaskbarItemPacket.ObjectType, addTaskbarItemPacket.ObjectIndex, addTaskbarItemPacket.UserId, addTaskbarItemPacket.ObjectData, addTaskbarItemPacket.Text);
                       
            client.Player.NotifySystem<TaskbarSystem>(addTaskbarItemEventArgs);
        }

        [PacketHandler(PacketType.REMOVEITEMTASKBAR)]
        public static void OnRemoveTaskbarItem(WorldClient client, INetPacketStream packet)
        {
            var removeTaskbarItemPacket = new RemoveTaskbarItemPacket(packet);
            var removeTaskbarItemEventArgs = new RemoveTaskbarItemEventArgs(removeTaskbarItemPacket.SlotLevelIndex, removeTaskbarItemPacket.SlotIndex);

            client.Player.NotifySystem<TaskbarSystem>(removeTaskbarItemEventArgs);
        }

        [PacketHandler(PacketType.SKILLTASKBAR)]
        public static void OnTaskbarSkill(WorldClient client, INetPacketStream packet)
        {
            var taskbarSkillPacket = new TaskbarSkillPacket(packet);
            var taskbarSkillEventArgs = new TaskbarSkillEventArgs(taskbarSkillPacket.Skills);

            client.Player.NotifySystem<TaskbarSystem>(taskbarSkillEventArgs);
        }
    }
}