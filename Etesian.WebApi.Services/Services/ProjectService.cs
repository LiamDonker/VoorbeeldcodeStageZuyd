

using Etesian.WebApi.Domain.Enums;
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
    public class ProjectService : IProjectService
    {
        private readonly SiroccoContext _siroccoContext;
        public ProjectService(SiroccoContext siroccoContext)
        {
            _siroccoContext = siroccoContext;
        }

        public async Task<Project> GetProject(long id)
        {
            Project project = await Task.Run(() =>
                _siroccoContext.Projects.AsQueryable()
                .Where(project => project.Id == id).Include(x => x.ProjectEmployees).ThenInclude(x => x.Employee).Include(x => x.Customer).Include(x => x.EndCustomer).FirstOrDefault()
            );
            UpdateProjectCustomer(project, HttpMethod.GET);
            return project;
        }

        public async Task<List<Project>> GetProjects()
        {
            List<Project> projects = await Task.Run(() =>
                _siroccoContext.Set<Project>().ToList()
            ); 
            foreach (Project project in projects)
            {
                UpdateProjectCustomer(project, HttpMethod.GET);
            }
            return projects;
        }

        public async Task<long> Insert(Project project)
        {
            UpdateProjectCustomer(project, HttpMethod.POST);
            _siroccoContext.Set<Project>().Add(project);
            await Task.Run(() =>
                _siroccoContext.SaveChanges()
            );
            return project.Id;
        }

        public async Task Update(Project project)
        {
            UpdateProjectCustomer(project, HttpMethod.POST);
            _siroccoContext.Set<Project>().Update(project);
            await Task.Run(() =>
                _siroccoContext.SaveChanges()
            );

        }

        public async Task Delete(Project project)
        {
            _siroccoContext.Set<Project>().Remove(project);
            await Task.Run(() =>
                _siroccoContext.SaveChanges()
            );
            return;
        }

        public async Task<List<Project>> GetCustomerProjects(long customerId)
        {
            List<Project> projects = await Task.Run(() =>
                _siroccoContext.Set<Project>()
                .Where(project => 
                    project.EndCustomerId == customerId ||
                    project.CustomerId == customerId
                ).ToList()
            );
            return projects;
        }

        public async Task<List<Project>> GetEmployeeProjects(long employeeId, DateTime? from, DateTime? to)
        {
            // Check dates. If not supplied, then set from to 2010 and to to 5 years in the future
            // To prevent huge calls in the future
            from = (from.HasValue) ? from.Value : new DateTime(DateTime.Now.Year - 10, 1, 1);
            to = (to.HasValue) ? from.Value : DateTime.Now.AddYears(5);
            List<Project> projects = await Task.Run(() =>
                 _siroccoContext.Set<Project>()
                 .Where(project =>
                     project.ProjectEmployees.Any(pe => 
                        pe.EmployeeId == employeeId &&
                        ((pe.DateFrom >= from && pe.DateFrom <= to) || 
                        (pe.DateFrom <= to && pe.DateTo >= to))
                    )
                 ).ToList()
            );
            return projects;
        }

        /// <summary>
        /// Converts the project to have the customer set on the correct variable.
        /// </summary>
        /// <param name="project"></param>
        /// <param name="httpMethod"></param>
        /// <returns></returns>
        public Project UpdateProjectCustomer(Project project, HttpMethod httpMethod)
        {
            //check for when requested Id is not available
            if (project != null)
            {
                //sets customer as endcustomer for front end displaying if there is only a customer and no end customer
                if (httpMethod == HttpMethod.GET && !project.EndCustomerId.HasValue && project.CustomerId > 0)
                {
                    project.EndCustomerId = project.CustomerId;
                    project.CustomerId = 0;
                    project.EndCustomer = project.Customer;
                    project.Customer = null;
                    return project;
                }

                //sets endcustomer as customer for database if there is only a endcustomer and no customer
                else if (httpMethod == HttpMethod.POST && project.EndCustomerId.HasValue && project.CustomerId == 0)
                {
                    project.CustomerId = (long)project.EndCustomerId;
                    project.EndCustomerId = null;
                    project.Customer = project.EndCustomer;
                    project.EndCustomer = null;
                    return project;
                }
            }
            return project;
        }

        public async Task<List<Project>> GetActiveCustomerProjects (long customerId)
        {
            List<Project> projects = await Task.Run(() =>
                _siroccoContext.Set<Project>().ToList()
            );

            foreach (Project project in projects)
            {
                if (project.StartDate < DateTime.Now && project.EndDate > DateTime.Now && project.CustomerId == customerId)
                {
                    projects.Add(project);
                }
            }
            return projects;
        }
    }
}
