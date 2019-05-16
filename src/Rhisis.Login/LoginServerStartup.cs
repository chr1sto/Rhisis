using System;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using Rhisis.Business;
using Rhisis.Core;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Helpers;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Database;
using Rhisis.Network;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Rhisis.Login
{
    public sealed class LoginServerStartup : IProgramStartup
    {
        private const string LoginConfigFile = "config/login.json";
        private const string DatabaseConfigFile = "config/database.json";

        private ILoginServer _server;

        /// <inheritdoc />
        public void Configure()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            PacketHandler<LoginClient>.Initialize();
            
            var dbConfig = ConfigurationHelper.Load<DatabaseConfiguration>(DatabaseConfigFile);
            DependencyContainer.Instance
                .GetServiceCollection()
                .RegisterDatabaseServices(dbConfig);
            
            BusinessLayer.Initialize();
            DependencyContainer.Instance.Register<ILoginServer, LoginServer>(ServiceLifetime.Singleton);
            DependencyContainer.Instance.Configure(services => services.AddLogging(builder =>
            {
                builder.AddFilter("Microsoft", LogLevel.Warning);
                builder.SetMinimumLevel(LogLevel.Trace);
                builder.AddNLog(new NLogProviderOptions
                {
                    CaptureMessageTemplates = true,
                    CaptureMessageProperties = true
                });
            }));
            DependencyContainer.Instance.Configure(services =>
            {
                var worldConfiguration = ConfigurationHelper.Load<LoginConfiguration>(LoginConfigFile, true);
                services.AddSingleton(worldConfiguration);
            });
            DependencyContainer.Instance.BuildServiceProvider();
        }

        /// <inheritdoc />
        public void Run()
        {
            var logger = DependencyContainer.Instance.Resolve<ILogger<LoginServerStartup>>();
            this._server = DependencyContainer.Instance.Resolve<ILoginServer>();

            try
            {
                logger.LogInformation("Starting LoginServer...");
                
                this._server.Start();
            }
            catch (Exception e)
            {
                logger.LogCritical(e, $"An unexpected error occured in LoginServer.");
#if DEBUG
                Console.ReadLine();
#endif
            }
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