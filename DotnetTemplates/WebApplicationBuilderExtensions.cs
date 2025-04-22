namespace DotnetTemplates.WebApp;

using System.Text;
using System.Text.Json.Serialization;
using DotnetTemplates.Configs;
using DotnetTemplates.Database;
using EntityFramework.Exceptions.SqlServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

/// <summary>
/// Web Application Builder Extensions.
/// </summary>
public static class WebApplicationBuilderExtensions
{
    /// <summary>
    /// Registers various services required by the web application.
    /// </summary>
    /// <param name="applicationBuilder">The <see cref="WebApplicationBuilder"/> instance to which the services will be registered.</param>
    /// <param name="apiConfig">API configuration.</param>
    /// <returns>The same <see cref="WebApplicationBuilder"/> instance after services registration.</returns>
    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder applicationBuilder, ApiConfig apiConfig)
    {
        applicationBuilder.Services.AddMvc().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        applicationBuilder.Services.AddJwtAuthentication(apiConfig.JwtSigningToken); // Static Jwt token.

        applicationBuilder.Services.AddDbContext<DotnetTemplatesContext>(options =>
        {
            options.UseSqlServer(apiConfig.DatabaseConnectionString);
            options.UseExceptionProcessor();

            if (ApiConfig.Environment.ToLower() is "developement" or "engineering")
            {
                options.EnableSensitiveDataLogging();
            }
        });

        return applicationBuilder;
    }

    private static AuthenticationBuilder AddJwtAuthentication(
        this IServiceCollection services,
        string jwtSigningToken,
        TokenValidationParameters? options = null,
        string defaultAuthenticateScheme = "Bearer",
        string defaultChallengeScheme = "Bearer") =>
            services.AddAuthentication(delegate(AuthenticationOptions authenticationOptions)
            {
                authenticationOptions.DefaultAuthenticateScheme = defaultAuthenticateScheme;
                authenticationOptions.DefaultChallengeScheme = defaultChallengeScheme;
            }).AddJwtBearer(delegate(JwtBearerOptions jwtBearerOptions)
            {
                var bytes = Encoding.ASCII.GetBytes(jwtSigningToken);
                jwtBearerOptions.TokenValidationParameters = options ?? new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(bytes),
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    RequireExpirationTime = false,
                };
            });
}
