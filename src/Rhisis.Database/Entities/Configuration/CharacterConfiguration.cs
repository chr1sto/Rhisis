using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Rhisis.Database.Entities.Configuration
{
    public class CharacterConfiguration : IEntityTypeConfiguration<DbCharacter>
    {
        public void Configure(EntityTypeBuilder<DbCharacter> builder)
        {
            builder.HasMany(x => x.ReceivedMails).WithOne(x => x.Receiver);
            builder.HasMany(x => x.SentMails).WithOne(x => x.Sender);
        }
    }
}
