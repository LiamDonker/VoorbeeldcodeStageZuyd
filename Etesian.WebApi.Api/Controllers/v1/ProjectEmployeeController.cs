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
    public class ProjectEmployeeController : Controller
    {
        private readonly IProjectEmployeeService _projectEmployeeService;
        private readonly IMapper _mapper;
        public ProjectEmployeeController(IMapper mapper, IProjectEmployeeService projectEmployeeService)
        {
            _mapper = mapper;
            _projectEmployeeService = projectEmployeeService;
        }

        [Authorize("AdminOnly")]
        [HttpGet("projectlinks")]
        public async Task<IActionResult> GetProjectEmployees()
        {
            try
            {
                List<Domain.Models.ProjectEmployee> projectEmployees = await _projectEmployeeService.GetProjectEmployees();
                if (projectEmployees == null || projectEmployees.Count == 0)
                {
                    return NotFound($"No project-employee relations have been found.");
                }
                return Ok(projectEmployees);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        // TODO: hoe deze te securen zonder employeeId?
        [HttpGet("projectlinks/{id}")]
        public async Task<IActionResult> GetProjectEmployee(long id)
        {
            try
            {
                Domain.Models.ProjectEmployee projectEmployee = await _projectEmployeeService.GetProjectEmployee(id);
                if (projectEmployee == null)
                {
                    return NotFound($"Project-Employee with id {id} has not been found.");
                }
                return Ok(projectEmployee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [Authorize("EmployeeOrAdmin")]
        [HttpGet("employees/{employeeId}/projectlinks")]
        public async Task<IActionResult> GetProjectEmployeeForEmployee(long employeeId, [FromQuery] DateTime? start_date, [FromQuery] DateTime? end_date)
        {
            try
            {
                List<Domain.Models.ProjectEmployee> projectEmployees;
                if (start_date != null && end_date != null)
                {
                    projectEmployees = await _projectEmployeeService.GetProjectEmployeesForCurrentWeek(employeeId, start_date.Value, end_date.Value);
                }
                else
                {
                    projectEmployees = await _projectEmployeeService.GetProjectEmployeesForEmployee(employeeId);
                }

                if (projectEmployees == null || projectEmployees.Count == 0)
                {
                    return NoContent();
                }
                return Ok(projectEmployees);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }


        // TODO: Check if employeeId can be deleted
        [Authorize("AdminOnly")]
        [HttpPost("employees/{employeeId}/projectlinks")]
        public async Task<IActionResult> Insert(long employeeId, [FromBody] ProjectEmployee projectEmployee)
        {
            try
            {
                Domain.Models.ProjectEmployee insertedProjectEmployee = await _projectEmployeeService.Insert(_mapper.Map<Domain.Models.ProjectEmployee>(projectEmployee));
                return Ok(insertedProjectEmployee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

        }

        // TODO: Check if employeeId can be deleted
        [Authorize("AdminOnly")]
        [HttpPut("employees/{employeeId}/projectlinks")]
        [HttpPut("projects/{projectId}/employeelinks")]
        public async Task<IActionResult> Update(long employeeId, [FromBody] ProjectEmployee projectEmployee)
        {
            try
            {
                await _projectEmployeeService.Update(_mapper.Map<Domain.Models.ProjectEmployee>(projectEmployee));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        // TODO: Check if projectEmployee object can be deleted
        // YvdK: should be deleted as we don't need the object to delete it.
        [Authorize("AdminOnly")]
        [HttpDelete("employees/{employeeId}/employeelinks")]
        public async Task<IActionResult> Delete(long employeeId, [FromBody] ProjectEmployee projectEmployee)
        {
            try
            {
                await _projectEmployeeService.Delete(_mapper.Map<Domain.Models.ProjectEmployee>(projectEmployee));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
