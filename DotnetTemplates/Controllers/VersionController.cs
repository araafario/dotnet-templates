namespace DotnetTemplates.Controllers;

using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller for the Version.
/// </summary>
public class VersionController(IWebHostEnvironment webHostEnvironment) : Controller
{
    private readonly IWebHostEnvironment webHostEnvironment = webHostEnvironment;

    /// <summary>
    /// Gets the current Environment and Version of the ProjectCodeName.
    /// </summary>
    /// <returns>The environment and the version.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetVersion()
    {
        var result = new
        {
            environment = this.webHostEnvironment.EnvironmentName,
            version = Assembly.GetExecutingAssembly().GetName()?.Version?.ToString(),
        };

        return this.Ok(result);
    }
}
