using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Rhisis.Database;
using System.Collections.Generic;
using System.IO;

namespace Rhisis.Business.Tests
{
    /// <summary>
    /// Provides basics methods and properties for a service test.
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public abstract class ServiceTestBase<TService> where TService : class
    {
        /// <summary>
        /// Gets the database mock.
        /// </summary>
        protected Mock<IDatabase> DatabaseMock { get; }

        /// <summary>
        /// Gets the logger mock.
        /// </summary>
        protected Mock<ILogger<TService>> Logger { get; }
        
        /// <summary>
        /// Creates a new <see cref="ServiceTestBase{TService}"/> instance.
        /// </summary>
        protected ServiceTestBase()
        {
            this.DatabaseMock = new Mock<IDatabase>();
            this.Logger = new Mock<ILogger<TService>>();
        }

        /// <summary>
        /// Loads a collection of a given type from a json file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected IList<T> LoadCollectionFromJson<T>(string fileName) 
            => JsonConvert.DeserializeObject<IList<T>>(File.ReadAllText(fileName));
    }
}
