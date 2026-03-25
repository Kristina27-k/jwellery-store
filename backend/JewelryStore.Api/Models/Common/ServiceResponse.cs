namespace JewelryStore.Api.Models.Common;

public class ServiceResponse<T>
{
    public T? Data { get; set; }
    public bool IsSuccess { get; set; } = true;
    public string Message { get; set; } = string.Empty;

    public static ServiceResponse<T> Success(T data, string message = "Success") => new() { Data = data, IsSuccess = true, Message = message };
    public static ServiceResponse<T> Failure(string message) => new() { IsSuccess = false, Message = message };
}
