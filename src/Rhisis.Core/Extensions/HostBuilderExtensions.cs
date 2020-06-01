using Microsoft.Extensions.Hosting;
using System.Globalization;
using System.Text;

namespace Rhisis.Core.Extensions
{
    /// <summary>
    /// Provides extensions for the <see cref="IHostBuilder"/> interface.
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Sets the console culture.
        /// </summary>
        /// <param name="hostBuilder">Current host builder.</param>
        /// <param name="cultureName">Culture name.</param>
        /// <returns>Configured host builder.</returns>
        public static IHostBuilder SetConsoleCulture(this IHostBuilder hostBuilder, string cultureName)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            CultureInfo.CurrentCulture = new CultureInfo(cultureName);
            CultureInfo.CurrentUICulture = new CultureInfo(cultureName);
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(cultureName);
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(cultureName);

            return hostBuilder;
        }
    }
}
