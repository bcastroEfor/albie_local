using ActioBP.Identity.EF;
using ActioBP.Identity.Interfaces;
using ActioBP.Identity.JsonWebToken;
using ActioBP.Identity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;

namespace ActioBP.Identity
{
    public static class ServiceIdentityExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
           var iStr = configuration.GetConnectionString("Default");
            if (string.IsNullOrWhiteSpace(iStr)) throw new Exception("ActioBP.Identity requires a connection string named 'Identity' must be defined in your appsettings file. Use the overload instead.");

            return AddIdentityServices(services, configuration, iStr);
        }
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration, string connectionString)
        {
            services.AddDbContext<MyDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddAuthentication(o =>
            {
                o.DefaultScheme = IdentityConstants.ApplicationScheme;
                o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            }).AddMicrosoftAccount(options =>
            {
                options.ClientId = configuration.GetSection("OAuth").GetSection("Microsoft").GetValue("ClientId", "");
                options.ClientSecret = configuration.GetSection("OAuth").GetSection("Microsoft").GetValue("ClientSecret", "");
                options.CallbackPath = "/api/oauth/callback";
            });

            services.AddIdentity<MyUser, MyRole>(options => BuildIdentityOptions(options))
                .AddEntityFrameworkStores<MyDbContext>()
                .AddSignInManager<MySignInManager>()
                .AddUserManager<MyUserManager>()
                .AddDefaultTokenProviders();

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("RequireAdministratorRole", policy => {
            //        policy.RequireRole("Administrator");
            //        policy.AddRequirements();
            //    });
            //});

            //services.AddAuthentication(options => ConfigureJwtAuthentication(options))
            //    .AddJwtBearer(options => ConfigureJwtAuth(options, configuration));

            services.AddScoped<IAuthRespository, AuthRepository>();

            return services;
        }

        private static void BuildIdentityOptions(IdentityOptions options)
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
        }

        private static void ConfigureJwtAuthentication(AuthenticationOptions options)
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }
        private static void ConfigureJwtAuth(JwtBearerOptions options, IConfiguration Configuration)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                // Validate the token expiry
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = Configuration.GetSection("TokenAuthentication:Issuer").Value,
                ValidAudience = Configuration.GetSection("TokenAuthentication:Audience").Value,
                IssuerSigningKey = JwtSecurityKey.Create(Configuration.GetSection("TokenAuthentication:SecretKey").Value),
                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };

            options.TokenValidationParameters = tokenValidationParameters;
            options.Validate();
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    //Fail
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    //Success
                    return Task.CompletedTask;
                }
            };
        }
    }
}
