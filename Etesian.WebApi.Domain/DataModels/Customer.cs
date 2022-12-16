using System;
using System.Collections.Generic;
using Etesian.WebApi.Domain.DataModels;

#nullable disable

namespace Etesian.WebApi.Domain.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Contacts = new HashSet<Contact>();
            ProjectCustomers = new HashSet<Project>();
            ProjectEndCustomers = new HashSet<Project>();
            Invoices = new HashSet<Invoice>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Website { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Street { get; set; }
        public short? HouseNumber { get; set; }
        public string HouseNumberAddition { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string DebCredNr { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Project> ProjectCustomers { get; set; }
        public virtual ICollection<Project> ProjectEndCustomers { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
