using System;
using System.Collections.Generic;
using Etesian.WebApi.Domain.DataModels;

#nullable disable

namespace Etesian.WebApi.Domain.Models
{
    public partial class TimeLog
    {
        public TimeLog()
        {
            EmployeeLeaveDebits = new HashSet<EmployeeLeaveDebit>();
        }

        public long Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }
        public decimal Tariff { get; set; }
        public decimal Provision { get; set; }
        public long EmployeeId { get; set; }
        public long? ProjectEmployeeId { get; set; }
        public long? InvoiceLineId { get; set; }
        public string FixedTimeCode { get; set; }
        public long? TimeTypeId { get; set; }
        public DateTime? ApprovedEmployee { get; set; }
        public DateTime? ApprovedBum { get; set; }
        public DateTime? Invoiced { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual FixedTimeCode FixedTimeCodeNavigation { get; set; }
        public virtual ProjectEmployee ProjectEmployee { get; set; }
        public virtual TimeType TimeType { get; set; }
        public virtual ICollection<EmployeeLeaveDebit> EmployeeLeaveDebits { get; set; }
        public virtual InvoiceLine InvoiceLine { get; set; }
    }
}
