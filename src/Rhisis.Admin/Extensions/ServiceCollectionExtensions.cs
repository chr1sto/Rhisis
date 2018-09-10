using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Rhisis.Admin.Options;

namespace Rhisis.Admin.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        internal static void ConfigureWritable<T>(this IServiceCollection services, IConfigurationSection section, string file)
            where T : class, new()
        {
            services.Configure<T>(section);
            services.AddTransient<IWritableOptions<T>>(provider =>
            {
                var environment = provider.GetService<IHostingEnvironment>();
                var options = provider.GetService<IOptionsMonitor<T>>();

                return new WritableOptions<T>(environment, options, section.Key, file);
            });
        }
    }
}
