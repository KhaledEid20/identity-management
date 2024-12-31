using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace identityManagement.Extention;

public static class JwtConfiguration
{
    public static void configureJwt(this IServiceCollection services , IConfiguration configuration){
        var jwtSetting = configuration.GetSection("jwtSetting");
        var secretKey = Environment.GetEnvironmentVariable("SECRET");

        services.AddAuthentication(opt =>{
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }
        ).AddJwtBearer(a => {
            a.TokenValidationParameters = new TokenValidationParameters{
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSetting["Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };
        });
    }
}
