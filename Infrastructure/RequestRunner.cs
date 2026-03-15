using Core.Abstractions;
using Core.Domain;


namespace Infrastructure;

public class RequestRunner : IRequestRunner
{
    public async Task<ResponseObject<IAsyncResult>> Run<T>(Func<Task<ResponseObject<T>>> func)
    {
        ResponseObject<T> response;

        try
        {
            response = await func.Invoke();
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
