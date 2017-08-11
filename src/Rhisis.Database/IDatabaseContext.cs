using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.Database
{
    public interface IDatabaseContext
    {
        void Initialize();

        void Migrate();
    }
}
