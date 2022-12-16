using System;
using System.Collections.Generic;

#nullable disable

namespace Etesian.WebApi.Domain.Models
{
    public partial class Employee
    {
        public Employee()
        {
            EmployeeLeaves = new HashSet<EmployeeLeave>();
            EmployeePartTimeFactors = new HashSet<EmployeePartTimeFactor>();
            EmployeeProvisions = new HashSet<EmployeeProvision>();
            EmployeeReservations = new HashSet<EmployeeReservation>();
            ProjectEmployees = new HashSet<ProjectEmployee>();
            TimeLogs = new HashSet<TimeLog>();
        }

        public long Id { get; set; }
        public string EmployeeNo { get; set; }
        public string FirstName { get; set; }
        public string OfficialNames { get; set; }
        public string Insertion { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public string MaritalStatus { get; set; }
        public string PrimaryPhoneNumber { get; set; }
        public string SecondaryPhoneNumber { get; set; }
        public string PrimaryEmailAddress { get; set; }
        public string SecondaryEmailAddress { get; set; }
        public string EmergencyContact { get; set; }
        public string Street { get; set; }
        public short? HouseNumber { get; set; }
        public string HouseNumberAddition { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string AADObjectId { get; set; }

        public virtual ICollection<EmployeeLeave> EmployeeLeaves { get; set; }
        public virtual ICollection<EmployeePartTimeFactor> EmployeePartTimeFactors { get; set; }
        public virtual ICollection<EmployeeProvision> EmployeeProvisions { get; set; }
        public virtual ICollection<EmployeeReservation> EmployeeReservations { get; set; }
        public virtual ICollection<ProjectEmployee> ProjectEmployees { get; set; }
        public virtual ICollection<TimeLog> TimeLogs { get; set; }
    }
}
