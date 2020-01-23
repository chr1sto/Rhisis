namespace Rhisis.Network.ISC.Structures
{
    public class BaseServerInfo
    {
        public int Id { get; set; }

        public string Host { get; set; }

        public string Name { get; set; }

        public BaseServerInfo(int id, string host, string name)
        {
            Id = id;
            Host = host;
            Name = name;
        }
    }
}
