using AutoMapper;
using Etesian.WebApi.Api.Controllers.v1.ViewModels;
using Etesian.WebApi.Domain.Exceptions;
using Etesian.WebApi.Domain.Interfaces.Data;
using Etesian.WebApi.Domain.Interfaces.v2;
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
    [Route("api/v1/time")]
    public class TimeController : Controller
    {
        private readonly ITimeService _timeService;
        private readonly ILeaveCodeService _leaveCodeService;
        private readonly IMapper _mapper;
        public TimeController(IMapper mapper, ITimeService timeService, ILeaveCodeService leaveCodeService)
        {
            _mapper = mapper;
            _timeService = timeService;
            _leaveCodeService = leaveCodeService;
        }

        [HttpGet("employees/{employeeId}/timeperiod")]
        public async Task<IActionResult> GetTimePeriod(long employeeId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] long? projectEmployeeId = null, [FromQuery] string leaveCode = null, [FromQuery] string fixedTimeCode = null)
        {
            try
            {
                var periodLog = new Domain.POCO.v2.PeriodLog();

                if (projectEmployeeId.HasValue)
                    periodLog = await _timeService.GetPeriodLogByProjectEmployeeId(employeeId, startDate, endDate, projectEmployeeId.Value);
                else if (!string.IsNullOrEmpty(leaveCode))
                    periodLog = await _timeService.GetPeriodLogByProjectLeaveCode(employeeId, startDate, endDate, leaveCode);
                else if (!string.IsNullOrEmpty(fixedTimeCode))
                    periodLog = await _timeService.GetPeriodLogByFixedTimeCode(employeeId, startDate, endDate, fixedTimeCode);
                else
                    periodLog = await _timeService.GetPeriodLog(employeeId, startDate, endDate);

                return Ok(periodLog);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut("employees/{employeeId}/timelogs")]
        public async Task<IActionResult> UpdateMultipleLogs(long employeeId, List<TimeLog> timeLogs)
        {
            try
            {
                var mappedLogs = _mapper.Map<List<TimeLog>, List<Domain.POCO.v2.SingleTimeLog>>(timeLogs);
                var test = await _timeService.UpdateTimeLogs(employeeId, mappedLogs);

                return Ok(test);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [ProducesResponseType(typeof(StandardHours), 200)]
        [ProducesResponseType(404)]
        [HttpGet("employees/{employeeId}/standardhours")]
        public async Task<IActionResult> GetEmployeeStandardHours(long employeeId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                return Ok(_mapper.Map<Domain.POCO.StandardHours, StandardHours>(await _timeService.GetStandardHours(employeeId, startDate, endDate)));

            }
            catch (SiroccoException ex)
            {
                return Problem(statusCode: 500, title: ex.ErrorMessage);
            }
        }

        [ProducesResponseType(typeof(List<Customer>), 200)]
        [ProducesResponseType(404)]
        [HttpGet("employees/{employeeId}/customers")]
        public async Task<IActionResult> GetPossibleCustomers(long employeeId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                return Ok(_mapper.Map<List<Domain.Models.Customer>, List<Customer>>(await _timeService.GetAllPossibleCustomers(employeeId, startDate, endDate)));

            }
            catch (SiroccoException ex)
            {
                return Problem(statusCode: 500, title: ex.ErrorMessage);
            }
        }

        [HttpGet("employees/{employeeId}/leaves/{date}")]
        public async Task<IActionResult> GetLeaves(long employeeId, DateTime date)
        {
            try
            {
                var allPossibleLeaves = await _timeService.GetAllPossibleLeaves(employeeId, date);
                var groupedEmployeeLeaves = allPossibleLeaves.GroupBy(x => x.LeaveCode);
                var leaveCodes = await _leaveCodeService.GetLeaveCodes();

                var possibleLeaves = new List<EmployeeLeaves>();

                // Add the leave name as well
                foreach (var grEL in groupedEmployeeLeaves)
                {
                    var possibleLeave = new EmployeeLeaves()
                    {
                        LeaveCode = grEL.Key,
                        LeaveDescription = leaveCodes.Where(x => x.Code == grEL.Key).FirstOrDefault().Description,
                        Leaves = new List<EmployeeLeave>()
                    };

                    possibleLeave.Leaves = grEL.Select(l => new EmployeeLeave()
                    {
                        Id = l.Id,
                        AmountLeft = l.AmountLeft,
                        OriginalAmount = l.OriginalAmount,
                        ValidTill = l.ValidTill,
                        DateAssigned = l.DateAssigned,
                        LeaveCode = l.LeaveCode
                    }).OrderBy(x=> x.ValidTill).ToList();

                    possibleLeave.TotalAmountOriginal = possibleLeave.Leaves.Sum(x => x.OriginalAmount).GetValueOrDefault();
                    possibleLeave.TotalAmountLeft = possibleLeave.Leaves.Sum(x => x.AmountLeft).GetValueOrDefault();

                    possibleLeaves.Add(possibleLeave);
                }

                possibleLeaves = possibleLeaves.OrderBy(x => x.LeaveDescription).ToList();

                return Ok(possibleLeaves);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }



    }
}
