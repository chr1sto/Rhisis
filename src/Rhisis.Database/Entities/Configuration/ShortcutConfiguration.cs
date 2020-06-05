using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Rhisis.Database.Entities.Configuration
{
    public class ShortcutConfiguration : IEntityTypeConfiguration<DbShortcut>
    {
        public void Configure(EntityTypeBuilder<DbShortcut> builder)
        {
            builder.ToTable("shortcuts");

            builder.Property(x => x.TargetTaskbar)
                .HasColumnType("TINYINT");

            builder.Property(x => x.Type)
                .HasColumnType("TINYINT");

            builder.Property(x => x.ObjectType)
                .HasColumnType("TINYINT");

            builder.Property(x => x.Slot)
                .IsRequired()
                .HasColumnType("TINYINT");

            builder.Property(x => x.SlotLevelIndex)
                .IsRequired()
                .HasColumnType("SMALLINT");

            builder.Property(x => x.ObjectItemSlot)
                .IsRequired(false)
                .HasColumnType("SMALLINT");

            builder.Property(x => x.ObjectIndex)
                .IsRequired()
                .HasColumnType("TINYINT");

            builder.Property(x => x.UserId)
                .IsRequired()
                .HasColumnType("SMALLINT");

            builder.Property(x => x.ObjectData)
                .IsRequired()
                .HasColumnType("SMALLINT");

            builder.Property(x => x.Text)
                .IsRequired()
                .HasColumnType("TEXT");

            builder.HasKey(x => new { x.CharacterId, x.Slot, x.SlotLevelIndex });

            builder.HasIndex(x => new { x.CharacterId, x.Slot, x.SlotLevelIndex })
                .IsUnique();

            builder.Property(x => x.CharacterId)
                .IsRequired();
            builder.HasOne(x => x.Character)
                .WithMany(x => x.TaskbarShortcuts)
                .HasForeignKey(x => x.CharacterId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
