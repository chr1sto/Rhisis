using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Configuration
{
    /// <summary>
    /// Reprensents the Login server configuration structure.
    /// </summary>
    [DataContract]
    public class LoginConfiguration
    {
        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        [DataMember(Name = "host")]
        [DefaultValue("127.0.0.1")]
        [Display(Name = "Login host address", Order = 0)]
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        [DataMember(Name = "port")]
        [DefaultValue(23000)]
        [Display(Name = "Login listening port", Order = 1)]
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the client build version.
        /// </summary>
        /// <remarks>
        /// During the authentication process, if this build version doesn't match the client build version
        /// you will not be allowed to connect to the Login Server.
        /// </remarks>
        [DataMember(Name = "buildVersion")]
        [DefaultValue("20100412")]
        [Display(Name = "Client build version", Order = 2)]
        public string BuildVersion { get; set; }

        /// <summary>
        /// Gets or sets the value if we check the account verification.
        /// </summary>
        [DataMember(Name = "accountVerification")]
        [DefaultValue(true)]
        [Display(Name = "Use account verification", Order = 3)]
        public bool AccountVerification { get; set; }

        /// <summary>
        /// Gets or sets the value if the login password is encrypted or not.
        /// </summary>
        [DataMember(Name = "passwordEncryption")]
        [DefaultValue(true)]
        [Display(Name = "Use password encryption", Order = 4)]
        public bool PasswordEncryption { get; set; }

        /// <summary>
        /// Gets or sets the password encryption key.
        /// </summary>
        [DataMember(Name = "encryptionKey")]
        [DefaultValue("dldhsvmflvm")]
        [Display(Name = "Encryption key", Order = 5)]
        public string EncryptionKey { get; set; }
    }
}
