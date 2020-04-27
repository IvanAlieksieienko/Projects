using AutoMapper;
using BookStore.BusinessLogicLayer.IRepositories;
using BookStore.BusinessLogicLayer.Services;
using BookStore.BusinessLogicLayer.Services.Interfaces;
using BookStore.DataAccessLayer.Dapper.Profiles;
using BookStore.DataAccessLayer.Dapper.Repositories;
using BookStore.DataAccessLayer.EntityFramework;
using BookStore.DataAccessLayer.EntityFramework.Profiles;
using BookStore.DataAccessLayer.EntityFramework.Repositories;
using BookStore.APILayer.Controllers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using BookStore.API.Layer;

namespace BookStore.APILayer
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env, IHostingEnvironment appEnv)
        {
            Configuration = configuration;
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
            Configuration = builder.Build();

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<MyOptions>(Configuration); 

            // In production, the Angular files will be served from this directory


            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new EFProfile());
                //mc.AddProfile(new DapperProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddDbContext<BookContext>(options => options.UseSqlServer(Configuration.GetConnectionString("BookStoreDatabase")), ServiceLifetime.Scoped);


            //services.AddScoped<IAdminRepository, DapperAdminRepository>(provider => new DapperAdminRepository(Configuration.GetConnectionString("BookStoreDatabase"), mapper));
            //services.AddScoped<IAuthorRepository, DapperAuthorRepository>(provider => new DapperAuthorRepository(Configuration.GetConnectionString("BookStoreDatabase"), mapper));
            //services.AddScoped<IBookRepository, DapperBookRepository>(provider => new DapperBookRepository(Configuration.GetConnectionString("BookStoreDatabase"), mapper));
            //services.AddScoped<IGenreRepository, DapperGenreRepository>(provider => new DapperGenreRepository(Configuration.GetConnectionString("BookStoreDatabase"), mapper));
            //services.AddScoped<IUserRepository, DapperUserRepository>(provider => new DapperUserRepository(Configuration.GetConnectionString("BookStoreDatabase"), mapper));


            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IGenreService, GenreService>();



            /* !!!!!!!!!!!!!!! Cookies Authentication !!!!!!!!!!!!!!! */

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
            });


            /******************************* JWT ************************************\
             
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.ISSUER,

                        ValidateAudience = true,
                        ValidAudience = AuthOptions.AUDIENCE,
                        ValidateLifetime = true,

                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                    };
                });

            \************************************************************************/

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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
