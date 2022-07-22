using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SquadronApi.Services;
using System.Text;

namespace SquadronApi.Registers;

public static partial class Register
{
    public static IServiceCollection IdentityServices(this IServiceCollection services, IConfiguration config)
    {
        // Adding Authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
             // Adding Jwt Bearer
             .AddJwtBearer(tokenOptions =>
             {
                 var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:TokenKey"]));

                 tokenOptions.SaveToken = true;
                 tokenOptions.RequireHttpsMetadata = false;
                 tokenOptions.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateIssuerSigningKey = true,

                     // Set Clock skew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                     ClockSkew = TimeSpan.Zero,

                     ValidIssuer = config["Jwt:Issuer"],
                     ValidAudience = config["Jwt:Audience"],

                     IssuerSigningKey = signInKey
                 };

                 tokenOptions.Events = new JwtBearerEvents
                 {
                     // TODO if need it
                 };
             });

        services.AddScoped<TokenService>();

        return services;
    }
}