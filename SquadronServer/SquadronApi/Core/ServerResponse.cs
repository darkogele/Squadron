namespace SquadronApi.Core;

public class ServerResponse<T>
{
    public bool IsSuccess { get; set; }

    public string? Message { get; set; }

    public T? Data { get; set; }

    public bool IsPaginated { get; set; }

    public static ServerResponse<T> Success(T data, bool isPaginated = false) => new()
    {
        Data = data,
        IsSuccess = true,
        IsPaginated = isPaginated
    };

    public static ServerResponse<T> Failure(string error) => new()
    {
        IsSuccess = false,
        Message = error
    };
}