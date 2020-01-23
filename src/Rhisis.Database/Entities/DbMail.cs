using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rhisis.Database.Entities
{
    [Table("mails")]
    public sealed class DbMail : DbEntity
    {
        [Required]
        [Encrypted]
        public string Title { get; set; }

        [Encrypted]
        public string Text { get; set; }

        [Column(TypeName = "BIGINT")]
        public uint Gold { get; set; }

        public int? ItemId { get; set; }

        [ForeignKey(nameof(ItemId))]
        public DbItem Item { get; set; }

        public short ItemQuantity { get; set; }

        public bool HasBeenRead { get; set; }

        public bool HasReceivedItem { get; set; }

        public bool HasReceivedGold { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateTime { get; set; }
        
        public int SenderId { get; set; }

        [ForeignKey(nameof(SenderId))]
        public DbCharacter Sender { get; set; }

        public int ReceiverId { get; set; }

        [ForeignKey(nameof(ReceiverId))]
        public DbCharacter Receiver { get; set; }

        public DbMail()
        {
            HasBeenRead = false;
            HasReceivedGold = false;
            HasReceivedItem = false;
            IsDeleted = false;
            CreateTime = DateTime.UtcNow;
        }

        public DbMail(DbCharacter sender, DbCharacter receiver, string title, string text)
            : this(sender, receiver, 0, null, 0, title, text, false, false, false, false, DateTime.UtcNow)
        {
        }

        public DbMail(DbCharacter sender, DbCharacter receiver, DbItem item, short itemQuantity, string title, string text)
            : this(sender, receiver, 0, item, itemQuantity, title, text, false, false, false, false, DateTime.UtcNow)
        {
        }

        public DbMail(DbCharacter sender, DbCharacter receiver, uint gold, string title, string text)
            : this(sender, receiver, gold, null, 0, title, text, false, false, false, false, DateTime.UtcNow)
        {
        }

        public DbMail(DbCharacter sender, DbCharacter receiver, uint gold, DbItem item, short itemQuantity, string title, string text)
            : this(sender, receiver, gold, item, itemQuantity, title, text, false, false, false, false, DateTime.UtcNow)
        {
        }

        public DbMail(DbCharacter sender, DbCharacter receiver, uint gold, DbItem item, short itemQuantity, string title, string text, bool hasBeenRead, bool hasReceivedItem, bool hasReceivedGold, bool isDeleted, DateTime createTime)
        {
            Sender = sender;
            Receiver = receiver;
            Gold = gold;
            Item = item;
            ItemQuantity = itemQuantity;
            Title = title;
            Text = text;
            HasBeenRead = hasBeenRead;
            IsDeleted = isDeleted;
            HasReceivedItem = hasReceivedItem;
            HasReceivedGold = hasReceivedGold;
            CreateTime = createTime;
        }
    }
}
