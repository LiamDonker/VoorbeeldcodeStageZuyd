using Etesian.WebApi.Domain.Interfaces.Data;
using Etesian.WebApi.Domain.DataModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Etesian.WebApi.Services.Data;
using Etesian.WebApi.Domain.Models;

namespace Etesian.WebApi.Services.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly SiroccoContext _siroccoContext;

        public InvoiceService(SiroccoContext siroccoContext)
        {
            _siroccoContext = siroccoContext;
        }

        public async Task<List<Invoice>> GetInvoices()
        {
            var invoices = await Task.Run(() =>
                _siroccoContext.Set<Invoice>().Include(x => x.Customer).Include(x => x.InvoiceLines).ToList()
            );
            return invoices;
        }

        public async Task<Invoice> GetInvoice(long id)
        {
            Invoice invoice = await Task.Run(() =>
            _siroccoContext.Set<Invoice>().Include(x => x.InvoiceLines).ThenInclude(x => x.ProjectEmployee)
                                            .Include(x => x.InvoiceLines).ThenInclude(x => x.TimeLogs)
                                            .Include(x => x.Customer)
                                            .Where(x => x.Id == id).FirstOrDefault()
            );


            Invoice invoice2 = await Task.Run(() =>
            _siroccoContext.Set<Invoice>().Include(x => x.InvoiceLines)
                                            .Include(x => x.Customer)
                                            .Where(x => x.Id == id).FirstOrDefault()
            );

            //foreach (InvoiceLine invoiceline in invoice.InvoiceLines)
            //{
            //    invoiceline.Amount = invoiceline.TimeLogs.Sum(x => x.Amount);
            //    invoiceline.Tariff = invoiceline.TimeLogs.FirstOrDefault().Tariff;
            //}
            return invoice;
        }

        public async Task<Invoice> InsertInvoice(Invoice invoice)
        {
            _siroccoContext.Set<Invoice>().Add(invoice);
            await Task.Run(() =>
                _siroccoContext.SaveChanges()
            );
            return invoice;
        }

        public async Task UpdateInvoice(Invoice invoice)
        {
            _siroccoContext.Set<Invoice>().Update(invoice);
            await Task.Run(() =>
                _siroccoContext.SaveChanges()
            );
        }

        public async Task<List<int>> GetInvoiceNumbers()
        {
            List<int> invoiceNumbers = _siroccoContext.Set<Invoice>().Select(x => Convert.ToInt32(x.InvoiceNumber)).ToList();
            List<int> OpenNumbers = Enumerable.Range((int)invoiceNumbers.Min(), (int)invoiceNumbers.Max()).Except(invoiceNumbers).ToList();
            return OpenNumbers;
        }


        async public Task<List<InvoiceLine>> GetAllTimelogs(long customerid)
        {
            List<TimeLog> timeLogsGenerate = _siroccoContext.Set<TimeLog>().Include(x => x.ProjectEmployee).ThenInclude(z => z.Project).ThenInclude(y => y.Customer).Where(tl => tl.ProjectEmployee.Project.Customer.Id == customerid).ToList(); // Ophalen wanneer voorstel gegenereerd, op basis van klantnaam/klantid

            // TimeLogsGet wordt mogelijk vervangen door InvoiceLines
            List<TimeLog> timeLogsGet = _siroccoContext.Set<TimeLog>().Include(x => x.ProjectEmployee).ThenInclude(z => z.Project).ThenInclude(y => y.Customer).Where(tl => tl.InvoiceLine.Id == 123456).ToList(); // nummer omschrijven naar parameter. Hiermee kan je alle timelogs ophalen als ze aan een invoice gekoppeld zijn

            // We willen nu de timelogs gaan groeperen zodat ze als een regel op de factuur kunnen komen te staan.
            // We groeperen op basis van Medewerker die op een project heeft gezeten
            // Om dit makkelijker te maken hallen we eerst alle projecten op om hiermee te kunnen filteren
            List<ProjectEmployee> projectEmployees = _siroccoContext.Set<ProjectEmployee>().
                                                                        Include(z => z.Project).
                                                                        ThenInclude(y => y.Customer).
                                                                        Where(pe => pe.Project.Customer.Name.
                                                                        ToLower() == "Etesian IT Consulting BV.".ToLower()).
                                                                        ToList();

            // Het aanmaken van een lijst waarin we alle factuurregels kunnen zetten
            // een factuur regel is een omschrijving van de gemaakte kosten, het totaal aantal uren, het tarief, de totale kosten en een lijst van alle timelogs relevant voor de factuurregel
            List<InvoiceLine> invoiceLines = new List<InvoiceLine>();

            // We gaan nu alle "contracten" doorlopen van de klant.
            foreach (ProjectEmployee pe in projectEmployees)
            {
                // Timelogs ophalen die relevant zijn voor de huidige project employee
                List<TimeLog> pe_timelogs = timeLogsGenerate.Where(x => x.ProjectEmployeeId == pe.Id).ToList();
                // Controle of e timelogs zijn voor de project employee, als er geen zijn hoeven we geen factuurregel te maken
                if (pe_timelogs.Count() > 0)
                {
                    // Factuurregel opstellen op basis van de verzamelde timelogs
                    InvoiceLine line = new InvoiceLine();
                    line.TimeLogs = pe_timelogs;
                    line.ProjectEmployee = pe;
                    line.Description = "Project Yannic";       // veranderen
                    invoiceLines.Add(line);
                }
            }
            return invoiceLines;
        }

        async public Task<List<Invoice>> GenerateMonthlyInvoices(DateTime selectedDate)
        {
            List<Invoice> generatedInvoices = new List<Invoice>();
            List<int> openNumbers = await GetInvoiceNumbers();
            List<Customer> ActiveCustomers = new List<Customer>();

            foreach (TimeLog timelog in _siroccoContext.Set<TimeLog>().Include(x => x.ProjectEmployee).ThenInclude(x => x.Project).ThenInclude(x => x.Customer).ToList())
            {
                if(timelog.Date.Year == selectedDate.Year && timelog.Date.Month == selectedDate.Month)
                {
                    if (!ActiveCustomers.Contains(timelog.ProjectEmployee.Project.Customer))
                    {
                        ActiveCustomers.Add(timelog.ProjectEmployee.Project.Customer);
                    }
                }
            }

            foreach (Customer customer in ActiveCustomers) 
            {
                // Get all project employees
                List<ProjectEmployee> pe = _siroccoContext.Set<ProjectEmployee>().Include(x => x.TimeLogs)
                                                                                .Include(x => x.Employee)
                                                                                .Include(x => x.Project)
                                                                                .ThenInclude(x => x.Customer)
                                                                                .Where(x => x.TimeLogs
                                                                                .Where(x => x.Date >= new DateTime(selectedDate.Year, selectedDate.Month, 1)) // ophalen van gegevens voor de check
                                                                                .Count() > 0 && x.Project.Customer.Id == customer.Id
                                                                                ).ToList();
                

                //------------------------------------------------
                // Create Invoice
                //------------------------------------------------
                Invoice invoice = new Invoice();

                if (openNumbers.Count > 0)
                {
                    invoice.InvoiceNumber = openNumbers.First();
                    openNumbers.Remove(openNumbers.First());
                }
                else
                {
                    invoice.InvoiceNumber = _siroccoContext.Set<Invoice>().OrderBy(x => x.InvoiceNumber).Last().InvoiceNumber + 1;
                }

                invoice.HeaderText = "Beste meneer/mevrouw...";
                invoice.Paid = false;
                invoice.Approved = false;
                invoice.CustomerId = customer.Id;
                invoice.Customer = customer;
                invoice.Year = (short)DateTime.Now.Year;
                invoice.Month = (short)DateTime.Now.Month;

                //------------------------------------------------
                // Create InvoiceLines
                //------------------------------------------------
                invoice.InvoiceLines = new List<InvoiceLine>();
                foreach (ProjectEmployee projectEmployee in pe)
                {
                    InvoiceLine invoiceLine = new InvoiceLine();
                    invoiceLine.Description = $"Gewerkte uren {projectEmployee.Employee.FirstName.Substring(0, 1).ToUpper()}" +
                        $"" +
                        $"" +
                        $"" +
                        projectEmployee.Employee.LastName.Substring(0, 1).ToUpper() +
                        " in " + projectEmployee.TimeLogs.OrderBy(x => x.Date).First().Date.Month.ToString() +
                        " " + projectEmployee.TimeLogs.OrderBy(x => x.Date).First().Date.Year.ToString();
                    invoiceLine.InvoiceId = invoice.Id;
                    invoiceLine.Invoice = invoice;
                    invoiceLine.ProjectEmployeeId = projectEmployee.Id;
                    invoiceLine.ProjectEmployee = projectEmployee;
                    invoiceLine.Amount = 8; //placeholder, moet SUM van alle gewerkte uren zijn
                    invoiceLine.Tariff = projectEmployee.Tariff;

                    invoice.InvoiceLines.Add(invoiceLine);
                }
                await InsertInvoice(invoice);
            }
            return generatedInvoices;
        }
    }
}
