using System.ComponentModel;
using System.Runtime.Serialization;

namespace Rhisis.Database
{
    [DataContract]
    public class DatabaseConfiguration
    {
        [DataMember(Name = "host")]
        [DefaultValue("127.0.0.1")]
        public string Host { get; set; }

        [DataMember(Name = "port")]
        [DefaultValue("3306")]
        public int Port { get; set; }

        [DataMember(Name = "username")]
        [DefaultValue("root")]
        public string Username { get; set; }

        [DataMember(Name = "password")]
        [DefaultValue("")]
        public string Password { get; set; }

        [DataMember(Name = "database")]
        [DefaultValue("rhisis")]
        public string Database { get; set; }

        [DataMember(Name = "provider")]
        [DefaultValue(DatabaseProvider.MySql)]
        public DatabaseProvider Provider { get; set; }

        [IgnoreDataMember]
        public bool IsValid { get; set; }
    }
}
