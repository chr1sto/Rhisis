using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Rhisis.Database
{
    [DataContract]
    public sealed class DatabaseConfiguration
    {
        /// <summary>
        /// Gets or sets the database host.
        /// </summary>
        [DataMember(Name = "host")]
        [DefaultValue("127.0.0.1")]
        [Display(Name = "Database server host address", Order = 0)]
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the database port.
        /// </summary>
        [DataMember(Name = "port")]
        [DefaultValue(3306)]
        [Display(Name = "Database server listening port", Order = 1)]
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the database connection username.
        /// </summary>
        [DataMember(Name = "username")]
        [DefaultValue("root")]
        [Display(Name = "Database username", Order = 2)]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the database connection password
        /// </summary>
        [DataMember(Name = "password")]
        [Display(Name = "Database user password", Order = 3)]
        [PasswordPropertyText]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the database name.
        /// </summary>
        [DataMember(Name = "database")]
        [DefaultValue("rhisis")]
        [Display(Name = "Database name", Order = 4)]
        public string Database { get; set; }

        /// <summary>
        /// Gets or sets the database encryption key.
        /// </summary>
        /// <remarks>
        /// This key will be used to encrypt every string fields of the database tables.
        /// </remarks>
        [DataMember(Name = "encryptionKey")]
        [NotMapped]
        public string EncryptionKey { get; set; }

        /// <summary>
        /// Gets or sets the valid state of the configuration.
        /// </summary>
        [IgnoreDataMember]
        [NotMapped]
        public bool IsValid { get; set; }

        /// <inheritdoc />
        public override string ToString() 
            => $"Host: {Host}, Port: {Port}, Username: {Username}, Password: {Password}, Database: {Database}";
    }
}
