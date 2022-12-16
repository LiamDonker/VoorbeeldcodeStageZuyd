using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Etesian.WebApi.Domain.Models;
using Etesian.WebApi.Domain.DataModels;

#nullable disable

namespace Etesian.WebApi.Services.Data
{
    public partial class SiroccoContext : DbContext
    {
        public SiroccoContext()
        {
        }

        public SiroccoContext(DbContextOptions<SiroccoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeLeave> EmployeeLeaves { get; set; }
        public virtual DbSet<EmployeeLeaveDebit> EmployeeLeaveDebits { get; set; }
        public virtual DbSet<EmployeePartTimeFactor> EmployeePartTimeFactors { get; set; }
        public virtual DbSet<EmployeeProvision> EmployeeProvisions { get; set; }
        public virtual DbSet<EmployeeReservation> EmployeeReservations { get; set; }
        public virtual DbSet<EmployeeReservationDebit> EmployeeReservationDebits { get; set; }
        public virtual DbSet<FixedTimeCode> FixedTimeCodes { get; set; }
        public virtual DbSet<LeaveCode> LeaveCodes { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectEmployee> ProjectEmployees { get; set; }
        public virtual DbSet<TimeLog> TimeLogs { get; set; }
        public virtual DbSet<TimeType> TimeTypes { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoiceLine> InvoiceLines { get; set; }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=tcp:sql-bluepeopleit.database.windows.net,1433;Initial Catalog=sirocco-liam;Persist Security Info=False;User ID=bpitadmin;Password=ThisIsAP@ssword;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30; MultipleActiveResultSets=true;");
                //throw new ArgumentNullException("The connectionstring is missing...");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("Contact", "Sirocco");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Insertion).HasMaxLength(20);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PrimaryEmailAddress).HasMaxLength(50);

                entity.Property(e => e.PrimaryPhoneNumber).HasMaxLength(20);

                entity.Property(e => e.SecondaryEmailAddress).HasMaxLength(255);

                entity.Property(e => e.SecondaryPhoneNumber).HasMaxLength(20);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Contacts)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerId");
            });

        

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer", "Sirocco");

                entity.HasIndex(e => new { e.Name, e.Department }, "UC_NameDepartment")
                    .IsUnique();

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.Country)
                    .HasMaxLength(2)
                    .IsFixedLength(true);

                entity.Property(e => e.DebCredNr).HasMaxLength(10);

                entity.Property(e => e.Department).HasMaxLength(255);

                entity.Property(e => e.EmailAddress).HasMaxLength(255);

                entity.Property(e => e.HouseNumberAddition).HasMaxLength(20);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.PhoneNumber).HasMaxLength(20);

                entity.Property(e => e.PostalCode).HasMaxLength(10);

                entity.Property(e => e.Street).HasMaxLength(255);

                entity.Property(e => e.Website).HasMaxLength(4000);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee", "Sirocco");

                entity.HasIndex(e => e.EmployeeNo, "UC_EmployeeNo")
                    .IsUnique();

                entity.Property(e => e.AADObjectId)
                    .HasMaxLength(40)
                    .HasColumnName("AADObjectId");

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.Country)
                    .HasMaxLength(2)
                    .IsFixedLength(true);

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.EmergencyContact).HasColumnType("ntext");

                entity.Property(e => e.EmployeeNo).HasMaxLength(20);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Gender)
                    .HasMaxLength(1)
                    .IsFixedLength(true);

                entity.Property(e => e.HouseNumberAddition).HasMaxLength(20);

                entity.Property(e => e.Insertion).HasMaxLength(20);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MaritalStatus).HasMaxLength(20);

                entity.Property(e => e.Nationality)
                    .HasMaxLength(2)
                    .IsFixedLength(true);

                entity.Property(e => e.OfficialNames).HasMaxLength(255);

                entity.Property(e => e.PostalCode).HasMaxLength(10);

                entity.Property(e => e.PrimaryEmailAddress).HasMaxLength(255);

                entity.Property(e => e.PrimaryPhoneNumber)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.SecondaryEmailAddress).HasMaxLength(255);

                entity.Property(e => e.SecondaryPhoneNumber).HasMaxLength(20);

                entity.Property(e => e.Street).HasMaxLength(255);
            });

            modelBuilder.Entity<EmployeeLeave>(entity =>
            {
                entity.ToTable("Employee_Leave", "Sirocco");

                entity.Property(e => e.DateAssigned).HasColumnType("date");

                entity.Property(e => e.LeaveCode)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsFixedLength(true);

                entity.Property(e => e.ValidTill).HasColumnType("date");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeLeaves)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employee_Leave_Employee");

                entity.HasOne(d => d.LeaveCodeNavigation)
                    .WithMany(p => p.EmployeeLeaves)
                    .HasForeignKey(d => d.LeaveCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employee_Leave_LeaveCodes");
            });

            modelBuilder.Entity<EmployeeLeaveDebit>(entity =>
            {
                entity.ToTable("Employee_LeaveDebit", "Sirocco");

                entity.HasIndex(e => new { e.EmployeeLeaveId, e.TimeLogId }, "UC_EmployeeLeaveIdDebit")
                    .IsUnique();

                entity.Property(e => e.EmployeeLeaveId).HasColumnName("Employee_LeaveId");

                entity.Property(e => e.Amount).HasColumnType("decimal(4, 2)");

                entity.HasOne(d => d.EmployeeLeave)
                    .WithMany(p => p.EmployeeLeaveDebits)
                    .HasForeignKey(d => d.EmployeeLeaveId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employee_LeaveDebit_Employee_Leave");

                entity.HasOne(d => d.TimeLog)
                    .WithMany(p => p.EmployeeLeaveDebits)
                    .HasForeignKey(d => d.TimeLogId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employee_LeaveDebit_TimeLog");
            });

            modelBuilder.Entity<EmployeePartTimeFactor>(entity =>
            {
                entity.HasKey(e => new { e.Id });

                entity.ToTable("Employee_PartTimeFactor", "Sirocco");

                entity.Property(e => e.Factor).HasColumnType("decimal(3, 2)");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeePartTimeFactors)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employee_PartTimeFactor_Employee");
            });

            modelBuilder.Entity<EmployeeProvision>(entity =>
            {
                entity.HasKey(e => new { e.Id });

                entity.ToTable("Employee_Provision", "Sirocco");

                entity.Property(e => e.Amount).HasColumnType("decimal(4, 3)");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeProvisions)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employee_Provision_Employee");
            });

            modelBuilder.Entity<EmployeeReservation>(entity =>
            {
                entity.ToTable("Employee_Reservation", "Sirocco");

                entity.Property(e => e.DateAssigned).HasColumnType("date");

                entity.Property(e => e.ValidTill).HasColumnType("date");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeReservations)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employee_Reservation_Employee");
            });

            modelBuilder.Entity<EmployeeReservationDebit>(entity =>
            {
                entity.ToTable("Employee_ReservationDebit", "Sirocco");

                entity.HasIndex(e => new { e.EmployeeReservationId, e.Date }, "UC_ReservationDebitIdDate")
                    .IsUnique();

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.EmployeeReservationId).HasColumnName("Employee_ReservationId");

                entity.HasOne(d => d.EmployeeReservation)
                    .WithMany(p => p.EmployeeReservationDebits)
                    .HasForeignKey(d => d.EmployeeReservationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employee_ReservationDebit_Employee_Reservation");
            });

            modelBuilder.Entity<FixedTimeCode>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("FixedTimeCodes", "Sirocco");

                entity.Property(e => e.Code)
                    .HasMaxLength(2)
                    .IsFixedLength(true);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<LeaveCode>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("LeaveCodes", "Sirocco");

                entity.HasIndex(e => new { e.Code, e.Name }, "UC_CodeName")
                    .IsUnique();

                entity.Property(e => e.Code)
                    .HasMaxLength(2)
                    .IsFixedLength(true);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Project", "Sirocco");

                entity.HasIndex(e => e.Code, "UC_Code")
                    .IsUnique();

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.ContactId)
                    .HasConstraintName("FK_Project_Contact");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.ProjectCustomers)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Project_Customer");

                entity.HasOne(d => d.EndCustomer)
                    .WithMany(p => p.ProjectEndCustomers)
                    .HasForeignKey(d => d.EndCustomerId)
                    .HasConstraintName("FK_Project_EndCustomer");
            });

            modelBuilder.Entity<ProjectEmployee>(entity =>
            {
                entity.ToTable("ProjectEmployee", "Sirocco");

                entity.Property(e => e.DateFrom).HasColumnType("date");

                entity.Property(e => e.DateTo).HasColumnType("date");

                entity.Property(e => e.Tariff).HasColumnType("decimal(6, 2)");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.ProjectEmployees)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectEmployee_Employee");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectEmployees)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectEmployee_Project");
            });

            modelBuilder.Entity<TimeLog>(entity =>
            {
                entity.ToTable("TimeLog", "Sirocco");

                entity.Property(e => e.Amount).HasColumnType("decimal(4, 2)");

                entity.Property(e => e.ApprovedBum)
                    .HasColumnType("datetime")
                    .HasColumnName("ApprovedBUM");

                entity.Property(e => e.ApprovedEmployee).HasColumnType("datetime");

                entity.Property(e => e.Comment).HasMaxLength(255);

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.FixedTimeCode)
                    .HasMaxLength(2)
                    .IsFixedLength(true);

                entity.Property(e => e.Invoiced).HasColumnType("datetime");

                entity.Property(e => e.Provision).HasColumnType("decimal(18, 5)");

                entity.Property(e => e.Tariff).HasColumnType("decimal(6, 2)");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.TimeLogs)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TimeLog_Employee");

                entity.HasOne(d => d.FixedTimeCodeNavigation)
                    .WithMany(p => p.TimeLogs)
                    .HasForeignKey(d => d.FixedTimeCode)
                    .HasConstraintName("FK_TimeLog_FixedTimeCodes");

                entity.HasOne(d => d.ProjectEmployee)
                    .WithMany(p => p.TimeLogs)
                    .HasForeignKey(d => d.ProjectEmployeeId)
                    .HasConstraintName("FK_TimeLog_ProjectEmployee");

                entity.HasOne(d => d.TimeType)
                    .WithMany(p => p.TimeLogs)
                    .HasForeignKey(d => d.TimeTypeId)
                    .HasConstraintName("FK_TimeLog_TimeType");

                // Invoice Module ------------------------------------------------------------------------------------

                entity.Property(d => d.InvoiceLineId);

                entity.HasOne(d => d.InvoiceLine)
                    .WithMany(p => p.TimeLogs)
                    .HasForeignKey(d => d.InvoiceLineId)
                    .HasConstraintName("FK_TimeLog_InvoiceLine");

                // Invoice Module ------------------------------------------------------------------------------------
            });

            modelBuilder.Entity<TimeType>(entity =>
            {
                entity.ToTable("TimeType", "Sirocco");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Factor).HasColumnType("decimal(3, 2)");
            });

            // Invoice Module ------------------------------------------------------------------------------------             
            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.ToTable("Invoice", "Sirocco");

                entity.Property(e => e.HeaderText);

                entity.Property(e => e.InvoiceNumber);             

                entity.Property(e => e.Paid);

                entity.Property(e => e.Approved);

                entity.Property(e => e.CustomerId);

                entity.Property(e => e.Year);

                entity.Property(e => e.Month);

                entity.HasOne(e => e.Customer)
                    .WithMany(c => c.Invoices)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Invoice_Customer");
            });

            modelBuilder.Entity<InvoiceLine>(entity =>
            {
                entity.ToTable("InvoiceLine", "Sirocco");

                entity.Property(e => e.Description);

                entity.Property(e => e.InvoiceId);

                entity.Property(e => e.ProjectEmployeeId);

                entity.Property(e => e.Amount);

                entity.Property(e => e.Tariff);

                entity.HasOne(e => e.Invoice)
                    .WithMany(i => i.InvoiceLines)
                    .HasForeignKey(e => e.InvoiceId)
                    .HasConstraintName("FK_InvoiceLine_Invoice");

                entity.HasOne(e => e.ProjectEmployee)
                    .WithMany(p => p.InvoiceLines)
                    .HasForeignKey(e => e.ProjectEmployeeId)
                    .HasConstraintName("FK_InvoiceLine_ProjectEmployee");             
            });
            // Invoice Module ------------------------------------------------------------------------------------

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
