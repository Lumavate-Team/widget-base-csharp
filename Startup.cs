using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Lumavate.Models;
using Lumavate.Common;

namespace Lumavate
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        private IHostingEnvironment HostingEnvironment { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()  
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            this.HostingEnvironment = env;        
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<LumavateContext>(opt => opt.UseInMemoryDatabase("Properties"));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.Configure<EnvironmentConfig>(this.Configuration);
             // configure jwt authentication
            //var key = "";
            //services.AddAuthentication(x =>
            //{
            //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(x =>
            //{
                // x.Events = new JwtBearerEvents
                // {
                //     OnTokenValidated = context =>
                //     {
                //         var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                //         var userId = int.Parse(context.Principal.Identity.Name);
                //         var user = userService.GetById(userId);
                //         if (user == null)
                //         {
                //             // return unauthorized if user no longer exists
                //             context.Fail("Unauthorized");
                //         }
                //         return Task.CompletedTask;
                //     }
                // };
                //x.RequireHttpsMetadata = false;
                //x.SaveToken = true;
                //x.TokenValidationParameters = new TokenValidationParameters
                //{
                    //ValidateIssuerSigningKey = true,
                    //IssuerSigningKey = new SymmetricSecurityKey(key),
                    //RequireSignedTokens = true,
                    //ValidateIssuer = false,
                    //ValidateAudience = false
                //};
            //});           
            /* var sharedKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("mysupers3cr3tsharedkey!"));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Specify the key used to sign the token:
                    IssuerSigningKey = sharedKey,
                    RequireSignedTokens = true,
                    // Other options...
                };
            }); */
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            //app.UseStaticFiles();
            app.UseCookiePolicy();
            //app.UseAuthentication();
            app.UseMvc();
        }
    }
}
