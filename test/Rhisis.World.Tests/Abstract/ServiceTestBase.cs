using Bogus;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.World.Tests.Abstract
{
    public class ServiceTestBase<TService> where TService : class
    {
        protected Mock<ILogger<TService>> LoggerMock { get; }

        protected TService Service { get; set; }

        protected Faker Faker { get; }

        /// <summary>
        /// Creates a new <see cref="ServiceTestBase{TService}"/>.
        /// </summary>
        protected ServiceTestBase()
        {
            LoggerMock = new Mock<ILogger<TService>>();
            Faker = new Faker();
        }
    }
}
