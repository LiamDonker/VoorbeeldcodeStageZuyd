using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Etesian.WebApi.Api.Controllers.v1.ViewModels
{
    public class ProjectEmployee
    {
        public long Id { get; set; }

        [Required]
        public long? ProjectId { get; set; }

        [Required]
        public long? EmployeeId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? DateFrom { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? DateTo { get; set; }

        [Required]
        [Column(TypeName = "Decimal(6, 2)")]
        public decimal? Tariff { get; set; }
    }
}
