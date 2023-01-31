namespace Wax.Core.Exceptions
{
    public class BusinessException : Exception
    {
        protected BusinessException(string message) : base(message)
        {
        }
    }
}
