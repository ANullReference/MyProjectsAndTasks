using Core.Abstractions;
using Core.Domain;

namespace Core;

public class ServiceManager (IProjectRepository projectRepository, ITaskRepository taskRepository) : IServiceManager
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly ITaskRepository _taskRepository = taskRepository;
    public async Task<ResponseObject<ProjectModel>> GetProjectById(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return new (
                data: null,
                message: "Invalid project ID.",
                responseType: ResponseType.ValidationError
            );
        }

        ProjectModel project = await _projectRepository.Get(id, cancellationToken);
        return new ResponseObject<ProjectModel>(project);
    }

    public async Task<ResponseObject<ProjectModel[]>> GetProjects(CancellationToken cancellationToken)
    {
        ProjectModel[] projects = await _projectRepository.Get(cancellationToken);
        
        if (projects == null || projects.Length == 0)
        {
            return new ResponseObject<ProjectModel[]>
            {
                Data = null,
                Message = "No projects found."
            };
        }
        
        return new ResponseObject<ProjectModel[]>(projects);
    }


    public async Task<ResponseObject<ProjectModel>> CreateProjects(ProjectModel projectModel, CancellationToken cancellationToken)
    {
        if (projectModel == null || string.IsNullOrWhiteSpace(projectModel.Name))
        {
            return new ResponseObject<ProjectModel>
            (
                data: null,
                message : "Invalid project data.",
                responseType:  ResponseType.ValidationError
            );
        }
    
        try
        {
            ProjectModel addedProject = await _projectRepository.Create(projectModel, cancellationToken);
            return new ResponseObject<ProjectModel>(addedProject);
        }
        catch (Exception ex)
        {
            return new ResponseObject<ProjectModel>(
            
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
            return new ResponseObject<bool> (
                data: false,
                message: "Invalid project ID.",
                responseType: ResponseType.ValidationError
            );
        }

        bool result = await _projectRepository.Delete(projectId, cancellationToken);

        return new ResponseObject<bool>(result);
    }



    #region task

    public async Task<ResponseObject<TaskModel>> CreateTask(TaskModel taskModel, CancellationToken cancellationToken)
    {
        if (taskModel == null || string.IsNullOrWhiteSpace(taskModel.Title))
        {
            return new ResponseObject<TaskModel>
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
                return new ResponseObject<TaskModel>(
                    data: null,
                    message: $"Limit reached for tasks for project id {taskModel.FkProjectId}",
                    responseType: ResponseType.ValidationError
                );
            }

            TaskModel addedTask = await _taskRepository.Create(taskModel, cancellationToken);
            return new ResponseObject<TaskModel>(addedTask);
        }
        catch (Exception ex)
        {
            return new ResponseObject<TaskModel>(

                data: null,
                message: $"An error occurred while adding the task: {ex.Message}",
                responseType: ResponseType.Error
            );
        }
    }

    #endregion
}
