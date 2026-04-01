using Core.Abstractions;
using Domain;

namespace Core;

public class ServiceManager(IProjectRepository projectRepository, ITaskRepository taskRepository) : IServiceManager
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly ITaskRepository _taskRepository = taskRepository;

    public async Task<ResponseObject<Project>> GetProjectById(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return new(
                data: null,
                message: "Invalid project ID.",
                responseType: ResponseType.ValidationError
            );
        }

        Project project = await _projectRepository.Get(id, cancellationToken);
        return new ResponseObject<Project>(project);
    }

    public async Task<ResponseObject<Project[]>> GetProjects(CancellationToken cancellationToken)
    {
        Project[] projects = await _projectRepository.Get(cancellationToken);

        if (projects == null || projects.Length == 0)
        {
            return new ResponseObject<Project[]>
            {
                Data = null,
                Message = "No projects found."
            };
        }

        return new ResponseObject<Project[]>(projects);
    }

    public async Task<ResponseObject<Project>> CreateProjects(Project projectModel, CancellationToken cancellationToken)
    {
        if (projectModel == null || string.IsNullOrWhiteSpace(projectModel.Name))
        {
            return new ResponseObject<Project>
            (
                data: null,
                message: "Invalid project data.",
                responseType: ResponseType.ValidationError
            );
        }

        try
        {
            Project addedProject = await _projectRepository.Create(projectModel, cancellationToken);
            return new ResponseObject<Project>(addedProject);
        }
        catch (Exception ex)
        {
            return new ResponseObject<Project>(

                data: null,
                message: $"An error occurred while adding the project: {ex.Message}",
                responseType: ResponseType.Error
            );
        }
    }

    public async Task<ResponseObject<bool>> DeleteProject(int projectId, CancellationToken cancellationToken)
    {
        if (projectId <= 0)
        {
            return new ResponseObject<bool>(
                data: false,
                message: "Invalid project ID.",
                responseType: ResponseType.ValidationError
            );
        }

        Project projectModel = await _projectRepository.Get(projectId, cancellationToken);

        if (projectModel is null)
        {
            return new ResponseObject<bool>(
                data: false,
                message: $"Project with ID {projectId} not found.",
                responseType: ResponseType.ValidationError
            );
        }

        bool result = await _projectRepository.Delete(projectId, cancellationToken);

        return new ResponseObject<bool>(result);
    }

    #region task

    public async Task<ResponseObject<ProjectTask>> CreateTask(ProjectTask taskModel, CancellationToken cancellationToken)
    {
        if (taskModel == null || string.IsNullOrWhiteSpace(taskModel.Title))
        {
            return new ResponseObject<ProjectTask>
            (
                data: null,
                message: "Invalid task data.",
                responseType: ResponseType.ValidationError
            );
        }

        try
        {
            if (await _taskRepository.DidIReachMaxTasksForAProjectId(taskModel.FkProjectId, cancellationToken))
            {
                return new ResponseObject<ProjectTask>(
                    data: null,
                    message: $"Limit reached for tasks for project id {taskModel.FkProjectId}",
                    responseType: ResponseType.ValidationError
                );
            }

            ProjectTask addedTask = await _taskRepository.Create(taskModel, cancellationToken);
            return new ResponseObject<ProjectTask>(addedTask);
        }
        catch (Exception ex)
        {
            return new ResponseObject<ProjectTask>(

                data: null,
                message: $"An error occurred while adding the task: {ex.Message}",
                responseType: ResponseType.Error
            );
        }
    }

    #endregion task
}