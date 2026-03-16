using Asp.Versioning;
using Core.Abstractions;
using Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyProjectsAndTasks.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class ProjectsController(IServiceManager _serviceManager) : BaseController
{
    [HttpGet]
    [Authorize(Policy = "ReadAccess")]
    [ProducesResponseType(typeof(ResponseObject<ProjectModel[]>), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("Get all projects")]
    [EndpointSummary("Get all projects")]

    public async Task<IActionResult> GetProjects(CancellationToken cancellationToken) => 
        await Run(() => _serviceManager.GetProjects(cancellationToken));

    [HttpGet("{projectId}")]
    [Authorize(Policy = "ReadAccess")]
    [ProducesResponseType(typeof(ResponseObject<ProjectModel[]>), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("Get a specific project by its id")]
    public async Task<IActionResult> GetProjectById(int projectId, CancellationToken cancellationToken) => 
        await Run(() => _serviceManager.GetProjectById(projectId, cancellationToken));

    [HttpPost()]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(ResponseObject<ProjectModel>), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("Get a specific project by its id")]
    public async Task<IActionResult> CreateProject(ProjectModel projectModel, CancellationToken cancellationToken) => 
        await Run(() => _serviceManager.CreateProjects(projectModel, cancellationToken));

    [HttpDelete("{projectId}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(ResponseObject<bool>), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("Delete a specific project by its id")]
    public async Task<IActionResult> DeleteProject(int projectId, CancellationToken cancellationToken) => 
        await Run(() => _serviceManager.DeleteProject(projectId, cancellationToken));
}