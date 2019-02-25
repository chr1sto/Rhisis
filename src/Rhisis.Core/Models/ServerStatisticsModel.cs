using System.Runtime.Serialization;

namespace Rhisis.Core.Models
{
    [DataContract]
    public class ServerStatisticsModel
    {
        /// <summary>
        /// Gets the number of users.
        /// </summary>
        [DataMember]
        public int NumberOfUsers { get; set; }

        /// <summary>
        /// Gets the number of characters.
        /// </summary>
        [DataMember]
        public int NumberOfCharacters { get; set; }

        /// <summary>
        /// Gets the number of connected players.
        /// </summary>
        [DataMember]
        public int NumberOfPlayersOnline { get; set; }
    }
}
