using Domain;
using Microsoft.AspNetCore.Mvc;

namespace MyProjectsAndTasks.Controllers;

public class BaseController : Controller
{
    /// <summary>
    /// Generic response handler for API endpoints.
    /// It executes the provided action, handles exceptions,
    /// and returns appropriate HTTP responses based on the result.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="action"></param>
    /// <returns></returns>
    public async Task<IActionResult> Run<T>(Func<Task<ResponseObject<T>>> action)
    {
        ResponseObject<T> response;

        try
        {
            response = await action.Invoke();
        }
        catch (Exception ex)
        {
            return Problem($"An error occurred while processing the request: {ex.Message}");
        }

        if (response.IsInvalid)
        {
            return BadRequest(response.Message);
        }

        if (!response.IsSuccess)
        {
            return NotFound(response.Message);
        }

        return Ok(response.Data);
    }
}