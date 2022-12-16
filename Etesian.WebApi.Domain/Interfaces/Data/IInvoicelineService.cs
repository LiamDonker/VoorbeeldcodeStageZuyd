using Etesian.WebApi.Domain.Models;
using Etesian.WebApi.Domain.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Etesian.WebApi.Domain.Interfaces.Data
{
    public interface IInvoicelineService
    {
        Task<InvoiceLine> InsertInvoiceline(InvoiceLine invoiceLine);
        Task<List<InvoiceLine>> GetAllInvoiceLines(long invoiceId);
        Task UpdateInvoiceLine(InvoiceLine invoiceLine);
        Task DeleteInvoiceLine(InvoiceLine invoiceLine);
    }
}
