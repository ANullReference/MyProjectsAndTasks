using Core.Domain;

namespace Core.Abstractions;

public interface IServiceManager
{
    Task<ResponseObject<ProjectModel[]>> GetProjects(CancellationToken cancellationToken);
    Task<ResponseObject<ProjectModel>> GetProjectById(int id, CancellationToken cancellationToken);
    Task<ResponseObject<bool>> DeleteProject(int projectId, CancellationToken cancellationToken);
    Task<ResponseObject<ProjectModel>> AddProject(ProjectModel projectModel, CancellationToken cancellationToken);
}