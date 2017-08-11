using System.Runtime.Serialization;

namespace Rhisis.Database
{
    [DataContract]
    public class DatabaseConfiguration
    {
        [DataMember(Name = "host")]
        public string Host { get; set; }

        [DataMember(Name = "port")]
        public int Port { get; set; }

        [DataMember(Name = "username")]
        public string Username { get; set; }

        [DataMember(Name = "password")]
        public string Password { get; set; }

        [DataMember(Name = "database")]
        public string Database { get; set; }

        [DataMember(Name = "provider")]
        public DatabaseProvider Provider { get; set; }
    }

    public enum DatabaseProvider
    {
        Unknown = 0,
        MySQL,
        MsSQL,
        PostgreSQL,
        SQLite,
    }
}
