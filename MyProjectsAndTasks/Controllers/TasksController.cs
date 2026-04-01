using Asp.Versioning;
using Core.Abstractions;
using Domain;
using Microsoft.AspNetCore.Mvc;
using MyProjectsAndTasks.Models;

namespace MyProjectsAndTasks.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/projects/{projectId}/[controller]")]
[ApiVersion("1.0")]
public class TasksController(IServiceManager _serviceManager) : BaseController
{
    [HttpPost()]
    [ProducesResponseType(typeof(ResponseObject<ProjectTask>), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("create task")]
    public async Task<IActionResult> CreateTask(int projectId, TaskModel taskModel, CancellationToken cancellationToken)
    {
        ProjectTask task = new ProjectTask(0, taskModel.Title, projectId, taskModel.FkStatusId, taskModel.FkPriorityId, taskModel.CreatedDate);

        return await Run(() => _serviceManager.CreateTask(task, cancellationToken));
    }
}