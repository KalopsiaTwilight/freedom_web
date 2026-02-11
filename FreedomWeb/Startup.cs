// This Startup file is based on ASP.NET Core new project templates and is included
// as a starting point for DI registration and HTTP request processing pipeline configuration.
// This file will need updated according to the specific scenario of the application being upgraded.
// For more information on ASP.NET Core startup files, see https://docs.microsoft.com/aspnet/core/fundamentals/startup

using FreedomLogic.DAL;
using FreedomLogic.Identity;
using FreedomLogic.Infrastructure;
using FreedomLogic.Managers;
using FreedomLogic.Services;
using FreedomWeb.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using Yarp.ReverseProxy.Forwarder;

namespace FreedomWeb
{
    public partial class Startup
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
            services.AddDbContext<DbDbc>(options => options.UseMySQL(Configuration.GetConnectionString("DbcDb")));
            services.AddDbContext<DbDboAcc>(options => options.UseMySQL(Configuration.GetConnectionString("DboAcc")));
            services.AddDbContext<DbDboChar>(options => options.UseMySQL(Configuration.GetConnectionString("DboChar")));

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
                    "lib/bootstrap/css/bootstrap.css", "css/font-awesome.css", 
                    "css/app.css", 
                    //"lib/DataTables-1.13.1/css/jquery.dataTables.css", 
                    "lib/DataTables-1.13.1/css/dataTables.bootstrap5.css"
                );
                pipeline.AddJavaScriptBundle("/js/jquery", "lib/jquery/jquery-3.6.1.js", "lib/gasparesganga-jquery-loading-overlay/index.min.js");
                pipeline.AddJavaScriptBundle("/js/bootstrap", "lib/bootstrap/js/bootstrap.bundle.min.js");
                pipeline.AddJavaScriptBundle("/js/modernizr", "lib/modernizr-2.8.3.js");
                pipeline.AddJavaScriptBundle("/js/dataTables", 
                    "lib/DataTables-1.13.1/js/jquery.dataTables.js",
                    "js/dataTablesStyling.js"
                    //"lib/DataTables-1.13.1/js/dataTables.bootstrap5.js"
                    //"lib/ColReorder-1.6.1/js/dataTables.colReorder.js",
                    //"lib/ColReorder-1.6.1/js/colReorder.bootstrap5.js"
                );
                pipeline.AddJavaScriptBundle("/js/freedom", "js/common.js", "js/freedom_datatables.js", "js/site.js");
            });

            services.AddHttpClient();

            // Add our own services
            services.AddScoped<AccountManager>();
            services.AddScoped<CharacterManager>();
            services.AddScoped<ServerControl>();
            services.AddScoped<DboServerControl>();
            services.AddScoped<ExtraDataLoader>();
            services.AddScoped<MailService>();

            services.AddHttpForwarder();

            services.AddReverseProxy();
                //.LoadFromConfig(Configuration.GetSection("Proxy"));
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

            var httpClient = new HttpMessageInvoker(new SocketsHttpHandler()
            {
                AllowAutoRedirect = false,
                AutomaticDecompression = DecompressionMethods.None,
                UseCookies = false,
                ActivityHeadersPropagator = new ReverseProxyPropagator(DistributedContextPropagator.Current),
                ConnectTimeout = TimeSpan.FromSeconds(15),
            });
            var transformer = new CustomForwarderTransformer();
            var requestConfig = new ForwarderRequestConfig { ActivityTimeout = TimeSpan.FromSeconds(100) };

            var isArmorPath = IsArmorPath();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.Map("/modelviewer/{**catch-all}", async (HttpContext httpContext, IHttpForwarder forwarder, IMemoryCache memoryCache) =>
                {
                    // If it's a custom item request, we return the custom item json
                    var match = IsArmorPath().Match(httpContext.Request.Path);
                    if (match.Success)
                    {
                        if (memoryCache.TryGetValue(FreedomWebConstants.CustomItemCacheKeyPrefix + match.Groups[1], out var responseObj))
                        {
                            httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                            httpContext.Response.ContentType = "application/json";
                            await httpContext.Response.WriteAsJsonAsync(responseObj);
                            return;
                        }
                    }
                    // Forward request to wowhead otherwise
                    var error = await forwarder.SendAsync(httpContext, "https://wow.zamimg.com/", httpClient, requestConfig, transformer);
                    if (error != ForwarderError.None)
                    {
                        var errorFeature = httpContext.GetForwarderErrorFeature();
                        var exception = errorFeature.Exception;
                    }
                });
            });
        }

        private void ConfigureMvcOptions(MvcOptions mvcOptions)
        { 
        }

        [GeneratedRegex("/live/meta/armor/\\d+/(\\d+)")]
        private static partial Regex IsArmorPath();
    }
}
