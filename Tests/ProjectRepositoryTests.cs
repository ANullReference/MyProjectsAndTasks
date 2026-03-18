using Infrastructure.DatabaseRepository;
using Infrastructure.DatabaseRepository.DTO;
using Microsoft.EntityFrameworkCore;
using Tests.Helpers;

namespace Tests;

/// <summary>
/// 
/// </summary>
public class ProjectRepositoryTests : IAsyncDisposable
{

    private readonly ApplicationDbContext _context;
    private readonly ProjectRepository _sut; // System Under Test

    public ProjectRepositoryTests()
    {
        _context = new Database().Generate();
        _context.Database.EnsureCreated();
        _sut = new ProjectRepository(_context);
    }

    [Test]
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
        await Assert.That(result).IsTrue();
        await Assert.That(await _context.Projects.AnyAsync(a => a.Id.Equals(1), CancellationToken.None)).IsFalse();
        await Assert.That(await _context.Tasks.Where(t => t.FkProjectId == 1).ToListAsync(CancellationToken.None)).IsEmpty();
    }

    public async ValueTask DisposeAsync() => await _context.DisposeAsync();
}