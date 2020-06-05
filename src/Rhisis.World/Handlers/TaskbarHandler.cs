using Rhisis.Network.Packets;
using Rhisis.Network.Packets.World.Taskbar;
using Rhisis.World.Client;
using Rhisis.World.Game.Structures;
using Rhisis.World.Systems.Taskbar;
using Sylver.HandlerInvoker.Attributes;

namespace Rhisis.World.Handlers
{
    /// <summary>
    /// Handles packets related to the taskbar.
    /// </summary>
    [Handler]
    public sealed class TaskbarHandler
    {
        private readonly ITaskbarSystem _taskbarSystem;

        /// <summary>
        /// Creates a new <see cref="TaskbarHandler"/> instance.
        /// </summary>
        /// <param name="taskbarSystem">Taskbar system.</param>
        public TaskbarHandler(ITaskbarSystem taskbarSystem)
        {
            _taskbarSystem = taskbarSystem;
        }

        /// <summary>
        /// Adds a new applet to the applet taskbar.
        /// </summary>
        /// <param name="serverClient">Current client.</param>
        /// <param name="packet">Incoming packet.</param>
        [HandlerAction(PacketType.ADDAPPLETTASKBAR)]
        public void OnAddTaskbarApplet(IWorldServerClient serverClient, AddTaskbarAppletPacket packet)
        {
            int? shortcutItemIndex = packet.Type != Core.Common.ShortcutType.Item && packet.ObjectId == 0 ? null : (int?)packet.ObjectId;

            var shortcutToAdd = new Shortcut(packet.SlotIndex, packet.Type, shortcutItemIndex, packet.ObjectType, packet.ObjectIndex, packet.UserId, packet.ObjectData, packet.Text);

            _taskbarSystem.AddShortcutToAppletTaskbar(serverClient.Player, shortcutToAdd);
        }

        /// <summary>
        /// Removes an applet from the applet taskbar.
        /// </summary>
        /// <param name="serverClient">Current client.</param>
        /// <param name="packet">Incoming packet.</param>
        [HandlerAction(PacketType.REMOVEAPPLETTASKBAR)]
        public void OnRemoveTaskbarApplet(IWorldServerClient serverClient, RemoveTaskbarAppletPacket packet)
        {
            _taskbarSystem.RemoveShortcutFromAppletTaskbar(serverClient.Player, packet.SlotIndex);
        }

        /// <summary>
        /// Adds an item to the item taskbar.
        /// </summary>
        /// <param name="serverClient">Current client.</param>
        /// <param name="packet">Incoming packet.</param>
        [HandlerAction(PacketType.ADDITEMTASKBAR)]
        public void OnAddTaskbarItem(IWorldServerClient serverClient, AddTaskbarItemPacket packet)
        {
            int? shortcutItemIndex = packet.Type != Core.Common.ShortcutType.Item && packet.ObjectId == 0 ? null : (int?)packet.ObjectId;

            var shortcutToAdd = new Shortcut(packet.SlotIndex, packet.Type, shortcutItemIndex, packet.ObjectType, packet.ObjectIndex, packet.UserId, packet.ObjectData, packet.Text);

            _taskbarSystem.AddShortcutToItemTaskbar(serverClient.Player, shortcutToAdd, packet.SlotLevelIndex);
        }

        /// <summary>
        /// Removes an item from the item taskbar.
        /// </summary>
        /// <param name="serverClient">Current client.</param>
        /// <param name="packet">Incoming packet.</param>
        [HandlerAction(PacketType.REMOVEITEMTASKBAR)]
        public void OnRemoveTaskbarItem(IWorldServerClient serverClient, RemoveTaskbarItemPacket packet)
        {
            _taskbarSystem.RemoveShortcutFromItemTaskbar(serverClient.Player, packet.SlotLevelIndex, packet.SlotIndex);
        }
    }
}