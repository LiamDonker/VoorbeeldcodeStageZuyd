using AutoMapper;
using Etesian.WebApi.Api.Controllers.v1.ViewModels;
using Etesian.WebApi.Domain.Interfaces.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Etesian.WebApi.Domain.DataModels;

namespace Etesian.WebApi.Api.Controllers.v1
{
    //[Authorize("AdminOnly")]
    [ApiController]
    [Route("api/v1/invoices")]
    public class InvoiceController : Controller
    {   
        private readonly IInvoiceService _invoiceService;
        private readonly IMapper _mapper;

        public InvoiceController(IMapper mapper, IInvoiceService invoiceService)
        {
            _mapper = mapper;
            _invoiceService = invoiceService;
        }

        [HttpPost]
        public async Task<IActionResult> InsertInvoice([FromBody] ViewModels.Invoice invoice)
        {
            try
            {
                Domain.DataModels.Invoice insertedInvoice = await _invoiceService.InsertInvoice(_mapper.Map<Domain.DataModels.Invoice>(invoice));
                // TODO: add uri;
                return Created("", insertedInvoice);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }



        [HttpGet]
        public async Task<IActionResult> GetInvoices()
        {
            try
            {
                List<Domain.DataModels.Invoice> invoices = await _invoiceService.GetInvoices();
                if (invoices == null || invoices.Count == 0)
                {
                    return NoContent();
                }

                return Ok(invoices);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoice(long id)
        {
            try
            {
                Domain.DataModels.Invoice invoice = await _invoiceService.GetInvoice(id);
                if (invoice == null)
                {
                    return NotFound($"Invoice with id {id} has not been found.");
                }
                return Ok(invoice);
                //GetInvoiceDetails(invoice.CustomerId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateInvoice([FromBody] ViewModels.Invoice invoice)
        {
            try
            {
                await _invoiceService.UpdateInvoice(_mapper.Map<Domain.DataModels.Invoice>(invoice));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("invoice/{customerid}")]
        public async Task<IActionResult> GetInvoiceDetails(long customerid)     // get details of an invoice based on customer ID
        {
            try
            {
                List<Domain.DataModels.InvoiceLine> invoice = await _invoiceService.GetAllTimelogs(customerid);
                if (invoice == null)
                {
                    return NotFound($"Invoice with id {customerid} has not been found.");
                }
                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }


        [HttpPost("invoice/{selectedDate}")]
        public async Task<IActionResult> GenerateMonthlyInvoices(DateTime selectedDate)
        {
            //YYYY-MM-DD
            try
            {
                List<Domain.DataModels.Invoice> generatedInvoices = await _invoiceService.GenerateMonthlyInvoices(selectedDate);
                if (generatedInvoices == null)
                {
                    return NotFound("There are no timelogs available to genereate invoices from, try selecting a different date.");
                }
                return Created("", generatedInvoices);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

    }
}
