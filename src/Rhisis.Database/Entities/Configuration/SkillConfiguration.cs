using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Rhisis.Database.Entities.Configuration
{
    public class SkillConfiguration : IEntityTypeConfiguration<DbSkill>
    {
        public void Configure(EntityTypeBuilder<DbSkill> builder)
        {
            builder.HasIndex(x => new { x.SkillId, x.CharacterId }).IsUnique();
        }
    }
}
