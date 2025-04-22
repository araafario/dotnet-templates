namespace DotnetTemplates;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using DotnetTemplates.Configs;
using DotnetTemplates.WebApp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

/// <summary>
/// The main class of the Harmony application.
/// </summary>
public static class Program
{
    /// <summary>
    /// The entry point of the application.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [ExcludeFromCodeCoverage] // Exclude this method from code coverage
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var apiConfig = BindAppConfig(builder);

        var logger = LoggerConfiguration(new LoggerConfiguration(), "DotnetTemplate", ApiConfig.Environment, apiConfig.Seq)
            .Filter.ByExcluding(logEvent =>
            {
                if (logEvent.Properties.TryGetValue("EndpointName", out var value))
                {
                    return value.ToString() == "HTTP: GET /";
                }

                return false;
            })
            .CreateLogger();

        Log.Logger = logger;
        logger.Information($"Starting DotnetTemplates {ApiConfig.Environment}");

        builder.Configuration.AddEnvironmentVariables().AddCommandLine(args);
        builder.WebHost
            .UseSerilog(logger)
            .ConfigureKestrel((context, serverOptions) => serverOptions.ListenAnyIP(apiConfig.Port, listenOptions =>
            {
                if (apiConfig.UseHttps)
                {
                    listenOptions.UseHttps();
                }
            }));

        await builder
            .RegisterServices(apiConfig)
            .Build()
            .SetupMiddleware(apiConfig)
            .RunAsync();
    }

    private static ApiConfig BindAppConfig(WebApplicationBuilder builder)
    {
        var apiConfig = new ApiConfig();
        builder.Configuration.Bind(apiConfig);

        // This is to support database connection string on azure webapp.
        var dbConnectiontring = builder.Configuration.GetConnectionString("DatabaseConnectionString");

        if (!string.IsNullOrWhiteSpace(dbConnectiontring))
        {
            apiConfig.DatabaseConnectionString = dbConnectiontring;
        }

        return apiConfig;
    }

    private static LoggerConfiguration LoggerConfiguration(
        LoggerConfiguration loggerConfiguration,
        string source,
        string environment,
        SeqConfig? seqConfig = null)
    {
        loggerConfiguration.MinimumLevel.Override("Microsoft", LogEventLevel.Information).Enrich.FromLogContext().Enrich.WithMachineName()
            .Enrich.WithProperty("Source", source)
            .Enrich.WithProperty("Environment", environment)
            .Filter.ByExcluding((LogEvent logEvent) => logEvent.Properties.ContainsKey("SourceContext") &&
                                logEvent.Properties["SourceContext"].ToString()
                                .Contains("Microsoft.AspNetCore.Mvc.Formatters.Json.Internal.JsonResultExecutor"))
                                .Filter.ByExcluding((LogEvent logEvent) => logEvent.Properties
                                .ContainsKey("RequestPath") &&
                                (logEvent.Properties["RequestPath"].ToString().Contains("/v1/health") ||
                                logEvent.Properties["RequestPath"].ToString().Contains("/v1/version")))
                                .Filter.ByExcluding((LogEvent logEvent) => logEvent.Properties.ContainsKey("SourceContext") &&
                                logEvent.Properties["SourceContext"].ToString().Contains("Microsoft.AspNetCore.Hosting.Diagnostics"));
        var environmentVariable = Environment.GetEnvironmentVariable("IS_STAGING");
        if (environmentVariable != null)
        {
            loggerConfiguration.Enrich.WithProperty("IsStaging", environmentVariable.ToUpper() == "TRUE");
        }

        if (environment == Environments.Development || environment == "Engineering")
        {
            loggerConfiguration.MinimumLevel.Debug().WriteTo.Console();
        }
        else
        {
            loggerConfiguration.MinimumLevel.Information();
        }

        if (seqConfig != null && seqConfig.Enabled)
        {
            var writeTo = loggerConfiguration.WriteTo;
            var url = seqConfig.Url;
            var apiKey = seqConfig.ApiKey;
            writeTo.Seq(url, LogEventLevel.Verbose, 1000, null, apiKey, null, null, 262144L);
        }

        return loggerConfiguration;
    }
}
