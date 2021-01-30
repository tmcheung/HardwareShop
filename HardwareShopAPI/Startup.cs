using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.EF;
using HardwareShopAPI.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServiceLayer;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace HardwareShopAPI
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
            services.AddControllers().AddNewtonsoftJson(a => a.SerializerSettings.TypeNameHandling = TypeNameHandling.None);
            services.AddHttpContextAccessor();
            services.AddAuthorization(opts =>
            {
                opts.AddPolicy("RequireAdminRole", policy =>
                {
                    policy.RequireAssertion(httpcx => httpcx.User.IsInRole("Admin"));
                });
            });

            services
                .AddTransient<IShopContext, ShopContext>(p => new ShopContext(Configuration.GetValue<string>("MySQLConnectionString")))
                .AddSingleton<IAuthenticationService, ChallengeOnlyAuthenticationService>()
                .AddTransient<ProductService>()
                .AddTransient<OrderService>()
                .AddTransient<BannerService>()

                .AddTransient<UserRepository>()
                .AddTransient<ProductRepository>()
                .AddTransient<OrderRepository>()
                .AddTransient<BannerRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IShopContext dbContext)
        {
            if (env.IsDevelopment())
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
                dbContext.Seed();

                app.UseDeveloperExceptionPage();
            }
            else
            {
                dbContext.Database.EnsureCreated();
            }


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseFakeAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
