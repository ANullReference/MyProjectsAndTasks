using Core.Abstractions;
using Core.Domain;
using Infrastructure.DatabaseRepository.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.DatabaseRepository;

public class ProjectRepository(ApplicationDbContext _context) : IProjectRepository
{

    public async Task<ProjectModel> Create(ProjectModel projectModel, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(projectModel, nameof(projectModel));

        int projectId;

        ProjectsDTO projectDTO = ProjectsDTO.FromModel(projectModel);

        try
        {
            projectDTO.Id = null;
            _context.Projects.Add(projectDTO);
            projectId = await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Error saving project: {exception.Message}");
            throw new Exception("Failed save project in database");
        }

        return projectDTO.ToModel();
    }

    public async Task<bool> Delete(int projectId, CancellationToken cancellationToken)
    {
        if (projectId <= 0)
        {
            return false;
        }


        if (!_context.Projects.Any(a => a.Id.Equals(projectId)))
        {
            return false;
        }

        using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            int _ = await _context.Tasks.Where(w => w.FkProjectId.Equals(projectId))
                .ExecuteDeleteAsync(cancellationToken);

            _ = await _context.Projects.Where(w => w.Id.Equals(projectId))
                .ExecuteDeleteAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
        return true;
    }

    public async Task<ProjectModel> Get(int projectId, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            cancellationToken.ThrowIfCancellationRequested();
        }

        ProjectsDTO projectDTO = await _context.Projects.FirstAsync(f => f.Id.Equals(projectId), cancellationToken);

        if (projectDTO is null)
        {
            return ProjectsDTO.ToEmpty.ToModel();
        }

        return projectDTO.ToModel();
    }

    public async Task<ProjectModel[]> Get(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            cancellationToken.ThrowIfCancellationRequested();
        }

        return await Task.FromResult(_context.Projects.Select(p => p.ToModel()).ToArray());
    }

    public async Task<TaskModel[]> GetTasks(int projectId, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            cancellationToken.ThrowIfCancellationRequested();
        }

        return await Task.FromResult(_context.Tasks
            .Where(w => w.FkProjectId.Equals(projectId))
            .Select(p => p.ToModel())
        .ToArray());
    }
}
