using Asp.Versioning;
using Core.Abstractions;
using Core.Domain;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace MyProjectsAndTasks.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class ProjectsController(IServiceManager _serviceManager) : BaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(ResponseObject<ProjectModel[]>), 200)] 
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("Get all projects")]
    [EndpointSummary("Get all projects")]
    
    public async Task<IActionResult> GetProjects(CancellationToken cancellationToken)
    {
        return await Run(() => _serviceManager.GetProjects(cancellationToken));
    }

    [HttpGet("{projectId}")]
    [ProducesResponseType(typeof(ResponseObject<ProjectModel[]>), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("Get a specific project by its id")]
    public async Task<IActionResult> GetProjectById(int projectId, CancellationToken cancellationToken)
    {
        return await Run(() => _serviceManager.GetProjectById(projectId, cancellationToken));
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(ResponseObject<ProjectModel>), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("Get a specific project by its id")]
    public async Task<IActionResult> CreateProject(ProjectModel projectModel, CancellationToken cancellationToken)
    {
        return await Run(() => _serviceManager.CreateProjects(projectModel, cancellationToken));
    }


    [HttpDelete("delete/{projectId}")]
    [ProducesResponseType(typeof(ResponseObject<bool>), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("Delete a specific project by its id")]
    public async Task<IActionResult> AddProject(int projectId, CancellationToken cancellationToken)
    {
        return await Run(() => _serviceManager.DeleteProject(projectId, cancellationToken));
    }
}