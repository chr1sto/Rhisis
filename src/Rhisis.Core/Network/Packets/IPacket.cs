namespace Rhisis.Core.Network.Packets
{
    public interface IPacket
    {
        void Read(FFPacket packet);
    }
}
