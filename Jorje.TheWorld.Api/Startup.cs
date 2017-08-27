using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Jorje.TheWorld.Dal.Context;
using Jorje.TheWorld.Bll.IBusiness;
using Jorje.TheWorld.Bll.Business;
using Jorje.TheWorld.Dal.IRepositories;
using Jorje.TheWorld.Dal.Repositories;

namespace Jorje.TheWorld.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Environment = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IHostingEnvironment Environment { get; }
        public IConfigurationRoot Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);

            if (Environment.IsEnvironment("Development") || Environment.IsEnvironment("Testing"))
            {
                //services.AddScoped<IMailService, DebugMailService>();
            }
            else
            {
                // Implement a real Mail Service
            }

            services.AddDbContext<WorldDBContext>();

            services.AddScoped<IStopRepository, StopRepository>();
            services.AddScoped<IStopBus, StopBus>();


            services.AddLogging();

            //var builder = new ContainerBuilder();
            //var container = builder.Build();

            // Add framework services.
            services.AddMvc(config =>
            {
                //config.Filters.Add(new EnvironmentFilter(container));
            });

            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.AppendTrailingSlash = true;

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
    //public class Startup
    //{
    //    public Startup(IHostingEnvironment env)
    //    {
    //        var builder = new ConfigurationBuilder()
    //            .SetBasePath(env.ContentRootPath)
    //            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    //            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
    //            .AddEnvironmentVariables();
    //        Configuration = builder.Build();
    //    }

    //    public IConfigurationRoot Configuration { get; }

    //    // This method gets called by the runtime. Use this method to add services to the container.
    //    public void ConfigureServices(IServiceCollection services)
    //    {
    //        // Add framework services.
    //        services.AddMvc();
    //    }

    //    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    //    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    //    {
    //        loggerFactory.AddConsole(Configuration.GetSection("Logging"));
    //        loggerFactory.AddDebug();

    //        app.UseMvc();
    //    }
    //}
}
