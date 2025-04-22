namespace DotnetTemplates.Controllers;

using System.Linq;
using DotnetTemplates.Database;
using DotnetTemplates.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Sample controller to indicate standard endpoints.
/// </summary>
[Route("v1/[controller]")]
[Authorize]
public class SampleController : ControllerBase
{
    private readonly DotnetTemplatesContext dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="SampleController"/> class.
    /// </summary>
    /// <param name="dbContext">dbContext.</param>
    public SampleController(DotnetTemplatesContext dbContext) => this.dbContext = dbContext;

    /// <summary>
    /// Get test object, relying on standard serialization.
    /// </summary>
    /// <returns>
    /// {
    ///   "data": {
    ///     "test": "one",
    ///     "next": "two"
    ///   }
    /// }.
    /// </returns>
    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetTest()
    {
        var returnObject = new { test = "one", next = "two" };

        return this.Ok(returnObject);
    }

    /// <summary>
    /// Get the name of a test person by ID using Entity Framework.
    /// </summary>
    /// <param name="id">The ID of the test person.</param>
    /// <returns>The name of the person if found, otherwise 404.</returns>
    [HttpGet("ef/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetTestEf(int id)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var person = this.dbContext.TestPersons.FirstOrDefault(x => x.Id == id);

        if (person == null)
        {
            return this.NotFound();
        }

        return this.Ok(person);
    }

    /// <summary>
    /// Post new person.
    /// </summary>
    /// <param name="person">The test person to add.</param>
    /// <returns>The created person.</returns>
    [HttpPost("ef")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult AddTestEf([FromBody] TestPerson person)
    {
        if (!this.ModelState.IsValid || string.IsNullOrWhiteSpace(person.Name))
        {
            return this.BadRequest(this.ModelState);
        }

        this.dbContext.TestPersons.Add(person);
        this.dbContext.SaveChanges();

        // cqrs.
        return this.Ok();
    }

    /// <summary>
    /// Get error object.
    /// </summary>
    /// <returns>
    /// {
    ///   "error": {
    ///     ...
    ///   }
    /// }.
    /// </returns>
    [HttpGet("error")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetError() => this.Problem("detail", "instance", StatusCodes.Status404NotFound, "title", "type");
}
