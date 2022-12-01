// This Startup file is based on ASP.NET Core new project templates and is included
// as a starting point for DI registration and HTTP request processing pipeline configuration.
// This file will need updated according to the specific scenario of the application being upgraded.
// For more information on ASP.NET Core startup files, see https://docs.microsoft.com/aspnet/core/fundamentals/startup

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
            services.AddWebOptimizer(pipeline =>
            {
                pipeline.AddCssBundle("/css/bundle.css",
                    "lib/bootstrap/css/bootstrap.css", "css/font-awesome.css", "lib/datatables/datatables.css", "css/app.css"
                );
                pipeline.AddJavaScriptBundle("/js/jquery", "lib/jquery/jquery-3.6.1.js");
                pipeline.AddJavaScriptBundle("/js/bootstrap", "lib/bootstrap/js/bootstrap.js");
                pipeline.AddJavaScriptBundle("/js/modernizr", "lib/modernizr-2.8.3.js");
                pipeline.AddJavaScriptBundle("/js/dataTables", "lib/datatables/datatables.js");
                pipeline.AddJavaScriptBundle("/js/freedom", "js/common.js", "js/freedom_datatables.js", "js/site.js");
            });

            // Add our own services
            services.AddScoped<AccountManager>();
            services.AddScoped<CharacterManager>();
            services.AddScoped<CommandStore>();
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
        }

        private void ConfigureMvcOptions(MvcOptions mvcOptions)
        { 
        }
    }
}
