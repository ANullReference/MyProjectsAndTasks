using Asp.Versioning;
using Core.Abstractions;
using Core.Domain;
using Microsoft.AspNetCore.Mvc;

namespace MyProjectsAndTasks.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/Projects/{projectId}/[controller]")]
[ApiVersion("1.0")]
public class TasksController(IServiceManager _serviceManager) : BaseController
{
    [HttpPost("create")]
    [ProducesResponseType(typeof(ResponseObject<TaskModel>), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("create task")]
    public async Task<IActionResult> CreateTask(int projectId, TaskModel taskModel, CancellationToken cancellationToken)
    {
        taskModel.FkProjectId = projectId;
        return await Run(() => _serviceManager.CreateTask(taskModel, cancellationToken));
    }
}
