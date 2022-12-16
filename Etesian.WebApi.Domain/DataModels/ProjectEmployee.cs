using Etesian.WebApi.Domain.Models;
using Etesian.WebApi.Domain.DataModels;
using System;
using System.Collections.Generic;

#nullable disable

namespace Etesian.WebApi.Domain.Models
{
    public partial class ProjectEmployee
    {
        public ProjectEmployee()
        {
            TimeLogs = new HashSet<TimeLog>();
            InvoiceLines = new HashSet<InvoiceLine>();
        }

        public long Id { get; set; }
        public long ProjectId { get; set; }
        public long EmployeeId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public decimal Tariff { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Project Project { get; set; }
        public virtual ICollection<TimeLog> TimeLogs { get; set; }
        public virtual ICollection<InvoiceLine> InvoiceLines { get; set; }
    }
}
