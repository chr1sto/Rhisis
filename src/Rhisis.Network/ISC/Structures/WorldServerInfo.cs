namespace Rhisis.Network.ISC.Structures
{
    public class WorldServerInfo : BaseServerInfo
    {
        public int ParentClusterId { get; private set; }

        public WorldServerInfo(int id, string host, string name, int parentClusterId)
            : base(id, host, name)
        {
            ParentClusterId = parentClusterId;
        }
    }
}
