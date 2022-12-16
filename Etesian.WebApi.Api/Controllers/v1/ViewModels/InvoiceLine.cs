using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etesian.WebApi.Api.Controllers.v1.ViewModels
{
    public class InvoiceLine
    {
        public InvoiceLine()
        {
            TimeLogs = new HashSet<TimeLog>();
        }
        public long Id { get; set; }
        public string Description { get; set; }
        //[NotMapped]
        public decimal Amount { get; set; }
        //[NotMapped]
        public decimal Tariff { get; set; }
        public long InvoiceId { get; set; }
        public long ProjectEmployeeId { get; set; }

        public virtual ICollection<TimeLog> TimeLogs { get; set; }
        public Invoice Invoice { get; set; }
        public ProjectEmployee ProjectEmployee { get; set; }
    }
}
