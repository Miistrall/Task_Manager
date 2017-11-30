using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Context;
using TaskManager.Models;

namespace TaskManager
{
    public class Startup
    {
        protected IConfigurationRoot ConfigurationRoot;
        public Startup()
        {
            var configuratioBuilder = new ConfigurationBuilder();
            configuratioBuilder.AddXmlFile("Config.xml");
            ConfigurationRoot = configuratioBuilder.Build();

        }

        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<EFContext>(optionsBuilder =>
                optionsBuilder.UseSqlServer(
                    ConfigurationRoot["ConnectionString"]));

            serviceCollection.AddScoped<SignInManager<UserModel>>();
            serviceCollection.AddScoped<UserManager<UserModel>>();
            serviceCollection.AddIdentity<UserModel, IdentityRole<int>>().AddEntityFrameworkStores<EFContext>();
            serviceCollection.AddScoped<RoleManager<IdentityRole<int>>>();
            serviceCollection.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();

        }
    }
}
