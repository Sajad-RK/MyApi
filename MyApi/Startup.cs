using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Cotracts;
using Data.Repositories;
using ElmahCore.Mvc;
using ElmahCore.Sql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Services.Services;
using WebFramework.Configuration;
using WebFramework.Middlewares;

namespace MyApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _siteSettings = Configuration.GetSection(nameof(Common.SiteSettings)).Get<Common.SiteSettings>();
        }
        private readonly Common.SiteSettings _siteSettings;
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Common.SiteSettings>(Configuration.GetSection(nameof(Common.SiteSettings)));
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetSection("ConnectionsString").Value);
            });
            services.AddControllers();
            services.AddElmah<SqlErrorLog>(options =>
            {
                //options.Path = "/elmah-errors";
                options.ConnectionString = Configuration.GetSection("ConnectionsString").Value;

            });
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IJWTService, JWTService>();
            services.AddJwtAuthenticateion(_siteSettings.JWTSettings);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCustomExceptionHandler();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //else
            //{
            //    //app.UseExceptionHandler();
            //    app.UseHsts();
            //}
            app.UseElmah();
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
