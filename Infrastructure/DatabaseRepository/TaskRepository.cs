using Core.Abstractions;
using Core.Domain;
using Infrastructure.DatabaseRepository.DTO;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DatabaseRepository;

public class TaskRepository(ApplicationDbContext context) : ITaskRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<TaskModel> Add(TaskModel taskModel, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(taskModel, nameof(taskModel));

        int taskId;

        TasksDTO taskDTO = TasksDTO.FromModel(taskModel);

        try
        {
            _context.Tasks.Add(taskDTO);
            taskId = await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Error saving task: {exception.Message}");
            throw new Exception("Failed save task in database");
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

        int _  = await _context.Tasks
            .Where(w => w.FkProjectId.Equals(projectId))            
            .ExecuteDeleteAsync(cancellationToken);

        return true;
    }
}