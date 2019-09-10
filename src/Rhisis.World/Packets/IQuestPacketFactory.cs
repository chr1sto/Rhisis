using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;

namespace Rhisis.World.Packets
{
    public interface IQuestPacketFactory
    {
        void SendQuest(IPlayerEntity player, Quest quest);
    }
}
