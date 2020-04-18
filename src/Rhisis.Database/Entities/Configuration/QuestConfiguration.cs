using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Rhisis.Database.Entities.Configuration
{
    public class QuestConfiguration : IEntityTypeConfiguration<DbQuest>
    {
        public void Configure(EntityTypeBuilder<DbQuest> builder)
        {
            builder.HasIndex(x => new { x.QuestId, x.CharacterId }).IsUnique();
        }
    }
}
