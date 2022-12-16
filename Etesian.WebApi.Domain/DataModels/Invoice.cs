using Etesian.WebApi.Domain.Models;
using Etesian.WebApi.Domain.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Etesian.WebApi.Domain.DataModels
{
    public partial class Invoice
    {
        public Invoice()
        {
            InvoiceLines = new HashSet<InvoiceLine>();
        }
        public long Id { get; set; }
        public long InvoiceNumber { get; set; }
        [Column(TypeName = "varchar(8000)")]
        [MaxLength]
        public string HeaderText { get; set; }
        public bool Paid { get; set; }
        public bool Approved { get; set; }
        public long CustomerId { get; set; }
        public Int16 Year { get; set; }
        public Int16 Month { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<InvoiceLine> InvoiceLines { get; set; }
    }
}
