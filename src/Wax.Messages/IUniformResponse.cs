using Mediator.Net.Contracts;
using Wax.Messages.Enums;

namespace Wax.Messages;

public interface IUniformResponse : IResponse
{
    public bool Success { get; set; }
    public ErrorReason Error { get; set; }
}

public interface IUniformResponse<T> : IUniformResponse
{
    T Data { get; set; }
}

public class UniformResponse : IUniformResponse
{
    protected UniformResponse()
    {
    }

    public bool Success { get; set; }
    public ErrorReason Error { get; set; }

    public static UniformResponse Succeed()
    {
        return new UniformResponse
        {
            Success = true,
            Error = null
        };
    }

    public static UniformResponse Failure(ErrorCode code, string message)
    {
        return new UniformResponse
        {
            Success = false,
            Error = new ErrorReason(code, message)
        };
    }
}

public class UniformResponse<T> : UniformResponse, IUniformResponse<T>
{
    private UniformResponse()
    {
    }

    public T Data { get; set; }

    public static UniformResponse<T> Succeed(T data)
    {
        return new UniformResponse<T>
        {
            Success = true,
            Data = data,
            Error = null
        };
    }
}

public class ErrorReason
{
    public ErrorReason(ErrorCode code, string message)
    {
        Code = code;
        Message = message;
    }

    public ErrorCode Code { get; }
    public string Message { get; }
}