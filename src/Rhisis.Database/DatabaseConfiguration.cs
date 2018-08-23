using System.Runtime.Serialization;

namespace Rhisis.Database
{
    [DataContract]
    public class DatabaseConfiguration
    {
        private const string DefaultHost = "127.0.0.1";
        private const int DefaultPort = 3306;
        private const string DefaultUsername = "root";
        private const string DefaultPassword = null;
        private const string DefaultDatabaseName = "rhisis";
        private const DatabaseProvider DefaultDatabaseProvider = DatabaseProvider.MySql;

        /// <summary>
        /// Gets or sets the database host.
        /// </summary>
        [DataMember(Name = "host")]
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the database port.
        /// </summary>
        [DataMember(Name = "port")]
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the database connection username.
        /// </summary>
        [DataMember(Name = "username")]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the database connection password
        /// </summary>
        [DataMember(Name = "password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the database name.
        /// </summary>
        [DataMember(Name = "database")]
        public string Database { get; set; }

        /// <summary>
        /// Gets or sets the database provider.
        /// </summary>
        [DataMember(Name = "provider")]
        public DatabaseProvider Provider { get; set; }

        /// <summary>
        /// Gets or sets the valid state of the configuration.
        /// </summary>
        [IgnoreDataMember]
        public bool IsValid { get; set; }

        /// <summary>
        /// Creates a new <see cref="DatabaseConfiguration"/> instance.
        /// </summary>
        public DatabaseConfiguration()
            : this(false)
        {
        }

        /// <summary>
        /// Creates a new <see cref="DatabaseConfiguration"/> instance.
        /// </summary>
        /// <param name="initializeDefaultValues">Value that indicates if we want default values</param>
        public DatabaseConfiguration(bool initializeDefaultValues)
        {
            if (initializeDefaultValues)
            {
                this.Host = DefaultHost;
                this.Port = DefaultPort;
                this.Username = DefaultUsername;
                this.Password = DefaultPassword;
                this.Database = DefaultDatabaseName;
                this.Provider = DefaultDatabaseProvider;
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"Host: {Host}, Port: {Port}, Username: {Username}, Password: {Password}, Database: {Database}, Provider: {Provider}";
        }
    }
}
