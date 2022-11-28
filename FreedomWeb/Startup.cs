// This Startup file is based on ASP.NET Core new project templates and is included
// as a starting point for DI registration and HTTP request processing pipeline configuration.
// This file will need updated according to the specific scenario of the application being upgraded.
// For more information on ASP.NET Core startup files, see https://docs.microsoft.com/aspnet/core/fundamentals/startup

using AspNetCore.Unobtrusive.Ajax;
using FreedomLogic.DAL;
using FreedomLogic.Identity;
using FreedomLogic.Infrastructure;
using FreedomLogic.Managers;
using FreedomLogic.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
            // Register App Configuration
            var appConfig = new AppConfiguration();
            Configuration.Bind(appConfig);
            services.AddSingleton(appConfig);

            // Configure EntityFramework contexts
            services.AddDbContext<DbFreedom>(options => options.UseMySQL(Configuration.GetConnectionString("FreedomDb")));
            services.AddDbContext<DbAuth>(options => options.UseMySQL(Configuration.GetConnectionString("AuthDb")));
            services.AddDbContext<DbCharacters>(options => options.UseMySQL(Configuration.GetConnectionString("CharactersDb")));
            services.AddDbContext<DbWorld>(options => options.UseMySQL(Configuration.GetConnectionString("WorldDb")));

            // Configure .NET Identity
            services.AddScoped<IPasswordHasher<User>, FreedomShaHasher>();
            services.AddScoped<IPasswordValidator<User>, FreedomPasswordValidator>();
            services.AddAuthorization();
            services.AddAuthentication(o =>
            {
                o.DefaultScheme = IdentityConstants.ApplicationScheme;
                o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            }).AddIdentityCookies();

            services.AddIdentityCore<User>()
                .AddUserStore<UserStore>()
                .AddRoles<FreedomRole>()
                .AddRoleStore<RoleStore>()
                .AddSignInManager()
                .AddUserManager<UserManager>()
                .AddDefaultTokenProviders();

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
                    "css/bootstrap.css", "css/font-awesome.css", "css/dataTables.bootstrap.min.css",
                    "css/bootstrap-select.min.css", "css/site.css"
                );
                pipeline.AddJavaScriptBundle("/js/jquery", "js/jquery-2.2.0.js");
                pipeline.AddJavaScriptBundle("/js/jqueryval", "js/jquery.validate.js");
                pipeline.AddJavaScriptBundle("/js/jqueryDateFormat", "js/jquery.dateFormat.min.js");
                pipeline.AddJavaScriptBundle("/js/modernizr", "js/modernizr-2.8.3.js");
                pipeline.AddJavaScriptBundle("/js/dataTables", "js/jquery.dataTables.min.js", "js/dataTables.bootstrap.min.js");
                pipeline.AddJavaScriptBundle("/js/bootstrapSelect", "js/bootstrap-select.min.js");
                pipeline.AddJavaScriptBundle("/js/bootstrap", "js/bootstrap.js", "js/respond.js");
                pipeline.AddJavaScriptBundle("/js/freedom", "js/Common/common.js", "js/Common/common_datatables.js", "js/Common/site.js");
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
            app.UseAuthentication();
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
