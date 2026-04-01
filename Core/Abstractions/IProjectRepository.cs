using Domain;

namespace Core.Abstractions;

public interface IProjectRepository
{
    Task<Project> Create(Project projectModel, CancellationToken cancellationToken);

    Task<Project[]> Get(CancellationToken cancellationToken);

    Task<Project> Get(int projectId, CancellationToken cancellationToken);

    Task<bool> Delete(int projectId, CancellationToken cancellationToken);
}