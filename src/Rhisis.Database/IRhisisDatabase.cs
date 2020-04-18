using Microsoft.EntityFrameworkCore;
using Rhisis.Database.Entities;
using System;

namespace Rhisis.Database
{
    /// <summary>
    /// Provides a mechanism to access Rhisis database.
    /// </summary>
    public interface IRhisisDatabase : IDisposable
    {
        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        DbSet<DbUser> Users { get; }

        /// <summary>
        /// Gets or sets the characters.
        /// </summary>
        DbSet<DbCharacter> Characters { get; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        DbSet<DbItem> Items { get; }

        /// <summary>
        /// Gets or sets the mails.
        /// </summary>
        DbSet<DbMail> Mails { get; }

        /// <summary>
        /// Gets or sets the taskbar shortcuts.
        /// </summary>
        DbSet<DbShortcut> TaskbarShortcuts { get; }

        /// <summary>
        /// Gets or sets the quests.
        /// </summary>
        DbSet<DbQuest> Quests { get;}

        /// <summary>
        /// Gets or sets the skills.
        /// </summary>
        DbSet<DbSkill> Skills { get; }

        /// <summary>
        /// Saves the database pending changes.
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        /// <summary>
        /// Migrates the database schema.
        /// </summary>
        void Migrate();

        /// <summary>
        /// Check if the database exists.
        /// </summary>
        /// <returns></returns>
        bool Exists();

        /// <summary>
        /// CHecks if the database is alive.
        /// </summary>
        /// <returns>True if the database connection is alive; false otherwise.</returns>
        bool IsAlive();
    }
}
