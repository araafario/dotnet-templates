namespace DotnetTemplates.Database;

using System.Threading;
using System.Threading.Tasks;
using DotnetTemplates.Database.Models;
using DotnetTemplates.Extensions;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Database context for the Teams User Management context.
/// </summary>
public class DotnetTemplatesContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DotnetTemplatesContext"/> class.
    /// </summary>
    public DotnetTemplatesContext()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DotnetTemplatesContext"/> class.
    /// </summary>
    /// <param name="options">Configuration options for the Teams Admin context.</param>
    public DotnetTemplatesContext(DbContextOptions<DotnetTemplatesContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the DBSet for the TestPerson entity.
    /// </summary>
    public DbSet<TestPerson> TestPersons { get; set; } = default!;

    /// <summary>
    /// Save changes to database.
    /// </summary>
    /// <returns>The number of state entries written to the database.</returns>
    public override int SaveChanges()
    {
        this.SetCreatedUpdatesTimestamps();
        return base.SaveChanges();
    }

    /// <summary>
    /// Save changes to database async.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The number of state entries written to the database.</returns>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        this.SetCreatedUpdatesTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TestPerson>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<TestPerson>()
            .HasIndex(t => t.Name)
            .IsUnique();
    }
}
