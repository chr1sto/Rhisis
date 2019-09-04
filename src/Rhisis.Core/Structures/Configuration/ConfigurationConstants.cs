namespace Rhisis.Core.Structures.Configuration
{
    /// <summary>
    /// Contains contants related to the configuration.
    /// </summary>
    public sealed class ConfigurationConstants
    {
        /// <summary>
        /// Gets the CoreServer configuration key.
        /// </summary>
        public const string CoreServer = "core";

        /// <summary>
        /// Gets the LoginServer configuration key.
        /// </summary>
        public const string LoginServer = "loginServer";

        /// <summary>
        /// Gets the LoginServer configuration file path.
        /// </summary>
        public const string LoginServerPath = "config/login.json";

        /// <summary>
        /// Gets the ClusterServer configuraiton key.
        /// </summary>
        public const string ClusterServer = "clusterServer";

        /// <summary>
        /// Gets the ClusterServer configuration file path.
        /// </summary>
        public const string ClusterServerPath = "config/cluster.json";

        /// <summary>
        /// Gets the WorldServer configuration key.
        /// </summary>
        public const string WorldServer = "worldServer";

        /// <summary>
        /// Gets the WorldServer configuration file path.
        /// </summary>
        public const string WorldServerPath = "config/world.json";

        /// <summary>
        /// Gets the Database configuration file path.
        /// </summary>
        public const string DatabasePath = "config/database.json";

        /// <summary>
        /// Prevents from creating a <see cref="ConfigurationConstants"/> instance from outside.
        /// </summary>
        private ConfigurationConstants()
        {
        }
    }
}
