using Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Abstractions;

public interface ITaskRepository
{
    Task<TaskModel> Create(TaskModel taskModel, CancellationToken cancellationToken);
    Task<TaskModel> Get(int taskId, CancellationToken cancellationToken);
    Task<TaskModel[]> GetByProjectId(int projectId, CancellationToken cancellationToken);
    Task<bool> Delete(int taskId, CancellationToken cancellationToken);
    Task<bool> DeleteByProjectId(int projectId, CancellationToken cancellationToken);
    Task<bool> DidIReachMaxTasksForAProjectId(int projectId, CancellationToken cancellationToken);
}
