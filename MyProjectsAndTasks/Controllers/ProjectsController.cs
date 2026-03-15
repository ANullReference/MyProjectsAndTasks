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
    [ProducesResponseType(typeof(ProjectModel[]), 200)] 
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("Get all projects")]
    [EndpointSummary("Get all projects")]
    
    public async Task<IActionResult> GetProjects()
    {
        return await Run(() => _serviceManager.GetProjects());
    }

    [HttpGet("{projectId}")]
    [ProducesResponseType(typeof(ProjectModel[]), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("Get a specific project by its id")]
    public async Task<IActionResult> GetProjectById(int projectId)
    {
        return await Run(() => _serviceManager.GetProjectById(projectId));
    }

    [HttpPut("add")]
    [ProducesResponseType(typeof(ProjectModel), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("Get a specific project by its id")]
    public async Task<IActionResult> AddProject(ProjectModel projectModel)
    {
        return await Run(() => _serviceManager.AddProject(projectModel));
    }


    [HttpDelete("delete/{projectId})]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("Delete a specific project by its id")]
    public async Task<IActionResult> AddProject(int projectId)
    {
        return await Run(() => _serviceManager.DeleteProject(projectModel));
    }
}