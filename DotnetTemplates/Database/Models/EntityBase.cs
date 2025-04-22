namespace DotnetTemplates.Database.Models;

using System;

/// <summary>
/// Base entity with timestamps to be implemented by entities used in EF Core aplications.
/// </summary>
public abstract class EntityBase
{
    /// <summary>
    /// Gets or sets the date the entity was first created. This property should be populated
    /// automatically by the database context when a new entity is created. If set manually
    /// it will be overwritten when saved.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date the entity was last updated. This property should be populated
    /// automatically by the database context when an entity is updated, if set manually
    /// it will be overwritten when saved.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}
