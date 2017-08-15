namespace Rhisis.Core.ISC.Packets
{
    public enum InterPacketType : uint
    {
        WELCOME = 0x01,
        AUTHENTICATE = 0x02,
        AUTHENTICATION_RESULT = 0x03,
    }
}
