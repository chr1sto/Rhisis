namespace Rhisis.Core.IPC.Packets
{
    public enum InterPacketType : uint
    {
        Welcome = 0x01,
        Authentication = 0x02,
        AuthenticationResult = 0x03,
        UpdateClusterWorldsList = 0x04,
    }
}
