using Etesian.WebApi.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Etesian.WebApi.Domain.Interfaces.Data
{
    public interface ICustomerService
    {
        Task<Customer> GetCustomer(long id);
        Task<List<Customer>> GetCustomers();
        Task Update(Customer customer);
        Task<Customer> Insert(Customer customer);
        Task Delete(Customer customer);
    }
}
