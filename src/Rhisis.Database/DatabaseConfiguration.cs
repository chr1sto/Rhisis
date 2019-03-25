using System.Runtime.Serialization;

namespace Rhisis.Database
{
    [DataContract]
    public class DatabaseConfiguration
    {
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
        /// Gets or sets the database encryption key.
        /// </summary>
        /// <remarks>
        /// This key will be used to encrypt every string fields of the database tables.
        /// </remarks>
        [DataMember(Name = "encryptionKey")]
        public string EncryptionKey { get; set; }

        /// <summary>
        /// Gets or sets the valid state of the configuration.
        /// </summary>
        [IgnoreDataMember]
        public bool IsValid { get; set; }

        /// <summary>
        /// Creates a new <see cref="DatabaseConfiguration"/> instance.
        /// </summary>
        public DatabaseConfiguration() { }

        /// <inheritdoc />
        public override string ToString() 
            => $"Host: {Host}, Port: {Port}, Username: {Username}, Password: {Password}, Database: {Database}, Provider: {Provider}";
    }
}
