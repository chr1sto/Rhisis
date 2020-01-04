using System;
using System.Threading.Tasks;
using Rhisis.Database.Context;
using Rhisis.Database.Repositories;

namespace Rhisis.Database
{
    public interface IDatabase : IDisposable
    {
        /// <summary>
        /// Gets the database context used.
        /// </summary>
        DatabaseContext DatabaseContext { get; }

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
        /// Gets the quests.
        /// </summary>
        IQuestRepository Quests { get; }

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