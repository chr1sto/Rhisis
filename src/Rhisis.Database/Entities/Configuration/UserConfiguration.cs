using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Rhisis.Database.Entities.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<DbUser>
    {
        public void Configure(EntityTypeBuilder<DbUser> builder)
        {
            builder.HasIndex(c => new { c.Username, c.Email }).IsUnique();
        }
    }
}
