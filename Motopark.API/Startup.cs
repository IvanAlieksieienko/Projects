using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Motopark.Core.Entities;
using Motopark.Core.IRepositories;
using Motopark.Core.IServices;
using Motopark.Core.Services;
using Motopark.Infrastructure.Repositories;
using System;
using System.IO;

namespace Motopark.API
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
            services.AddMvc();
            services.AddControllers().AddJsonOptions(options =>
            {
                // ...
            });

            /* Development mode */
            var connectionString = Configuration.GetConnectionString("LocalDB");
            /* Production mode */
            //var connectionString = Configuration.GetConnectionString("RemoteDB");

            services.AddScoped<ICategoryRepository<Category>, CategoryRepository>(provider => new CategoryRepository(connectionString));
            services.AddScoped<IProductRepository<Product>, ProductRepository>(provider => new ProductRepository(connectionString));
            services.AddScoped<IAdminRepository<Admin>, AdminRepository>(provider => new AdminRepository(connectionString));
            services.AddScoped<IBasketRepository<Basket>, BasketRepository>(provider => new BasketRepository(connectionString));
            services.AddScoped<IDeliveryRepository<Delivery>, DeliveryRepository>(provider => new DeliveryRepository(connectionString));
            services.AddScoped<IImageProductRepository<ImageProduct>, ImageProductRepository>(provider => new ImageProductRepository(connectionString));
            services.AddScoped<IOrderRepository<Order>, OrderRepository>(provider => new OrderRepository(connectionString));
            services.AddScoped<IFeatureRepository<Feature>, FeatureRepository>(provider => new FeatureRepository(connectionString));

            services.AddScoped<ICategoryService<Category>, CategoryService>();
            services.AddScoped<IProductService<Product>, ProductService>();
            services.AddScoped<IAdminService<Admin>, AdminService>();
            services.AddScoped<IBasketService<Basket>, BasketService>();
            services.AddScoped<IDeliveryService<Delivery>, DeliveryService>();
            services.AddScoped<IImageProductService<ImageProduct>, ImageProductService>();
            services.AddScoped<IOrderService<Order>, OrderService>();
            services.AddScoped<IFeatureService<Feature>, FeatureService>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/basket");
                });

            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            /* Production mode */
            //services.AddSpaStaticFiles(configuration => configuration.RootPath = $"ClientApp/dist");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            /* Development mode */
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}/{id?}");
            });
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                RequestPath = new PathString("/Resources")
            });

            /* Production mode */
            //app.UseSpaStaticFiles();
            //app.UseSpa(configuration => { /*spa.Options.SourcePath = "ClientApp";*/ });

            /* Development mode */
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                    spa.Options.StartupTimeout = TimeSpan.FromSeconds(500);
                }
            });
        }
    }
}
