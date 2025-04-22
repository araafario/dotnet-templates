namespace DotnetTemplates.Configs;

using System;
using System.Reflection;
using Microsoft.Extensions.Hosting;

/// <summary>
/// Represents the configuration settings for the API.
/// </summary>
public class ApiConfig
{
    /// <summary>
    /// Gets the environment the application is running in. This value determines database and API server
    /// the applicatin will point to.
    /// </summary>
    public static string Environment => System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environments.Development;

    /// <summary>
    /// Gets API version.
    /// </summary>
    public static string? Version => Assembly
          .GetExecutingAssembly()
          .GetName()
          ?.Version?.ToString();

    /// <summary>
    /// Gets or sets a value indicating whether the application should use HTTPs.
    /// </summary>
    /// <remarks>
    /// This settings should only be set to false when a service is running behind a
    /// reverse proxy (i.e. a containerised development environment).
    /// </remarks>
    public bool UseHttps { get; set; } = true;

    /// <summary>
    /// Gets or sets the port the server will run on when running in development mode. When run as a hosted service
    /// this value will be overwritten.
    /// </summary>
    public int Port { get; set; } = 5000;

    /// <summary>
    /// Gets or sets the database connection string.
    /// </summary>
    public string DatabaseConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the token used to sign JWTs.
    /// </summary>
    public string JwtSigningToken { get; set; } = Guid.Empty.ToString();

    /// <summary>
    /// Gets or sets the Seq config.
    /// </summary>
    public SeqConfig? Seq { get; set; }
}
