using CSharpApp.Infrastructure.Security.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CSharpApp.Infrastructure.Security.DependencyInjection
{
    public static class SecurityRegistration
    {
            public static IServiceCollection AddJwtSecurity(this IServiceCollection services, IConfiguration configuration)
            {
                services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
                services.AddSingleton<IJwtTokenService, JwtTokenService>();

            var options = configuration.GetSection("JwtOptions").Get<JwtOptions>()!;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = options.Issuer,
                        ValidAudience = options.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(options.SecretKey))
                    };
                });

            services.AddAuthorization();

            return services;
        }
    }
}
