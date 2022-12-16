using System.ComponentModel.DataAnnotations;

namespace Etesian.WebApi.Api.Controllers.v1.ViewModels
{
    public class Contact
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(20)]
        public string Insertion { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [EmailAddress]
        [MaxLength(50)]
        public string PrimaryEmailAddress { get; set; }

        [EmailAddress]
        [MaxLength(50)]
        public string SecondaryEmailAddress { get; set; }

        [Phone]
        [MaxLength(20)]
        public string PrimaryPhoneNumber { get; set; }

        [Phone]
        [MaxLength(20)]
        public string SecondaryPhoneNumber { get; set; }

        public long? CustomerId { get; set; }

        [Required]
        public bool IsPrimaryContact { get; set; }
    }
}
