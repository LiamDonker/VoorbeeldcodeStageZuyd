using Etesian.WebApi.Domain.Models;
using Etesian.WebApi.Domain.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Etesian.WebApi.Domain.Interfaces.Data
{
    public interface IInvoiceService
    {
        Task<List<Invoice>> GetInvoices();
        Task<Invoice> GetInvoice(long id);
        public Task<List<InvoiceLine>> GetAllTimelogs(long id);
        Task UpdateInvoice(Invoice invoice);
        Task<Invoice> InsertInvoice(Invoice invoice);

        Task<List<Invoice>> GenerateMonthlyInvoices(DateTime selectedDate);
        //public Task<Invoice> GetInvoiceDetails(long id);

    }
}
