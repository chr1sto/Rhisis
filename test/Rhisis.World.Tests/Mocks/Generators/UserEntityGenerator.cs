using Bogus;
using Rhisis.Database.Entities;

namespace Rhisis.World.Tests.Mocks.Database.Entities
{
    internal class UserEntityGenerator : Faker<DbUser>
    {
        public UserEntityGenerator()
        {
            RuleFor(x => x.Username, (faker, prop) => faker.Internet.UserName())
                .RuleFor(x => x.Password, (faker, prop) => faker.Internet.Password())
                .RuleFor(x => x.Email, (faker, prop) => faker.Internet.Email())
                .RuleFor(x => x.Authority, (faker, prop) => 80);
        }
    }
}
