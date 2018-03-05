using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AgencyManager.Models;
using Microsoft.EntityFrameworkCore;
using AgencyManager.DAL.Repository;
using System.IO;

namespace AgencyManager
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var connection = Configuration.GetConnectionString("AgencyManagerDatabase");
            services.AddDbContext<AgencyManagerContext>(options => options.UseSqlServer(connection));
            services.AddTransient<AddressRepository, AddressRepository>();
            services.AddTransient<AgentRepository, AgentRepository>();
            services.AddTransient<CompanyCategoryRepository, CompanyCategoryRepository>();
            services.AddTransient<ContactCategoryRepository, ContactCategoryRepository>();
            services.AddTransient<ContactRepository, ContactRepository>();
            services.AddTransient<ConversationRepository, ConversationRepository>();
            services.AddTransient<IndustryRepository, IndustryRepository>();
            services.AddTransient<PositionRepository, PositionRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //test source control
            app.Use(async (context, next) => {
                await next();
                if (context.Response.StatusCode == 404 &&
                   !Path.HasExtension(context.Request.Path.Value) &&
                   !context.Request.Path.Value.StartsWith("/api/"))
                {
                    context.Request.Path = "/index.html";
                    await next();
                }
            });

            app.UseMvc();
            //app.UseMvcWithDefaultRoute();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            
        }
    }
}
