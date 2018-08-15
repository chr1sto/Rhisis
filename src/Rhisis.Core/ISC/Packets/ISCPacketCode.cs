namespace Rhisis.Core.ISC.Packets
{
    public enum ISCPacketCode : uint
    {
        AUTH_SUCCESS = 0,
        AUTH_FAILED_UNKNOWN_SERVER,
        AUTH_FAILED_CLUSTER_EXISTS,
        AUTH_FAILED_NO_CLUSTER,
        AUTH_FAILED_WORLD_EXISTS
    }
}
