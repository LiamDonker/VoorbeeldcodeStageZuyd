using System;
using System.Collections.Generic;

#nullable disable

namespace Etesian.WebApi.Domain.Models
{
    public partial class Project
    {
        public Project()
        {
            ProjectEmployees = new HashSet<ProjectEmployee>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long CustomerId { get; set; }
        public long? EndCustomerId { get; set; }
        public long? ContactId { get; set; }
        public bool Bonus { get; set; }
        public bool Overtime { get; set; }

        public virtual Contact Contact { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Customer EndCustomer { get; set; }
        public virtual ICollection<ProjectEmployee> ProjectEmployees { get; set; }
    }
}
