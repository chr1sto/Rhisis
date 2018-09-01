using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using Rhisis.Business;
using Rhisis.Database;
using System;

namespace Rhisis.Admin
{
    public class Program
    {
        public const string LogginConfigurationFile = "nlog.config";
        public const string CustomConfigurationFile = "appsettings.custom.json";

        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog(LogginConfigurationFile).GetCurrentClassLogger();

            try
            {
                logger.Trace("Starting Rhisis.Admin");
                DatabaseFactory.Instance.Setup();
                BusinessLayer.Initialize();
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception e)
            {
                logger.Error(e, "Stopped because of unhandled exception.");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                })
                .UseNLog()
                .UseStartup<Startup>();
    }
}
