using System;
using Microsoft.EntityFrameworkCore;

namespace Rhisis.Database.Contexts
{
    internal class MsSQLContext : DatabaseContext
    {
        public MsSQLContext(DatabaseConfiguration configuration) 
            : base(configuration)
        {
        }

        public override void Migrate()
        {
            throw new NotImplementedException();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // TODO: Configure MsSQL context.
            base.OnConfiguring(optionsBuilder);
        }
    }
}
