using Etesian.WebApi.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Etesian.WebApi.Domain.Interfaces.Data
{
    public interface ITimeTypeService
    {
        Task<TimeType> GetTimeType(long id);
        Task<List<TimeType>> GetTimeTypes();
    }
}
