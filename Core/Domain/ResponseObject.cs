
namespace Core.Domain;

public class ResponseObject<T>
{
    public T? Data { get; set; }
    public string? Message { get; set; }
    public ResponseType ResponseType { get; }
    public bool IsSuccess => Data != null && ResponseType.Equals(ResponseType.Success);
    public bool IsInvalid => ResponseType.Equals(ResponseType.ValidationError);

    public ResponseObject(T? data, string? message = "", ResponseType responseType = ResponseType.Success)
    {
        Data = data;
        Message = message;
        ResponseType = responseType;
    }

    public ResponseObject()
    {
    }
}