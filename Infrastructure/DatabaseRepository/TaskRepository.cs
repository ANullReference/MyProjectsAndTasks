using Core.Abstractions;
using Core.Domain;
using Infrastructure.DatabaseRepository.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace Infrastructure.DatabaseRepository;

/// <summary>
/// Provides methods for creating, retrieving, and deleting task entities in the application data store.
/// </summary>
/// <remarks>This repository encapsulates data access logic for tasks, including operations scoped to specific
/// projects. All methods are asynchronous and require a valid database context. Thread safety is determined by the
/// underlying ApplicationDbContext implementation.</remarks>
/// <param name="context">The database context used to access and manage task and project data.</param>
/// <seealso cref="ApplicationDbContext"/>
public class TaskRepository(ApplicationDbContext context) : ITaskRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<TaskModel> Create(TaskModel taskModel, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(taskModel, nameof(taskModel));

        int taskId;

        TasksDTO taskDTO = TasksDTO.FromModel(taskModel);
        using IDbContextTransaction transaction = _context.Database.BeginTransaction();

        try
        {
            //verify if i am at the 10 task limit before adding.
            _context.Tasks.Add(taskDTO);
            taskId = await _context.SaveChangesAsync(cancellationToken);
            transaction.Commit();
        }
        catch (Exception exception)
        {
            transaction.Rollback();
            Console.WriteLine($"Error saving task: {exception.Message}");
            throw;
        }

        return _context.Tasks.Find(taskId)?.ToModel() ?? throw new Exception("Failed to retrieve the added task.");
    }

    public async Task<TaskModel> Get(int taskId, CancellationToken cancellationToken)
    {
        TasksDTO taskDto = await _context.Tasks.FirstAsync(f => f.Id.Equals(taskId), cancellationToken);
        return taskDto.ToModel();
    }

    public async Task<bool> Delete(int taskId, CancellationToken cancellationToken)
    {
        if (taskId <= 0)
        {
            return false;
        }

        if (!_context.Tasks.Any(a => a.Id.Equals(taskId)))
        {
            return false;
        }

        int _ = await _context.Tasks.Where(w => w.Id.Equals(taskId)).ExecuteDeleteAsync(cancellationToken);

        return true;
    }

    public async Task<TaskModel[]> GetByProjectId(int projectId, CancellationToken cancellationToken)
    {
        if (!_context.Tasks.Any(a => a.FkProjectId.Equals(projectId)))
        {
            return [];
        }

        TaskModel[] taskModels = await _context.Tasks
            .Where(w => w.FkProjectId.Equals(projectId))
            .Select(s => s.ToModel())
            .ToArrayAsync(cancellationToken);

        return taskModels;
    }

    public async Task<bool> DeleteByProjectId(int projectId, CancellationToken cancellationToken)
    {
        if (!_context.Projects.Any(a => a.Id.Equals(projectId)))
        {
            return false;
        }

        int _ = await _context.Tasks
            .Where(w => w.FkProjectId.Equals(projectId))
            .ExecuteDeleteAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DidIReachMaxTasksForAProjectId(int projectId, CancellationToken cancellationToken)
    {
        ProjectsDTO? project = await _context.Projects.FirstOrDefaultAsync(f => f.Id.Equals(projectId)
            , cancellationToken: cancellationToken);

        if (project is not null && project.Tasks?.Count >= 10)//move this 10 to app settings
        {
            return true;
        }

        return false;
    }
}