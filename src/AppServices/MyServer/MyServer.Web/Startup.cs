namespace MyServer.Web
{
    using System.Globalization;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication.OpenIdConnect;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Localization;

    using MyServer.Web.Helpers;

    using Newtonsoft.Json.Serialization;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public static IStringLocalizer<SharedResource> SharedLocalizer { get; private set; }

        public IConfiguration Configuration { get; }

        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
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

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(
                routes =>
                    {
                        routes.MapRoute(name: "areaRoute", template: "{area:exists}/{controller}/{action=Index}/{id?}");
                        routes.MapRoute(name: "default", template: "{controller}/{action=Index}/{id?}");
                    });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(
                sharedOptions =>
                    {
                        sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                    }).AddAzureAdB2C(options => this.Configuration.Bind("AzureAdB2C", options)).AddCookie();

            services.AddCloudscribeNavigation();

            services.AddMvc(options => options.Filters.Add(typeof(RequireHttpsAttribute)))
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver())
                .AddRazorPagesOptions(options => { }).AddViewLocalization(x => x.ResourcesPath = "Resources");
        }
    }
}