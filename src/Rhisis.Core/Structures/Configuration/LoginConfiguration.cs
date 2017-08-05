using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Configuration
{
    [DataContract]
    public class LoginConfiguration : BaseConfiguration
    {
        /// <summary>
        /// Gets or sets the client build version.
        /// </summary>
        /// <remarks>
        /// During the authentication process, if this build version doesn't match the client build version
        /// you will not be allowed to connect to the Login Server.
        /// </remarks>
        [DataMember(Name = "buildVersion")]
        public string BuildVersion { get; set; }

        /// <summary>
        /// Gets or sets the value if we check the account verification.
        /// </summary>
        [DataMember(Name = "accountVerification")]
        public bool AccountVerification { get; set; }

        /// <summary>
        /// Gets or sets the value if the login password is encrypted or not.
        /// </summary>
        [DataMember(Name = "passwordEncryption")]
        public bool PasswordEncryption { get; set; }

        /// <summary>
        /// Gets or sets the password encryption key.
        /// </summary>
        [DataMember(Name = "encryptionKey")]
        public string EncryptionKey { get; set; }

        /// <summary>
        /// Gets or sets the Inter-Server configuration.
        /// </summary>
        [DataMember(Name = "interServer")]
        public InterServerConfiguration InterServer { get; set; }

        /// <summary>
        /// Creates a new <see cref="LoginConfiguration"/> instance.
        /// </summary>
        public LoginConfiguration()
        {
            this.InterServer = new InterServerConfiguration();
        }
    }
}
