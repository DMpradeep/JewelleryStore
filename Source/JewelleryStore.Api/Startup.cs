using JewelleryStore.Api.Authorization;
using JewelleryStore.Api.Filters;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace JewelleryStore.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            DependencyConfiguration.AddDependency(services, Configuration);

            services
              .AddCors()
              .AddCustomAuthorizationPolicy()
              .AddMvc(options =>
              {
                  options.Filters.Add(typeof(CustomExceptionFilterAttribute));
                  options.EnableEndpointRouting = false;
              })
              .SetCompatibilityVersion(CompatibilityVersion.Latest);

            services
                .AddHttpClient()
                .AddLogging(builder =>
                {
                    builder
                    .AddConsole()
                    .AddDebug();
                });

            services
                .AddHttpContextAccessor()
                .AddSwaggerGen(c => c.SwaggerDoc("v2", new OpenApiInfo { Title = "Jewellery REST API", Version = Process.GetCurrentProcess().MainModule.FileVersionInfo.FileVersion }));

            var assemblies = new List<Assembly> {
                typeof(Application.User.UserDetailsQuery).Assembly
            };

            services.AddMediatR(assemblies.ToArray());

            ConfigureAuth(services);
        }

        private static void ConfigureAuth(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
            })
            .AddJwtBearer("JwtBearer", jwtBeareOptions =>
            {
                jwtBeareOptions.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("JewelleryStoreSecret")),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(5)
                };
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("./v2/swagger.json", "Jewellery REST API"));

            app.UseRouting();
            app.UseCors(builder => builder.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader());
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMvc();
        }
    }
}
