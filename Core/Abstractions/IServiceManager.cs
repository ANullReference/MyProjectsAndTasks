using Domain;

namespace Core.Abstractions;

public interface IServiceManager
{
    Task<ResponseObject<Project[]>> GetProjects(CancellationToken cancellationToken);

    Task<ResponseObject<Project>> GetProjectById(int id, CancellationToken cancellationToken);

    Task<ResponseObject<bool>> DeleteProject(int projectId, CancellationToken cancellationToken);

    Task<ResponseObject<Project>> CreateProjects(Project projectModel, CancellationToken cancellationToken);

    #region tasks

    Task<ResponseObject<ProjectTask>> CreateTask(ProjectTask taskModel, CancellationToken cancellationToken);

    #endregion tasks
}