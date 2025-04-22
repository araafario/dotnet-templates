namespace DotnetTemplates.WebApp;

using DotnetTemplates.Configs;
using DotnetTemplates.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

/// <summary>
/// Contains extension methods for setting up middleware in the <see cref="WebApplication"/>.
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Sets up the middleware required for the web application.
    /// </summary>
    /// <param name="application">The <see cref="WebApplication"/> instance to which the middleware will be applied.</param>
    /// <param name="apiConfig">API configuration.</param>
    /// <returns>The same <see cref="WebApplication"/> instance after middleware setup.</returns>
    public static WebApplication SetupMiddleware(this WebApplication application, ApiConfig apiConfig)
    {
        var useHttps = apiConfig.UseHttps;

        if (useHttps)
        {
            application.UseHttpsRedirection();
        }

        application.UseCors();
        application.UseForwardedHeaders();
        application.UseRouting();
        application.UseAuthentication();
        application.UseAuthorization();
        application.MapControllers();

        if (application.Environment.IsDevelopment())
        {
            var dotnetTemplatesDbContext = application.Services.CreateScope().ServiceProvider.GetRequiredService<DotnetTemplatesContext>();
            dotnetTemplatesDbContext.Database.Migrate();
        }

        application.MapControllerRoute(
                name: "version",
                pattern: "/v1/version",
                defaults: new { controller = "Version", action = "GetVersion" });

        return application;
    }
}
