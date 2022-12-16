using AutoMapper;
using Etesian.WebApi.Api.Controllers.v1.ViewModels;
using Etesian.WebApi.Domain.Interfaces.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etesian.WebApi.Api.Controllers.v1
{
    //[Authorize]
    [ApiController]
    [Route("api/v1/")]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;
        public ProjectController(IMapper mapper, IProjectService projectService)
        {
            _mapper = mapper;
            _projectService = projectService;
        }

        [Authorize("AdminOnly")]
        [HttpGet("projects")]
        public async Task<IActionResult> GetAllProjects()
        {
            try
            {
                List<Domain.Models.Project> projects = await _projectService.GetProjects();
                if (projects == null || projects.Count == 0)
                {
                    return NoContent();
                }

                return Ok(projects);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [Authorize("AdminOnly")]
        [HttpGet("customers/{customerId}/projects")]
        public async Task<IActionResult> GetProjectsForCustomer(long customerId)
        {
            try
            {
                List<Domain.Models.Project> projects = await _projectService.GetCustomerProjects(customerId);
                if (projects == null || projects.Count == 0)
                {
                    return NotFound($"No projects have been found.");
                }
                return Ok(projects);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [Authorize("EmployeeOrAdmin")]
        [HttpGet("employees/{employeeId}/projects")]
        public async Task<IActionResult> GetEmployeeProjects([FromRoute] long employeeId, [FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
        {
            try
            {

                List<Domain.Models.Project> projects = await _projectService.GetEmployeeProjects(employeeId, fromDate, toDate);

                if (projects == null || projects.Count == 0)
                {
                    return NoContent();
                }
                return Ok(projects);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [Authorize("AdminOnly")]
        [HttpGet("projects/{id}")]
        public async Task<IActionResult> GetProjectById(long id)
        {
            try
            {
                Domain.Models.Project project = await _projectService.GetProject(id);

                if (project == null)
                {
                    return NotFound($"Project with id {id} has not been found.");
                }
                return Ok(project);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [Authorize("AdminOnly")]
        [HttpPost("projects")]
        public async Task<IActionResult> Insert([FromBody] Project project)
        {
            try
            {
                long projectId = await _projectService.Insert(_mapper.Map<Domain.Models.Project>(project));
                return Ok(projectId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [Authorize("AdminOnly")]
        [HttpPut("projects")]
        public async Task<IActionResult> Update([FromBody] Project project)
        {
            try
            {
                await _projectService.Update(_mapper.Map<Domain.Models.Project>(project));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [Authorize("AdminOnly")]
        [HttpDelete("projects")]
        public async Task<IActionResult> Delete([FromBody] Project project)
        {
            try
            {

                await _projectService.Delete(_mapper.Map<Domain.Models.Project>(project));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("projects/customer/{customerId}")]
        public async Task<IActionResult> GetActiveCustomerProjects([FromBody] long customerId)
        {
            try
            {
                List<Domain.Models.Project> projects = await _projectService.GetActiveCustomerProjects(customerId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
