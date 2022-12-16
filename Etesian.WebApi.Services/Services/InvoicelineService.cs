using Etesian.WebApi.Domain.Interfaces.Data;
using Etesian.WebApi.Domain.DataModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Etesian.WebApi.Services.Data;
using Etesian.WebApi.Domain.Models;

namespace Etesian.WebApi.Services.Services
{
    public class InvoicelineService : IInvoicelineService
    {
        private readonly SiroccoContext _siroccoContext;

        public InvoicelineService(SiroccoContext siroccoContext)
        {
            _siroccoContext = siroccoContext;
        }

        // Create
        public async Task<InvoiceLine> InsertInvoiceline(InvoiceLine invoiceline)
        {
            _siroccoContext.Set<InvoiceLine>().Add(invoiceline);
            await Task.Run(() =>
                _siroccoContext.SaveChanges()
            );
            return invoiceline;
        }

        // Read
        async public Task<List<InvoiceLine>> GetAllInvoiceLines(long invoiceId)
        {
            List<InvoiceLine> InvoiceLines = _siroccoContext.Set<InvoiceLine>().Where(i => i.Invoice.Id == invoiceId).ToList();
            return InvoiceLines;
        }

        // Update
        public async Task UpdateInvoiceLine(InvoiceLine invoiceline)
        {
            _siroccoContext.Set<InvoiceLine>().Update(invoiceline);
            await Task.Run(() =>
                _siroccoContext.SaveChanges()
            );
        }

        // Delete
        public async Task DeleteInvoiceLine(InvoiceLine invoiceLine)
        {
            //foreach invoicelineid in invoiceline.timelog   invoicelineid = null
            _siroccoContext.Set<InvoiceLine>().Remove(invoiceLine);
            await Task.Run(() =>
                _siroccoContext.SaveChanges()
            );
        }
    }
}
