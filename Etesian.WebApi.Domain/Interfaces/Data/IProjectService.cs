using Etesian.WebApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Etesian.WebApi.Domain.Interfaces.Data
{
    public interface IProjectService
    {
        Task<Project> GetProject(long id);
        Task<List<Project>> GetProjects();
        Task<List<Project>> GetCustomerProjects(long customerId);
        Task<List<Project>> GetEmployeeProjects(long employeeId, DateTime? from, DateTime? to);
        Task<long> Insert(Project project);
        Task Update(Project project);
        Task Delete(Project project);
        Task<List<Project>> GetActiveCustomerProjects(long customerId);
    }
}
