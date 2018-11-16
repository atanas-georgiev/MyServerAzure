namespace MyServer.Web
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication.OpenIdConnect;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Razor;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Localization;
    using Microsoft.Net.Http.Headers;

    using MyServer.Data.Common;
    using MyServer.Data.Models;
    using MyServer.Services.Mappings;
    using MyServer.Services.Users;
    using MyServer.Web.Helpers;

    using Newtonsoft.Json.Serialization;

    public class Startup
    {
        public static IServiceScopeFactory scopeFactory = null;

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public static IStringLocalizer<SharedResource> SharedLocalizer { get; private set; }

        public IConfiguration Configuration { get; }

        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            IStringLocalizer<SharedResource> sharedLocalizer,
            IServiceScopeFactory scopeFactory)
        {
            Startup.scopeFactory = scopeFactory;

            var supportedCultures = new[] { new CultureInfo("en-US"), new CultureInfo("bg-BG") };
            SharedLocalizer = sharedLocalizer;

            app.UseRequestLocalization(
                new RequestLocalizationOptions
                    {
                        DefaultRequestCulture = new RequestCulture("en-US"),
                        SupportedCultures = supportedCultures,
                        SupportedUICultures = supportedCultures
                    });

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error?statusCode=500");
                app.UseStatusCodePagesWithReExecute("/Error", "?statusCode={0}");
            }

            app.UseStaticFiles(
                new StaticFileOptions()
                    {
                        OnPrepareResponse = (context) =>
                            {
                                var headers = context.Context.Response.GetTypedHeaders();
                                headers.CacheControl =
                                    new CacheControlHeaderValue()
                                        {
                                            MaxAge = TimeSpan.FromDays(
                                                100)
                                        };
                            }
                    });

            app.UseAuthentication();

            app.UseMvc(
                routes =>
                    {
                        // Areas support
                        routes.MapRoute(name: "areaRoute", template: "{area:exists}/{controller}/{action=Index}/{id?}");

                        routes.MapRoute(name: "default", template: "{controller}/{action=Index}/{id?}");
                    });

            app.UseKendo(env);

            var autoMapperConfig = new AutoMapperConfig();
            autoMapperConfig.Execute(
                new List<Assembly>
                    {
                        Assembly.GetEntryAssembly(),
                        // typeof(LatestAddedAlbumsViewComponent).GetTypeInfo().Assembly,
                        // typeof(SocialViewComponent).GetTypeInfo().Assembly
                    });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(
                sharedOptions =>
                    {
                        sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                    }).AddAzureAdB2C(options => this.Configuration.Bind("AzureAdB2C", options))
                    .AddCookie(options => options.Events = new CookieAuthenticationEvents()
                                                               {
                                                                   OnSigningIn = OnSigningIn
                                                               });

            services.AddCloudscribeNavigation();

            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddMemoryCache();

            services.Configure<RazorViewEngineOptions>(
                o => { o.ViewLocationExpanders.Add(new ViewLocationExpander()); });

            services.Add(ServiceDescriptor.Scoped(typeof(IRepository<>), typeof(AzureTableStorage<>)));
            services.AddScoped<IUserService, UserService>();

            services.AddMvc(options => options.Filters.Add(typeof(RequireHttpsAttribute)))
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver())
                .AddRazorPagesOptions(options => { }).AddViewLocalization(x => x.ResourcesPath = "Resources");

            services.AddKendo();

            services.Configure<IISOptions>(options => { });

            var embeddedFileProviders = new List<EmbeddedFileProvider>();

            /*
            // ImageGalleryViewComponents
            embeddedFileProviders.Add(
                new EmbeddedFileProvider(
                    typeof(LatestAddedAlbumsViewComponent).GetTypeInfo().Assembly,
                    "MyServer.ViewComponents.ImageGallery"));

            // CommonViewComponents
            embeddedFileProviders.Add(
                new EmbeddedFileProvider(
                    typeof(SocialViewComponent).GetTypeInfo().Assembly,
                    "MyServer.ViewComponents.Common"));*/

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.Configure<RazorViewEngineOptions>(
                options =>
                    {
                        foreach (var embeddedFileProvider in embeddedFileProviders)
                        {
                            options.FileProviders.Add(embeddedFileProvider);
                        }
                    });
        }

        private Task OnSigningIn(CookieSigningInContext cookieSignedInContext)
        {
            var users = new AzureTableStorage<User>(this.Configuration);
            var id = cookieSignedInContext.Principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var user = users.GetAsync("user", id).GetAwaiter().GetResult();

            if (user != null && user.IsAdmin)
            {
                var identity = new ClaimsIdentity(cookieSignedInContext.Principal.Identity);
                identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                var principal = new ClaimsPrincipal(identity);
                cookieSignedInContext.Principal = new ClaimsPrincipal(principal);
            }

            return Task.CompletedTask;
        }
    }
}