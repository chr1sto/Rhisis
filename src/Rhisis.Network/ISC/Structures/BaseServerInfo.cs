namespace Rhisis.Network.ISC.Structures
{
    public class BaseServerInfo
    {
        public int Id { get; set; }

        public string Host { get; set; }

        public string Name { get; set; }

        public BaseServerInfo(int id, string host, string name)
        {
            this.Id = id;
            this.Host = host;
            this.Name = name;
        }
    }
}
