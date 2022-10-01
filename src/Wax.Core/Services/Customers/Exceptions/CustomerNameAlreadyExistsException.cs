using Wax.Core.Exceptions;

namespace Wax.Core.Services.Customers.Exceptions
{
    public class CustomerNameAlreadyExistsException : BusinessException
    {
        public CustomerNameAlreadyExistsException() : base("Customer with this name already exists.")
        {
        }
    }
}