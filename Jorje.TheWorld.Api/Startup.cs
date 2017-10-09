using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Jorje.TheWorld.Dal.Context;
using Jorje.TheWorld.Bll.IBusiness;
using Jorje.TheWorld.Bll.Business;
using Jorje.TheWorld.Dal.IRepositories;
using Jorje.TheWorld.Dal.Repositories;
using Jorje.TheWorld.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Jorje.TheWorld.Api.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Jorje.TheWorld.Bll.Mappers;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.AspNetCore.Mvc.Formatters;

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

            services.Configure<JWTSettings>(Configuration.GetSection("JWTSettings"));

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
                config.ReturnHttpNotAcceptable = true;
                config.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
            });

            services.AddIdentity<WorldUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 8;
                config.Cookies.ApplicationCookie.LoginPath = "/Account/Login";

            }).AddEntityFrameworkStores<WorldDBContext>();

            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.AppendTrailingSlash = true;

            });

            services.Configure<IdentityOptions>(options =>
            {
                // avoid redirecting REST clients on 401
                options.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = ctx =>
                    {
                        ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        return Task.FromResult(0);
                    }
                };
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
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("Unexpected fault happened. Try again later.");
                    });
                });
            }

            app.UseStaticFiles();

            app.UseIdentity();

            // secretKey contains a secret passphrase only your server knows
            var secretKey = Configuration.GetSection("JWTSettings:SecretKey").Value;
            var issuer = Configuration.GetSection("JWTSettings:Issuer").Value;
            var audience = Configuration.GetSection("JWTSettings:Audience").Value;
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = issuer,

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = audience
            };
            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                TokenValidationParameters = tokenValidationParameters
            });

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AutomaticAuthenticate = false,
                AutomaticChallenge = false
            });

            AutoMapperContainer.Initialize();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
