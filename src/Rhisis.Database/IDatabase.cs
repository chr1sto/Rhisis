using Rhisis.Database.Repositories;
using System;
using System.Threading.Tasks;

namespace Rhisis.Database
{
    public interface IDatabase : IDisposable
    {
        /// <summary>
        /// Gets the users.
        /// </summary>
        IUserRepository Users { get; set; }

        /// <summary>
        /// Gets the characters.
        /// </summary>
        ICharacterRepository Characters { get; set; }

        /// <summary>
        /// Gets the items.
        /// </summary>
        IItemRepository Items { get; set; }

        /// <summary>
        /// Gets the mails.
        /// </summary>
        IMailRepository Mails { get; set; }

        /// <summary>
        /// Gets the taskbar shortcuts.
        /// </summary>
        IShortcutRepository TaskbarShortcuts { get; }

        /// <summary>
        /// Complete the pending database operations.
        /// </summary>
        void Complete();

        /// <summary>
        /// Complete the pending database operations in an asynchronous context.
        /// </summary>
        /// <returns></returns>
        Task CompleteAsync();
    }
}
