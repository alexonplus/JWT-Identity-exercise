using Security.Models;
using System;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using System.Text;
using Security.Data;




public static class IdentityServiceExtensions
{
    // This method sets up the basic Identity system (Users and Roles)
    public static IServiceCollection AddIdentityStuff(this IServiceCollection services)
    {
        // 1. Tell the app to use ApplicationUser and IdentityRole
        // 2. Point it to our Database Context (AppDbContext) to save user data
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders(); // This adds support for things like password reset tokens

        return services;
    }

    // This method configures JWT (Token) authentication and Swagger UI
    public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration config)
    {
        // Tell the app: "Check for a JWT Bearer token in the header of every request"
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Check if the token was signed by our specific secret key
                    ValidateIssuerSigningKey = true,

                    // The secret key itself (grabbed from your appsettings.json file)
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!)),

                    // We set these to 'false' for now to make testing easier
                    // (In a real app, you'd check who issued the token and who it's for)
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        // This part adds the "Authorize" lock icon to the Swagger webpage
        services.AddSwaggerGen(c => {
            // Define how the security should look in Swagger
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Type exactly this: 'Bearer {your_token}'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            // Make sure Swagger actually sends that token when you click "Try it out"
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    new string[] { }
                }
            });
        });

        return services;
    }
}