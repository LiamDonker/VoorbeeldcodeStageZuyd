using Etesian.WebApi.Domain.Models;
using Etesian.WebApi.Domain.POCO;
using Etesian.WebApi.Domain.POCO.v2;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Etesian.WebApi.Domain.Interfaces.v2
{
    public interface ITimeService
    {
        Task<PeriodLog> GetPeriodLog(long employeeId, DateTime startDate, DateTime endDate);
        Task<bool> UpdateTimeLogs(long employeeId, List<SingleTimeLog> timeLogs);
        Task<List<EmployeeLeave>> GetAllPossibleLeaves(long employeeId, DateTime leaveDate);
        Task<StandardHours> GetStandardHours(long employeeId, DateTime startDate, DateTime endDate);
        Task<PeriodLog> GetPeriodLogByProjectEmployeeId(long employeeId, DateTime startDate, DateTime endDate, long projectEmployeeId);
        Task<PeriodLog> GetPeriodLogByProjectLeaveCode(long employeeId, DateTime startDate, DateTime endDate, string leaveCode);
        Task<PeriodLog> GetPeriodLogByFixedTimeCode(long employeeId, DateTime startDate, DateTime endDate, string fixedTimeCode);
        Task<List<Customer>> GetAllPossibleCustomers(long employeeId, DateTime startDate, DateTime endDate);
    }
}
