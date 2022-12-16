using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Etesian.WebApi.Api.Controllers.v1.ViewModels
{
    public class TimeLog
    {
        public long Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        [Required]
        [Column(TypeName = "Decimal(4, 2)")]
        public decimal? Amount { get; set; }

        [MaxLength(255)]
        public string Comment { get; set; }

        [Column(TypeName = "Decimal(6, 2)")]
        public decimal Tariff { get; set; }

        [Column(TypeName = "Decimal(18, 5)")]
        public decimal Provision { get; set; }

        [Required]
        public long EmployeeId { get; set; }

        public long? ProjectEmployeeId { get; set; }

        [MaxLength(2)]
        public string FixedTimeCode { get; set; }

        public long? TimeTypeId { get; set; }

        public string LeaveCode { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ApprovedEmployee { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ApprovedBum { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? Invoiced { get; set; }
    }
}
