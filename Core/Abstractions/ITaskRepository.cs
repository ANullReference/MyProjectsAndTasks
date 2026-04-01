using Domain;

namespace Core.Abstractions;

public interface ITaskRepository
{
    Task<ProjectTask> Create(ProjectTask taskModel, CancellationToken cancellationToken);

    Task<ProjectTask> Get(int taskId, CancellationToken cancellationToken);

    Task<ProjectTask[]> GetByProjectId(int projectId, CancellationToken cancellationToken);

    Task<bool> Delete(int taskId, CancellationToken cancellationToken);

    Task<bool> DeleteByProjectId(int projectId, CancellationToken cancellationToken);

    Task<bool> DidIReachMaxTasksForAProjectId(int projectId, CancellationToken cancellationToken);
}