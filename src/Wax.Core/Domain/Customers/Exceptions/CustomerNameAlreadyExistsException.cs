using Wax.Core.Exceptions;
using Wax.Messages.Enums;

namespace Wax.Core.Domain.Customers.Exceptions
{
    public class CustomerNameAlreadyExistsException : BusinessException
    {
        public CustomerNameAlreadyExistsException() : base(ErrorCode.CustomerNameAlreadyExist,
            "Customer with this name already exists.")
        {
        }
    }
}