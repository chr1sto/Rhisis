using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using Rhisis.Business;
using Rhisis.Cluster.Client;
using Rhisis.Cluster.ISC;
using Rhisis.Core;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Helpers;
using Rhisis.Core.Resources;
using Rhisis.Core.Resources.Loaders;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Database;
using Rhisis.Network;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Rhisis.Cluster
{
    public class ClusterServerStartup : IProgramStartup
    {
        private const string ClusterConfigFile = "config/cluster.json";
        private const string DatabaseConfigFile = "config/database.json";

        private readonly IEnumerable<Type> _loaders = new[]
        {
            typeof(DefineLoader),
            typeof(JobLoader),
        };

        private IClusterServer _server;

        /// <inheritdoc />
        public void Configure()
        {
            GameResources.Instance.Initialize(this._loaders);
            DependencyContainer.Instance.BuildServiceProvider();
        }

        /// <inheritdoc />
        public void Run()
        {

            GameResources.Instance.Load();

        }

        /// <inheritdoc />
        public void Dispose()
        {
            this._server?.Dispose();
            DependencyContainer.Instance.Dispose();
            LogManager.Shutdown();
        }
    }
}