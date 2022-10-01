﻿using Wax.Core.DependencyInjection;
using Wax.Core.Domain.Customers;

namespace Wax.Core.Services.Customers
{
    public interface ICustomerDataProvider : IScopedDependency
    {
        Task<Customer> GetByIdAsync(Guid id);
        Task<bool> CheckIsUniqueNameAsync(string name);
        Task<Customer> AddAsync(Customer customer);
        Task<Customer> UpdateAsync(Customer customer);
    }
}