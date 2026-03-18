using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DatabaseRepository;

/// <summary>
/// Represents the Entity Framework Core database context for the application, providing access to tasks and projects
/// data.
/// </summary>
/// <remarks>This context manages the application's data model and is typically configured and injected by the
/// application's dependency injection system. Use this class to query and save instances of tasks and projects within
/// the database.</remarks>
/// <param name="options">The options to be used by the DbContext. Must not be null.</param>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public virtual DbSet<DTO.TasksDTO> Tasks { get; set; }

    public virtual DbSet<DTO.ProjectsDTO> Projects { get; set; }
}