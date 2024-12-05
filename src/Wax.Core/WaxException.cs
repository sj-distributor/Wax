namespace Wax.Core;

public class WaxException : Exception
{
    public WaxException()
    {
    }

    public WaxException(string message)
        : base(message)
    {
    }

    public WaxException(string messageFormat, params object[] args)
        : base(string.Format(messageFormat, args))
    {
    }

    public WaxException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}