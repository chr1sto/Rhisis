using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rhisis.Business;
using Rhisis.Core.DependencyInjection;
using Rhisis.Database;

namespace Rhisis.API
{
    public class Startup
    {
        /// <summary>
        /// Gets the API configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Creates a new <see cref="Startup"/> class instance.
        /// </summary>
        /// <param name="configuration">Configuration</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("DevAllowAllOrigins", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
                });
            });
            services.AddMvc();

            var databaseConfiguration = this.Configuration.GetSection(nameof(DatabaseConfiguration)).Get<DatabaseConfiguration>();

            DependencyContainer.Instance.SetServiceCollection(services);
            BusinessLayer.Initialize();
            services.AddSingleton(Configuration);

            services.RegisterDatabaseServices(databaseConfiguration);
            
            DependencyContainer.Instance.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("DevAllowAllOrigins");
            }

            app.UseMvc();
        }
    }
}
