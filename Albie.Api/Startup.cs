using ActioBP.Identity;
using Albie.BS;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using System;

namespace Albie.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connString = Configuration.GetConnectionString("Default");

            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true; // Para aceptar los XML // aplicación respete los encabezados Accept del explorador
                options.OutputFormatters.Add(new Microsoft.AspNetCore.Mvc.Formatters.XmlSerializerOutputFormatter());
                //var policy = new AuthorizationPolicyBuilder()
                //    .RequireAuthenticatedUser()
                //    .Build();
                //options.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddJsonOptions(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            )
            .AddXmlSerializerFormatters()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddAllServices(connString);
            services.AddIdentityServices(Configuration);
            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.LoginPath = "/api/account/login";
            //    options.LogoutPath = "/api/account/signout";
            //})
            ////services.AddAuthentication(options =>
            ////{
            ////    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            ////})
            ////.AddCookie(option =>
            ////{
            ////    //option.LoginPath = "/api/account/redirect"; option.LogoutPath = new PathString("/api/account/signout");
            ////    option.LoginPath = "/auth/login";
            ////    option.LogoutPath = new PathString("/api/account/signout");
            ////})
            //.AddAuthentication()
            //.AddMicrosoftAccount(options =>
            //{
            //    options.ClientId = Configuration.GetSection("OAuth").GetSection("Microsoft").GetValue("ClientId", "");
            //    options.ClientSecret = Configuration.GetSection("OAuth").GetSection("Microsoft").GetValue("ClientSecret", "");
            //    options.CallbackPath = "/api/oauth/callback";
            //    options.SaveTokens = true;
            //    //options.Scope.Add("https://login.microsoftonline.com/");
            //    // SCOPES POSIBLES:
            //    //Microsoft Graph: https://graph.microsoft.com
            //    //Office 365 Mail API: https://outlook.office.com
            //    //Azure AD Graph: https://graph.windows.net
            //    options.Validate(); // No sé si tendrás esto, ponlo si te deja
            //});
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //services.AddAuthentication().AddMicrosoftAccount(options =>
            //{
            //    options.ClientId = Configuration["authentication:microsoft:clientid"];
            //    options.ClientSecret = Configuration["authentication:microsoft:clientsecret"];
            //    options.Scope.Add("https://login.microsoftonline.com/");
            //    options.SaveTokens = true;
            //    //options.callbackpath para cambiar la url de retorno
            //});

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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            System.Web.HttpContextCore.Configure(app.ApplicationServices.
                GetRequiredService<IHttpContextAccessor>()
            );
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthentication();
            //app.UseCookiePolicy();

            //app.UseAuthentication();
            AppContext.SetSwitch("System.Net.Http.UseSocketsHttpHandler", false);
            app.UseMvc();
        }
    }
}
