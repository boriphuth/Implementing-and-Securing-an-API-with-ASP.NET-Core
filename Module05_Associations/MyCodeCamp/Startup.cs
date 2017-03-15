using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyCodeCamp.Data;
using Newtonsoft.Json;

namespace MyCodeCamp
{
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            _configuration = builder.Build();
        }        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add config as singleton
            services.AddSingleton(_configuration);            

            // EF Contexts + Repos
            services.AddDbContext<CampContext>(ServiceLifetime.Scoped); /* init db context for the repos */
            services.AddScoped<ICampRepository, CampRepository>(); /* scope of a request +/- */
            services.AddTransient<CampDbInitializer>(); /* seed data if necessary */

            // Automapper
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAutoMapper();

            // Add framework services.
            services.AddMvc()
                .AddJsonOptions(opt => {
                    opt.SerializerSettings.ReferenceLoopHandling =
                      ReferenceLoopHandling.Ignore;
                });
            ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            CampDbInitializer dbSeeder
        )
        {
            loggerFactory.AddConsole(_configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();                

            dbSeeder.Seed().Wait();
        }
    }
}
