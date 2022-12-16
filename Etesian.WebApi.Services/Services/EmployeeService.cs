using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Etesian.WebApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Etesian.WebApi.Domain.Interfaces.Data;

namespace Etesian.WebApi.Services.Data
{
    public class EmployeeService: IEmployeeService
    {
        private readonly SiroccoContext _siroccoContext;

        public EmployeeService(SiroccoContext siroccoContext)
        {
            _siroccoContext = siroccoContext;
        }

        public async Task<List<Employee>> GetEmployees()
        {
            return await Task.Run(() =>
                _siroccoContext.Employees.ToListAsync()
            );
        }

        public async Task<Employee> GetEmployee(long id)
        {
            Employee employee = await Task.Run(() => 
                _siroccoContext.Set<Employee>().Where(x => x.Id == id)
                .Include(x => x.ProjectEmployees).ThenInclude(pe => pe.Project)
                .Include(x => x.EmployeeLeaves)
                .Include(x => x.EmployeeProvisions)
                .Include(x => x.EmployeeReservations)
                .Include(x => x.EmployeePartTimeFactors).FirstOrDefaultAsync()
            );
            return employee;
        }

        public async Task<Employee> GetEmployeeByObjectId(string oid)
        {
            Employee employee = await Task.Run(() => 
                _siroccoContext.Set<Employee>().Where(x => x.AADObjectId == oid)
               .Include(x => x.ProjectEmployees).ThenInclude(pe => pe.Project)
               .Include(x => x.EmployeeLeaves)
               .Include(x => x.EmployeeProvisions)
               .Include(x => x.EmployeeReservations)
               .Include(x => x.EmployeePartTimeFactors).FirstOrDefaultAsync()
           );
            return employee;
        }

        public async Task<Employee> GetEmployeeByEmail(string email)
        {
            Employee employee = await Task.Run(() => 
                _siroccoContext.Set<Employee>().Where(x => x.PrimaryEmailAddress.ToUpper() == email.ToUpper())
               .Include(x => x.ProjectEmployees).ThenInclude(pe => pe.Project)
               .Include(x => x.EmployeeLeaves)
               .Include(x => x.EmployeeProvisions)
               .Include(x => x.EmployeeReservations)
               .Include(x => x.EmployeePartTimeFactors).FirstOrDefault()
            );
            return employee;
        }

        public async Task<Employee> Insert(Employee employee)
        {
            _siroccoContext.Employees.Add(employee);
            await Task.Run(() =>
                _siroccoContext.SaveChangesAsync()
            );
            return employee;
        }

        public async Task Update(Employee employee)
        {
            _siroccoContext.Employees.Update(employee);
            await Task.Run(() =>
                _siroccoContext.SaveChangesAsync()
            );
        }

        public async Task Delete(Employee employee)
        {
            _siroccoContext.Set<Employee>().Remove(employee);
            await Task.Run(() =>
                _siroccoContext.SaveChangesAsync()
            );
        }

    }
}
