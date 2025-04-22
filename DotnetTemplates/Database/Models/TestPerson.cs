namespace DotnetTemplates.Database.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Base Model for TestPerson.
/// </summary>
public class TestPerson : EntityBase
{
    /// <summary>
    /// Gets or Sets the Id.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets Name.
    /// </summary>
    [MaxLength(64)]
    public string Name { get; set; } = string.Empty;
}
