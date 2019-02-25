namespace Rhisis.Core.Services
{
    public interface IStatisticsService
    {
        /// <summary>
        /// Gets the number of registered Users.
        /// </summary>
        /// <returns></returns>
        int GetRegisteredUsers();

        /// <summary>
        /// Gets the number of characters.
        /// </summary>
        /// <returns></returns>
        int GetNumberOfCharacters();

        /// <summary>
        /// Gets the number of online players.
        /// </summary>
        /// <returns></returns>
        int GetOnlinePlayers();
    }
}
