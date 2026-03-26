namespace API.Common;

public class ApiResponse<T>
{
    public bool Success { get; private set; }
    public string Message { get; private set; }
    public T? Data { get; private set; }
    public IEnumerable<string>? Errors { get; private set; }

    private ApiResponse() { }

    public static ApiResponse<T> Ok(T data, string message = "Request successful") =>
        new() { Success = true, Message = message, Data = data };

    public static ApiResponse<T> Fail(string message, IEnumerable<string>? errors = null) =>
        new() { Success = false, Message = message, Errors = errors };
}
