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
using Microsoft.AspNetCore.Diagnostics;
using NLog.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Jorje.TheWorld.Common.Helpers.Sorting;
using Jorje.TheWorld.Dal.Sort;
using Newtonsoft.Json.Serialization;
using Jorje.TheWorld.Common.Services.Contract;
using Jorje.TheWorld.Common.Services;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Jorje.TheWorld.Api.Filters.Jorje.TheWorld.Api.Filters;

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

            InjectRepositoryLayer(services);
            InjectBusinessLayer(services);

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper, UrlHelper>(implementationFactory =>
            {
                var actionContext = implementationFactory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });
            services.AddTransient<ITypeHelperService, TypeHelperService>();

            services.AddLogging();

            //var builder = new ContainerBuilder();
            //var container = builder.Build();

            // Add Cors
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            // Add framework services.
            services.AddMvc(config =>
            {
                //config.Filters.Add(new EnvironmentFilter(container));
                config.ReturnHttpNotAcceptable = true;
                config.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                config.InputFormatters.Add(new XmlDataContractSerializerInputFormatter());

                var jsonOutputFormatter = config.OutputFormatters
                    .OfType<JsonOutputFormatter>().FirstOrDefault();

                if (jsonOutputFormatter != null)
                {
                    jsonOutputFormatter.SupportedMediaTypes.Add("application/hateoas+json");
                }
            })
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Trips API", Version = "v1" });
                // Define Jwt Bearer Options
                //c.AddSecurityDefinition("bearer", new ApiKeyScheme()
                //{
                //    Type = "apiKey",
                //    In = "header",
                //    Name = "Authorization"
                //});

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    Description = "Authorization header using the Bearer scheme",
                    Name = "Authorization",
                    In = "header"
                });

                c.DocumentFilter<SwaggerSecurityRequirementsDocumentFilter>();
            });

            services.AddIdentity<TripIdentityUser, IdentityRole>(config =>
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
                        return Task.CompletedTask;
                    },
                    OnRedirectToAccessDenied = ctx =>
                    {
                        ctx.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug(LogLevel.Information); //debug window
            //loggerFactory.AddProvider(new NLog.Extensions.Logging.NLogLoggerProvider());
            loggerFactory.AddNLog();

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
                    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (exceptionHandlerFeature != null)
                    {
                        var logger = loggerFactory.CreateLogger("Global exception logger");
                        logger.LogError(500, exceptionHandlerFeature.Error, exceptionHandlerFeature.Error.Message);
                        }
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("Unexpected fault happened. Try again later.");
                    });
                });
            }

            app.UseStaticFiles();

            app.UseIdentity();

            app.UseJwtBearerAuthentication(new JwtBearerOptions()
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = Configuration.GetSection("JWTSettings:Issuer").Value,
                    ValidAudience = Configuration.GetSection("JWTSettings:Audience").Value,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("JWTSettings:SecretKey").Value)),
                    ValidateLifetime = true
                }
            });

            //// secretKey contains a secret passphrase only your server knows
            //var secretKey = Configuration.GetSection("JWTSettings:SecretKey").Value;
            //var issuer = Configuration.GetSection("JWTSettings:Issuer").Value;
            //var audience = Configuration.GetSection("JWTSettings:Audience").Value;
            //var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            //var tokenValidationParameters = new TokenValidationParameters
            //{
            //    ValidateIssuerSigningKey = true,
            //    IssuerSigningKey = signingKey,

            //    // Validate the JWT Issuer (iss) claim
            //    ValidateIssuer = true,
            //    ValidIssuer = issuer,

            //    // Validate the JWT Audience (aud) claim
            //    ValidateAudience = true,
            //    ValidAudience = audience
            //};
            //app.UseJwtBearerAuthentication(new JwtBearerOptions
            //{
            //    AutomaticAuthenticate = true,
            //    AutomaticChallenge = true,
            //    TokenValidationParameters = tokenValidationParameters
            //});

            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    AutomaticAuthenticate = true,
            //    AutomaticChallenge = true
            //});

            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    AuthenticationScheme = "Cookies",
            //    AutomaticAuthenticate = true,
            //    AutomaticChallenge = true
            //});

            //app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions
            //{
            //    ClientId = "41326939717-76coj8vvak5t3e4ietrj0eirg3m1ginv.apps.googleusercontent.com",
            //    ClientSecret = "VU3yjPIPN0OEMEYTMm8nyncO",
            //    Authority = "https://accounts.google.com",
            //    ResponseType = OpenIdConnectResponseType.Code,
            //    GetClaimsFromUserInfoEndpoint = true,
            //    SaveTokens = true,
            //    Events = new OpenIdConnectEvents()
            //    {
            //        OnRedirectToIdentityProvider = (context) =>
            //        {
            //            if (context.Request.Path != "/api/values/external")
            //            {
            //                context.Response.Redirect("/api/values/external");
            //                context.HandleResponse();
            //            }

            //            return Task.FromResult(0);
            //        }
            //    }
            //});

            AutoMapperContainer.Initialize();

            // Enable Cors
            app.UseCors("MyPolicy");

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Trip API V1");
                c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void InjectBusinessLayer(IServiceCollection services)
        {
            services.AddScoped<IStopBus, StopBus>();
            services.AddScoped<ITripBus, TripBus>();
            services.AddScoped<IPersonBus, PersonBus>();
        }

        private void InjectRepositoryLayer(IServiceCollection services)
        {
            services.AddScoped<IStopRepository, StopRepository>();
            services.AddScoped<ITripRepository, TripRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
        }
    }
}
