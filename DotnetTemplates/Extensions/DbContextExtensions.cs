namespace DotnetTemplates.Extensions;

using System;
using System.Linq;
using DotnetTemplates.Database.Models;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// DbContext extension that allows tracking creation and update of entities to set
/// CreatedAt and UpdatedAt timestamps.
/// </summary>
public static class DbContextExtensions
{
    /// <summary>
    /// Sets CreatedAt and UpdatedAt values. Should be called inside SaveChanges() and
    /// SaveChangesAsync().
    /// </summary>
    /// <param name="context">Context with entities to set timestamps to.</param>
    public static void SetCreatedUpdatesTimestamps(this DbContext context)
    {
        foreach (var item in from entry in context.ChangeTracker.Entries()
                                     where entry.Entity is EntityBase && (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                                     select entry)
        {
            var entityBase = (EntityBase)item.Entity;
            var utcNow = DateTime.UtcNow;
            if (entityBase.CreatedAt == default)
            {
                entityBase.CreatedAt = utcNow;
            }

            entityBase.UpdatedAt = utcNow;
        }
    }
}
