using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etesian.WebApi.Api.Controllers.v1.ViewModels
{
    public class Invoice
    {
        
        public long Id { get; set; }
        public string HeaderText { get; set; }
        public long InvoiceNumber { get; set; }
        public bool Paid { get; set; }
        public bool Approved { get; set; }
        public long CustomerId { get; set; }
        public short Year { get; set; }     // short == int16
        public short Month { get; set; }

        public List<InvoiceLine> invoicelines { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
