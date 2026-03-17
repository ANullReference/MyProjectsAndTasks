using Infrastructure.DatabaseRepository;
using Infrastructure.DatabaseRepository.DTO;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Tests;

/// <summary>
/// 
/// </summary>
public class ProjectRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly ApplicationDbContext _context;
    private readonly ProjectRepository _sut; // System Under Test

    public ProjectRepositoryTests()
    {
        // 1. Create and open the connection
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        // 2. Build options and ensure the schema is created
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new ApplicationDbContext(options);
        _context.Database.EnsureCreated(); // This creates your tables/keys

        _sut = new ProjectRepository(_context);
    }

    [Fact]
    public async Task Delete_WithValidId_ShouldRemoveProjectAndTasks()
    {
        // Arrange
        ProjectsDTO project = new() { Id = 1, Name = "Test Project" };
        TasksDTO task = new() { Id = 10, FkProjectId = 1, Title = "Subtask" };

        _context.Projects.Add(project);
        _context.Tasks.Add(task);
        int _ = await _context.SaveChangesAsync(CancellationToken.None);

        // Act
        bool result = await _sut.Delete(1, CancellationToken.None);

        // Assert
        Assert.True(result);
        Assert.False(await _context.Projects.AnyAsync(a => a.Id.Equals(1), CancellationToken.None));
        Assert.Empty(await _context.Tasks.Where(t => t.FkProjectId == 1).ToListAsync(CancellationToken.None));
    }

    public void Dispose()
    {
        // SQLite In-Memory is deleted when the connection closes
        _connection.Close();
        _context.Dispose();
    }
}