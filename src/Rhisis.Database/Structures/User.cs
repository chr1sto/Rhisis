using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.Database.Structures
{
    public sealed class User
    {
        public int ID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public int Authority { get; set; }
    }
}
