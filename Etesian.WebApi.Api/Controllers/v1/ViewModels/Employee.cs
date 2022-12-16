using System;
using System.ComponentModel.DataAnnotations;

namespace Etesian.WebApi.Api.Controllers.v1.ViewModels
{
    public class Employee
    {
        public long Id { get; set; }
        [MaxLength(20)]
        public string EmployeeNo { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(255)]
        public string OfficialNames { get; set; }
        [MaxLength(20)]
        public string Insertion { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        [MaxLength(1, ErrorMessage = "Either 'M' or 'F'")]
        public string Gender { get; set; }
        [MaxLength(2, ErrorMessage = "Use the ISO-3166-1 Alpha-2 notation")]
        public string Nationality { get; set; }
        [MaxLength(20)]
        public string MaritalStatus { get; set; }
        [Phone]
        [Required]
        [MaxLength(20)]
        public string PrimaryPhoneNumber { get; set; }
        [Phone]
        [MaxLength(20)]
        public string SecondaryPhoneNumber { get; set; }
        [EmailAddress]
        [MaxLength(255)]
        public string PrimaryEmailAddress { get; set; }
        [EmailAddress]
        [MaxLength(255)]
        public string SecondaryEmailAddress { get; set; }
        [EmailAddress]
        [MaxLength(255)]
        public string EmergencyContact { get; set; }
        [MaxLength(255)]
        public string Street { get; set; }
        public short? HouseNumber { get; set; }
        [MaxLength(20)]
        public string HouseNumberAddition { get; set; }
        [MaxLength(10)]
        public string PostalCode { get; set; }
        [MaxLength(50)]
        public string City { get; set; }
        [MaxLength(2, ErrorMessage = "Make use of the ISO-3166-1 Alpha-2 notation")]
        public string Country { get; set; }
        [MaxLength(40)]
        public string AADObjectId { get; set; }
    }
}
