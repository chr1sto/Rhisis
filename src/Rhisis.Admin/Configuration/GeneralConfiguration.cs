using System.Runtime.Serialization;

namespace Rhisis.Admin.Configuration
{
    [DataContract]
    public class GeneralConfiguration
    {
        /// <summary>
        /// Gets or sets a value that indicates if the application has been installed correctly.
        /// </summary>
        [DataMember(Name = "isInstalled")]
        public bool IsInstalled { get; set; }

        /// <summary>
        /// Creates a new <see cref="GeneralConfiguration"/> instance.
        /// </summary>
        public GeneralConfiguration()
        {
            this.IsInstalled = false;
        }
    }
}
