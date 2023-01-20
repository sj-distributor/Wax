using Wax.Messages.Enums;

namespace Wax.Core.Exceptions
{
    public class BusinessException : Exception
    {
        protected BusinessException(ErrorCode errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }

        public ErrorCode ErrorCode { get; }
    }
}