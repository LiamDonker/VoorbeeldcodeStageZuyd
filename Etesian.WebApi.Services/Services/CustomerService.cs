using Etesian.WebApi.Domain.Interfaces.Data;
using Etesian.WebApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Etesian.WebApi.Services.Data
{
    public class CustomerService : ICustomerService
    {
        private readonly SiroccoContext _siroccoContext;

        public CustomerService(SiroccoContext siroccoContext)
        {
            _siroccoContext = siroccoContext;
        }

        public async Task<Customer> GetCustomer(long id)
        {
            Customer customer = await Task.Run(() =>
            _siroccoContext.Set<Customer>()
                .Include(x => x.ProjectCustomers)
                .Include(x => x.ProjectEndCustomers)
                .Include(x => x.Contacts)
                .Where(x => x.Id == id).FirstOrDefault()
            );
            return customer;
        }

        public async Task<List<Customer>> GetCustomers()
        {
            List<Customer> customers = await Task.Run(() => 
                _siroccoContext.Set<Customer>().ToList()
            );
            return customers;
        }

        public async Task<Customer> Insert(Customer customer)
        {
            _siroccoContext.Set<Customer>().Add(customer);
            await Task.Run(() => 
                _siroccoContext.SaveChanges()   
            );
            return customer;
        }

        public async Task Update(Customer customer)
        {
            _siroccoContext.Set<Customer>().Update(customer);
            await Task.Run(() => 
                _siroccoContext.SaveChanges()
            );
        }

        public async Task Delete(Customer customer)
        {
            _siroccoContext.Set<Customer>().Remove(customer);
            await Task.Run(() => 
                _siroccoContext.SaveChanges()
            );
        }
    }
}
