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
    [Route("api/v1/employees")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public EmployeeController(IMapper mapper, IEmployeeService employeeService)
        {
            _mapper = mapper;
            _employeeService = employeeService;
        }

        //[Authorize("AdminOnly")]
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                List<Domain.Models.Employee> employees = await _employeeService.GetEmployees();
                if (employees == null || employees.Count() > 0)
                {
                    return Ok(employees);
                }
                else
                {
                    return NotFound("No items were found in the Database");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        //[Authorize("EmployeeOrAdmin")]
        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetEmployeeById(string employeeId, [FromQuery] bool useAAD = false)
        {
            try
            {
                Domain.Models.Employee employee = null;

                // Use the Employee Id which is assigned to the person by Etesian
                if (long.TryParse(employeeId, out long employeeIdInt))
                {
                    employee = await _employeeService.GetEmployee(employeeIdInt);
                }
                // Use the ObjectId which is assigned to the person by AAD
                else if (Guid.TryParse(employeeId, out Guid objectId))
                {
                    if (useAAD == false)
                    {
                        employee = await _employeeService.GetEmployeeByObjectId(objectId.ToString());
                    }
                    else
                    {
                        // TODO: Get the used from AAD.
                        // possibly use Graph. Check if there are any better options
                    }

                }
                // UserPrincipalName (email aad)
                else if (employeeId.Contains("@"))
                {
                    if (useAAD == false)
                    {
                        employee = await _employeeService.GetEmployeeByEmail(employeeId);
                    }
                    else
                    {
                        // TODO: get user info from AAD
                        //employee = await _msGraphService.GetUserAsEmployee(id);
                    }
                }

                if (employee == null)
                {
                    return NotFound($"Employee with id {employeeId} has not been found.");
                }
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        //[Authorize("AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] Employee employee)
        {
            try
            {
                Domain.Models.Employee insertedEmployee = await _employeeService.Insert(_mapper.Map<Domain.Models.Employee>(employee));
                // TODO: add uri;
                return Created("", insertedEmployee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        //[Authorize("AdminOnly")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Employee employee)
        {
            try
            {
                await _employeeService.Update(_mapper.Map<Domain.Models.Employee>(employee));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        //TODO: Rewrite to employeeId instead of object
        //YvdK: I still don't like the idea of deleting 'core' data objects from the database. So that is why it is commented out
        // Maybe have a look at it to somehow block it from being deleted a
        //[HttpDelete]
        //public async Task<IActionResult> Delete([FromBody] Employee employee)
        //{
        //    try
        //    {
        //        await _employeeService.Delete(_mapper.Map<Domain.Models.Employee>(employee));
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.ToString());
        //    }
        //}
    }
}
