using Etesian.WebApi.Domain.Models;
using Etesian.WebApi.Domain.POCO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Etesian.WebApi.Domain.Interfaces.Data
{
    public interface ITimeLogService
    {
        Task<TimeLog> GetTimeLog(long id);
        Task<List<TimeLog>> GetTimeLogs();
        Task<List<ProjectWeekLog>> GetTimeLogsForProjectWeek(long employeeId, DateTime start_date, DateTime end_date);
        Task<List<TimeLogProject>> GetTimeLogProjectByDate(long employeeId, DateTime start_date, DateTime end_date);
        Task<decimal> GetTotalHoursBooked(long employeeId, DateTime start_date, DateTime end_date);
        Task<decimal> GetOvertime(long employeeId, int year, int month);
        Task<long> Insert(TimeLog timeLog);
        Task Update(TimeLog timeLog);
        Task Delete(long timeLogId);
    }
}
