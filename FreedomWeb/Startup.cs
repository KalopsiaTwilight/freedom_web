// This Startup file is based on ASP.NET Core new project templates and is included
// as a starting point for DI registration and HTTP request processing pipeline configuration.
// This file will need updated according to the specific scenario of the application being upgraded.
// For more information on ASP.NET Core startup files, see https://docs.microsoft.com/aspnet/core/fundamentals/startup

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Unobtrusive.Ajax;
using FreedomLogic.DAL;
using FreedomLogic.Identity;
using FreedomLogic.Managers;
using FreedomLogic.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FreedomWeb
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
            // Configure EntityFramework contexts
            services.AddDbContext<DbFreedom>(options => options.UseMySQL(Configuration.GetConnectionString("FreedomDb")));
            services.AddDbContext<DbAuth>(options => options.UseMySQL(Configuration.GetConnectionString("AuthDb")));
            services.AddDbContext<DbCharacters>(options => options.UseMySQL(Configuration.GetConnectionString("CharactersDb")));
            services.AddDbContext<DbWorld>(options => options.UseMySQL(Configuration.GetConnectionString("WorldDb")));

            // Configure .NET Identity
            services.AddScoped<IPasswordHasher<User>, FreedomShaHasher>();
            services.AddAuthorization();
            services.AddAuthentication(o =>
            {
                o.DefaultScheme = IdentityConstants.ApplicationScheme;
                o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            }).AddIdentityCookies();

            services.AddIdentityCore<User>()
                .AddDefaultTokenProviders()
                .AddUserStore<UserStore>()
                .AddSignInManager()
                .AddUserManager<UserManager<User>>();

            // MVC Configuration 
            services.AddControllersWithViews(ConfigureMvcOptions)
                // Newtonsoft.Json is added for compatibility reasons
                // The recommended approach is to use System.Text.Json for serialization
                // Visit the following link for more guidance about moving away from Newtonsoft.Json to System.Text.Json
                // https://docs.microsoft.com/dotnet/standard/serialization/system-text-json-migrate-from-newtonsoft-how-to
                .AddNewtonsoftJson(options =>
                {
                    options.UseMemberCasing();
                });
            services.AddUnobtrusiveAjax();
            services.AddWebOptimizer(pipeline =>
            {
                pipeline.AddCssBundle("/css/bundle.css",
                    "Content/bootstrap.css", "Content/font-awesome.css", "Content/dataTables.bootstrap.min.css",
                    "Content/bootstrap-select.min.css", "Content/site.css"
                );
                pipeline.AddJavaScriptBundle("/js/jquery", "Scripts/jquery-2.2.0.js");
                pipeline.AddJavaScriptBundle("/js/jqueryval", "Scripts/jquery.validate.js");
                pipeline.AddJavaScriptBundle("/js/jqueryDateFormat", "Scripts/jquery.dateFormat.min.js");
                pipeline.AddJavaScriptBundle("/js/modernizr", "Scripts/modernizr-2.8.3.js");
                pipeline.AddJavaScriptBundle("/js/dataTables", "Scripts/jquery.dataTables.min.js", "Scripts/dataTables.bootstrap.min.js");
                pipeline.AddJavaScriptBundle("/js/bootstrapSelect", "Scripts/bootstrap-select.min.js");
                pipeline.AddJavaScriptBundle("/js/bootstrap", "Scripts/bootstrap.js", "Scripts/respond.js");
                pipeline.AddJavaScriptBundle("/js/freedom", "Scripts/Common/common.js", "Scripts/Common/common_datatables.js", "Scripts/Common/site.js");
            });

            // Add our own services
            services.AddScoped<AccountManager>();
            services.AddScoped<CharacterManager>();
            services.AddScoped<ServerManager>();
            services.AddScoped<ServerControl>();
            services.AddScoped<ExtraDataLoader>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseWebOptimizer();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseUnobtrusiveAjax();
        }

        private void ConfigureMvcOptions(MvcOptions mvcOptions)
        { 
        }
    }
}
