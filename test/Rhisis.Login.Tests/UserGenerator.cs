using Bogus;
using Rhisis.Database.Entities;
using System.Collections.Generic;

namespace Rhisis.Login.Tests
{
    public class UserGenerator : Faker<DbUser>
    {
        public UserGenerator()
        {
            RuleFor(x => x.Username, x => x.Internet.UserName())
                .RuleFor(x => x.Password, x => x.Internet.Password());
        }

        public override List<DbUser> Generate(int count, string ruleSets = null)
        {
            List<DbUser> users = base.Generate(count, ruleSets);

            users.Add(new DbUser
            {
                Username = "Rhisis",
                Password = "CorrectPassword",
                IsDeleted = false
            });
            users.Add(new DbUser
            {
                Username = "Bubble",
                Password = "randomPassword",
                IsDeleted = false
            });
            users.Add(new DbUser
            {
                Username = "Shade",
                Password = "password",
                IsDeleted = true
            });

            return users;
        }
    }
}
