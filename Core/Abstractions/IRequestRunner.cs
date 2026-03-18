using Core.Domain;

namespace Core.Abstractions;

public interface IRequestRunner
{
    Task<ResponseObject<IAsyncResult>> Run<T>(Func<Task<ResponseObject<T>>> func);
}