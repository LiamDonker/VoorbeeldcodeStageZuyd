using AutoMapper;
using Etesian.WebApi.Api.Controllers.v1.ViewModels;
using Etesian.WebApi.Domain.Interfaces.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Etesian.WebApi.Api.Controllers.v1
{
    //[Authorize("EmployeeOrAdmin")]
    [ApiController]
    [Route("api/v1/")]
    public class TimeLogController : Controller
    {
        private readonly ITimeLogService _timeLogService;
        private readonly IMapper _mapper;
        public TimeLogController(IMapper mapper, ITimeLogService timeLogService)
        {
            _mapper = mapper;
            _timeLogService = timeLogService;
        }

        [HttpGet("employees/{employeeId}/timelogs")]
        public async Task<IActionResult> GetTimeLogsForCurrentWeek(long employeeId, [FromQuery] DateTime start_date, [FromQuery] DateTime end_date)
        {
            try
            {
                List<Domain.POCO.ProjectWeekLog> projectWeekLogs = await _timeLogService.GetTimeLogsForProjectWeek(employeeId, start_date, end_date);

                if (projectWeekLogs == null || projectWeekLogs.Count == 0)
                {
                    return NoContent();
                }
                return Ok(projectWeekLogs);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("employees/{employeeId}/timelogprojects")]
        public async Task<IActionResult> GetTimeLogProjectByDate(long employeeId, [FromQuery] DateTime start_date, [FromQuery] DateTime end_date)
        {
            try
            {
                List<Domain.POCO.TimeLogProject> timeLogs = await _timeLogService.GetTimeLogProjectByDate(employeeId, start_date, end_date);
                return Ok(timeLogs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost("employees/{employeeId}/timelogs")]
        public async Task<IActionResult> Insert([FromBody] TimeLog timeLog)
        {
            try
            {
                long timeLogId = await _timeLogService.Insert(_mapper.Map<Domain.Models.TimeLog>(timeLog));
                return Ok(timeLogId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut("employees/{employeeId}/timelogs")]
        public async Task<IActionResult> Update([FromBody] TimeLog timeLog)
        {
            try
            {
                {
                    await _timeLogService.Update(_mapper.Map<Domain.Models.TimeLog>(timeLog));
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        //TODO: check if employeeId is needed, else delete it ==> Ja is nodig voor authorizatie
        [HttpDelete("employees/{employeeId}/timelogs/{id}")]
        public async Task<IActionResult> Delete(string employeeId, long id)
        {
            try
            {
                if (id != 0)
                {
                        await _timeLogService.Delete(id);
                        return Ok();
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("employees/{employeeId}/totalhoursbooked")]
        public async Task<IActionResult> totalHoursBooked(long employeeId, [FromQuery] DateTime start_date, [FromQuery] DateTime end_date)
        {
            try
            { 
                if (employeeId == 0 || start_date == default || end_date == default)
                {
                    return BadRequest("Invalid request due to missing parameters");
                }
                decimal totalHoursBooked = await _timeLogService.GetTotalHoursBooked(employeeId, start_date, end_date);
                return Ok(totalHoursBooked);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("employees/{employeeId}/overtime")]
        public async Task<IActionResult> GetOvertime(long employeeId, [FromQuery] int year, [FromQuery] int month)
        {
            try
            {
                if (employeeId == 0 || year == 0 || month == 0)
                {
                    return BadRequest("Invalid request due to missing parameters");
                }
                decimal overtime = await _timeLogService.GetOvertime(employeeId, year, month);
                return Ok(overtime);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
