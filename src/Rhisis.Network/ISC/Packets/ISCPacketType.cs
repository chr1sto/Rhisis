namespace Rhisis.Network.ISC.Packets
{
    public enum ISCPacketType : uint
    {
        WELCOME = 0x01,
        AUTHENT = 0x02,
        AUTHENT_RESULT = 0x03,
        UPDATE_CLUSTER_WORLDS_LIST = 0x04,
    }
}
