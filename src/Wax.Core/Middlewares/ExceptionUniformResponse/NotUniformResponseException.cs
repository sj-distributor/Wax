namespace Wax.Core.Middlewares.ExceptionUniformResponse;

public class NotUniformResponseException : Exception
{
    public NotUniformResponseException(Exception innerException) : base("Not implemented IUniformResponse",
        innerException)
    {
    }
}