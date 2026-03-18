using Core.Domain;

namespace Core.Abstractions;

public interface IProjectRepository
{
    Task<ProjectModel> Create(ProjectModel projectModel, CancellationToken cancellationToken);

    Task<ProjectModel[]> Get(CancellationToken cancellationToken);

    Task<ProjectModel> Get(int projectId, CancellationToken cancellationToken);

    Task<bool> Delete(int projectId, CancellationToken cancellationToken);
}