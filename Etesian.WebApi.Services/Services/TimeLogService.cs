using Etesian.WebApi.Domain.Enums;
using Etesian.WebApi.Domain.Interfaces.Data;
using Etesian.WebApi.Domain.Models;
using Etesian.WebApi.Domain.POCO;
using Etesian.WebApi.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etesian.WebApi.Services.Data
{
    public class TimeLogService : ITimeLogService
    {
        private readonly SiroccoContext _siroccoContext;
        private readonly ITimeTypeService _timeTypeService;
        private readonly IProjectEmployeeService _projectEmployeeService;
        private readonly IFixedTimeCodeService _fixedTimeCodeService;
        private readonly ILeaveCodeService _leaveCodeService;


        public TimeLogService(
            SiroccoContext siroccoContext,
            ITimeTypeService timeTypeService,
            IProjectEmployeeService projectEmployeeService,
            IFixedTimeCodeService fixedTimeCodeService,
            ILeaveCodeService leaveCodeService)
        {
            _siroccoContext = siroccoContext;
            _timeTypeService = timeTypeService;
            _projectEmployeeService = projectEmployeeService;
            _fixedTimeCodeService = fixedTimeCodeService;
            _leaveCodeService = leaveCodeService;
        }

        public async Task<TimeLog> GetTimeLog(long id)
        {
            return await Task.Run(() =>
                _siroccoContext.Set<TimeLog>().Where(x => x.Id == id).FirstOrDefault()
            );
        }

        public async Task<List<TimeLog>> GetTimeLogs()
        {
            return await Task.Run(() =>
                _siroccoContext.Set<TimeLog>().ToList()
            );
        }

        public async Task<List<ProjectWeekLog>> GetTimeLogsForProjectWeek(long employeeId, DateTime start_date, DateTime end_date)
        {
            List<TimeType> timeTypes = await _timeTypeService.GetTimeTypes();
            List<ProjectEmployee> projectEmployees = await _projectEmployeeService.GetProjectEmployeesForCurrentWeek(employeeId, start_date, end_date);

            List<ProjectWeekLog> projectWeekLogs = new List<ProjectWeekLog>();
            foreach (ProjectEmployee projectEmployee in projectEmployees)
            {
                foreach (TimeType timeType in timeTypes)
                {
                    List<TimeLog> timeLogs = await GetTimeLogs(employeeId, timeType.Id, projectEmployee.Id, start_date, end_date, null);
                    if (timeLogs.Count > 0)
                    {
                        ProjectWeekLog projectWeekLog = new ProjectWeekLog();
                        projectWeekLog.SelectListItem = new SelectListObject
                        {
                            Display = projectEmployee.Project.Name,
                            Type = SelectListTypeEnum.ProjectEmployee,
                            Value = projectEmployee
                        };
                        projectWeekLog.ProjectEmployeeId = projectEmployee.Id;
                        projectWeekLog.TimeTypeId = timeType.Id;
                        projectWeekLog.TimeLogs = timeLogs;
                        projectWeekLogs.Add(projectWeekLog);
                    }
                }
            }
            List<FixedTimeCode> fixedTimeCodes = await Task.Run(() => _fixedTimeCodeService.GetFixedTimeCodes());
            foreach (FixedTimeCode fixedTimeCode in fixedTimeCodes)
            {
                List<TimeLog> timeLogs = await GetTimeLogs(employeeId, null, null, start_date, end_date, fixedTimeCode.Code);
                if (timeLogs.Count > 0)
                {
                    ProjectWeekLog projectWeekLog = new ProjectWeekLog();
                    projectWeekLog.SelectListItem = new SelectListObject { Display = fixedTimeCode.Description, Type = SelectListTypeEnum.FixedTimeCode, Value = fixedTimeCode };
                    projectWeekLog.TimeLogs = timeLogs;
                    projectWeekLogs.Add(projectWeekLog);
                }
            }
            List<LeaveCode> leaveCodes = await _leaveCodeService.GetLeaveCodes();
            foreach (LeaveCode leaveCode in leaveCodes)
            {
                List<TimeLog> timelogs = await GetLeaveTimeLogs(employeeId, start_date, end_date, leaveCode.Code);
                if (timelogs.Count > 0)
                {
                    ProjectWeekLog LeaveWeekLog = new ProjectWeekLog();
                    LeaveWeekLog.SelectListItem = new SelectListObject
                    {
                        Display = leaveCode.Description,
                        Type = SelectListTypeEnum.LeaveCode,
                        Value = leaveCode,
                    };
                    LeaveWeekLog.TimeLogs = timelogs;
                    projectWeekLogs.Add(LeaveWeekLog);
                }
            }
            return projectWeekLogs;
        }

        public async Task<long> Insert(TimeLog timeLog)
        {
            timeLog = await CalculateProvision(timeLog);
            _siroccoContext.Set<TimeLog>().Add(timeLog);
            await Task.Run(() => _siroccoContext.SaveChanges());
            return timeLog.Id;

        }

        public async Task Update(TimeLog timeLog)
        {
            timeLog = await CalculateProvision(timeLog);
            _siroccoContext.Set<TimeLog>().Update(timeLog);
            await Task.Run(() => _siroccoContext.SaveChanges());
        }

        public async Task Delete(long timeLogId)
        {
            TimeLog timeLog = _siroccoContext.Set<TimeLog>().SingleOrDefault(x => x.Id == timeLogId);

            _siroccoContext.Set<TimeLog>().Remove(timeLog);
            await _siroccoContext.SaveChangesAsync();
        }

        private async Task<List<TimeLog>> GetTimeLogs(long employeeId, long? timeTypeId, long? projectEmployeeId, DateTime start_date, DateTime end_date, string fixedTimeCode)
        {
            List<TimeLog> timeLogs = await Task.Run(() =>
            _siroccoContext.Set<TimeLog>().Where(
                x => x.Date >= start_date &&
                x.Date <= end_date &&
                x.EmployeeId == employeeId &&
                ((x.TimeTypeId != null && x.TimeTypeId == timeTypeId && x.ProjectEmployeeId != null && x.ProjectEmployeeId == projectEmployeeId)
                || (x.FixedTimeCode != null && x.FixedTimeCode == fixedTimeCode)
            )).ToList<TimeLog>());
            return timeLogs;
        }

        private async Task<List<TimeLog>> GetLeaveTimeLogs(long employeeId, DateTime start_date, DateTime end_date, string leaveCode)
        {
            List<TimeLog> timeLogs = await Task.Run(() => (from timeLog in _siroccoContext.Set<TimeLog>()
                                                           join employeeLeaveDebit in _siroccoContext.Set<EmployeeLeaveDebit>()
                                                           on timeLog.Id equals employeeLeaveDebit.TimeLogId
                                                           join employeeLeave in _siroccoContext.Set<EmployeeLeave>()
                                                           on employeeLeaveDebit.EmployeeLeaveId equals employeeLeave.Id
                                                           where 
                                                               timeLog.Date >= start_date &&
                                                               timeLog.Date <= end_date &&
                                                               timeLog.EmployeeId == employeeId &&
                                                               timeLog.TimeTypeId == null &&
                                                               timeLog.ProjectEmployeeId == null &&
                                                               timeLog.FixedTimeCode == null &&
                                                               employeeLeave.LeaveCode == leaveCode
                                                           select timeLog).ToList());
            return timeLogs;
        }

        public async Task<decimal> GetTotalHoursBooked(long employeeId, DateTime start_date, DateTime end_date)
        {
            decimal bookedHours = await Task.Run(() =>
                _siroccoContext.TimeLogs.Where(x => x.EmployeeId == employeeId && x.Date >= start_date && x.Date <= end_date).Sum(x => x.Amount));
            return bookedHours;
        }

        // TODO: will need to rewrite as of 7-6-2021
        public async Task<decimal> GetOvertime(long employeeId, int year, int month)
        {
            // set Dates to first and last day of the month
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            // calculate the total hours worked based on the start and end date
            var test = GetTimeLogsForProjectWeek(employeeId, startDate, endDate).Result;
            decimal workedHours = test.Sum(x => x.TimeLogs.Sum(e => e.Amount));
            EmployeePartTimeFactor PTF = await Task.Run(() => _siroccoContext.Set<EmployeePartTimeFactor>().Where(x => x.EmployeeId == employeeId && x.Date <= startDate).OrderByDescending(y => y.Date).FirstOrDefault());

            // TODO: This should be removed as there should be a PTF assigned at all times [PBI-1322]
            if (PTF == null)
            {
                PTF = new EmployeePartTimeFactor
                {
                    Factor = 1
                };
            }

            // Calculate the total work hours required for the current employee
            decimal totalWorkHours = Workdays(startDate, endDate) * 8 * PTF.Factor;

            decimal overtime = workedHours - totalWorkHours;

            return overtime;
        }

        private int Workdays(DateTime startDate, DateTime endDate)
        {
            double workDays =
            1 + ((endDate - startDate).TotalDays * 5 -
            (startDate.DayOfWeek - endDate.DayOfWeek) * 2) / 7;

            if (endDate.DayOfWeek == DayOfWeek.Saturday) workDays--;
            if (startDate.DayOfWeek == DayOfWeek.Sunday) workDays--;

            return Convert.ToInt32(workDays);
        }

        private async Task<TimeLog> CalculateProvision(TimeLog tempTimeLog)
        {
            if (tempTimeLog.ProjectEmployeeId != null)
            {
                ProjectEmployee tempProjectEmployee = await Task.Run(() => _siroccoContext.Set<ProjectEmployee>().Where(x => x.Id == tempTimeLog.ProjectEmployeeId).FirstOrDefault());
                DateTime date = new DateTime(tempTimeLog.Date.Year, tempTimeLog.Date.Month, 1);
                EmployeeProvision provision = await Task.Run(() => _siroccoContext.Set<EmployeeProvision>()
                .Where(x => x.EmployeeId == tempTimeLog.EmployeeId && x.Date <= date).OrderByDescending(y => y.Date).FirstOrDefault());
                ;
                if (provision == null)
                {
                    provision = new EmployeeProvision
                    {
                        Amount = 0,
                    };
                }
                TimeType timeType = await Task.Run(() => _siroccoContext.Set<TimeType>().Where(x => x.Id == tempTimeLog.TimeTypeId).FirstOrDefault());
                tempTimeLog.Tariff = tempProjectEmployee.Tariff;
                tempTimeLog.Provision = ((tempTimeLog.Tariff * timeType.Factor) * provision.Amount * tempTimeLog.Amount);
            }
            return tempTimeLog;
        }

        public async Task<List<TimeLogProject>> GetTimeLogProjectByDate(long employeeId, DateTime start_date, DateTime end_date)
        {
            List<TimeLogProject> timeLogProjects = await Task.Run(() =>
            _siroccoContext.Set<TimeLog>().Where(x => x.EmployeeId == employeeId && x.Date >= start_date && x.Date <= end_date && x.ProjectEmployeeId != null)
            .Select(y => new TimeLogProject { 
                Id = y.Id,
                Date = y.Date,
                Amount = y.Amount,
                ProjectEmployeeId = y.ProjectEmployeeId.Value
            }).ToList());

            return timeLogProjects;
        }
    }
}
