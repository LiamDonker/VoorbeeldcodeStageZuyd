using System.ComponentModel.DataAnnotations;

namespace Etesian.WebApi.Api.Controllers.v1.ViewModels
{
    public class Customer
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Department { get; set; }

        [MaxLength(4000)]
        public string Website { get; set; }

        [EmailAddress]
        [MaxLength(255)]
        public string EmailAddress { get; set; }

        [Phone(ErrorMessage = "Must be a valid phonenumber")]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [MaxLength(255)]
        public string Street { get; set; }

        public short? HouseNumber { get; set; }

        [MaxLength(20)]
        public string HouseNumberAddition { get; set; }

        [MaxLength(10)]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }

        [MaxLength(50)]
        public string City { get; set; }

        [MaxLength(2, ErrorMessage = "Make use of the ISO-3166-1 Alpha-2 notation")]
        public string Country { get; set; }
        [MaxLength(10)]
        public string DebCredNr { get; set; }
    }
}
