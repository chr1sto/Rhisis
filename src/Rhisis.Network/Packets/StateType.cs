namespace Rhisis.Network.Packets
{
    public enum StateType : uint
    {
        STATE_PK_MODE = 0x00000001,
        STATE_PVP_MODE = 0x00000002,
        STATE_BASEMOTION_MODE = 0x00000004,
        STATE_BASEMOTION = 0x0000000C
    }
}
