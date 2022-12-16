
using Etesian.WebApi.Domain.Interfaces.Data;
using Etesian.WebApi.Domain.Models;
using Etesian.WebApi.Services.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etesian.WebApi.Services.Data
{
    public class ProjectEmployeeService : IProjectEmployeeService
    {
        private readonly SiroccoContext _siroccoContext;
        public ProjectEmployeeService(SiroccoContext siroccoContext)
        {
            _siroccoContext = siroccoContext;
        }

        public async Task<ProjectEmployee> GetProjectEmployee(long id)
        {
            ProjectEmployee projectEmployee = await Task.Run(() => 
                _siroccoContext.Set<ProjectEmployee>()
                .Where(pe => pe.Id == id).FirstOrDefault()
            );
            return projectEmployee;
        }

        public async Task<List<ProjectEmployee>> GetProjectEmployees()
        {
            List<ProjectEmployee> projectEmployees = await Task.Run(() =>
                _siroccoContext.Set <ProjectEmployee>().ToList()
            );
            return projectEmployees;
        }

        public async Task<ProjectEmployee> Insert(ProjectEmployee projectEmployee)
        {
            _siroccoContext.Set<ProjectEmployee>().Add(projectEmployee);
             await Task.Run(() => 
                _siroccoContext.SaveChanges()
            );
            return projectEmployee;
        }

        public async Task Update(ProjectEmployee projectEmployee)
        {
            _siroccoContext.Set<ProjectEmployee>().Update(projectEmployee);
            await Task.Run(() =>
               _siroccoContext.SaveChanges()
           );
        }

        public async Task Delete(ProjectEmployee projectEmployee)
        {
            _siroccoContext.Set<ProjectEmployee>().Remove(projectEmployee);
            await Task.Run(() =>
               _siroccoContext.SaveChanges()
           ); ;
        }

        public async Task<List<ProjectEmployee>> GetProjectEmployeesForEmployee(long employeeId)
        {
            List<ProjectEmployee> projectEmployeesForEmployee = await Task.Run(() =>
                _siroccoContext.Set<ProjectEmployee>()
                .Where(pe => pe.EmployeeId == employeeId).ToList()
            );
            return projectEmployeesForEmployee;
        }

        public async Task<List<ProjectEmployee>> GetProjectEmployeesForCurrentWeek(long employeeId, DateTime start_date, DateTime end_date)
        {
            List<ProjectEmployee> projectEmployees = await Task.Run(() =>
                _siroccoContext.Set<ProjectEmployee>()
                .Where(pe => pe.EmployeeId == employeeId && 
                    (start_date <= pe.DateFrom && end_date >= pe.DateTo ||
                    start_date <= pe.DateTo && start_date >= pe.DateFrom ||
                    end_date <= pe.DateTo && end_date >= pe.DateFrom)
                    ).Include(x => x.Project).ToList()
            );
            return projectEmployees;
        }
    }
}
