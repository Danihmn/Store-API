using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Store.Api.Extensions;

public static class AuthenticationExtension
{
    public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            var key = Encoding.UTF8.GetBytes(configuration["JwtSecretKey"]
                                             ?? throw new InvalidOperationException("JWT key is not configured"));

            options.MapInboundClaims = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                RoleClaimType = "role"
            };
        });
    }
}