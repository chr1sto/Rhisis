using Rhisis.Database.Exceptions;
using System;
using Xunit;

namespace Rhisis.Database.Test
{
    public class DatabaseConfigurationTest
    {
        [Fact]
        public void DatabaseConfigurationSuccess()
        {
            DatabaseService.UnloadConfiguration();
            DatabaseService.Configure("localhsot", 3306, "root", "", "rhisis", DatabaseProvider.Unknown);
        }

        [Fact]
        public void DatabaseConfiguredTwice()
        {
            DatabaseService.UnloadConfiguration();
            DatabaseService.Configure("localhsot", 3306, "root", "", "rhisis", DatabaseProvider.Unknown);

            var exception = Record.Exception(() => DatabaseService.Configure("localhsot", 3306, "root", "", "rhisis", DatabaseProvider.Unknown));

            Assert.IsType(typeof(RhisisDatabaseConfigurationException), exception);
        }

        [Fact]
        public void DatbaseGetContextFailed()
        {
            DatabaseService.UnloadConfiguration();
            var exception = Record.Exception(() => DatabaseService.GetContext());

            Assert.IsType(typeof(RhisisDatabaseConfigurationException), exception);
        }

    }
}
