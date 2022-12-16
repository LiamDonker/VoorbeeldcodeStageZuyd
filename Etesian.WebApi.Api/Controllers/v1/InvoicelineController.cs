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
    [Route("api/v1/invoicelines")]
    public class InvoicelineController : Controller
    {
        private readonly IInvoicelineService _invoicelineService;
        private readonly IMapper _mapper;

        public InvoicelineController(IMapper mapper, IInvoicelineService invoicelineService)
        {
            _mapper = mapper;
            _invoicelineService = invoicelineService;
        }




        // Create
        [HttpPost("invoices/{invoiceid}/invoicelines")]
        public async Task<IActionResult> InsertInvoiceline([FromBody] ViewModels.InvoiceLine invoiceline)
        {
            try
            {
                Domain.DataModels.InvoiceLine insertedInvoiceline = await _invoicelineService.InsertInvoiceline(_mapper.Map<Domain.DataModels.InvoiceLine>(invoiceline));
                // TODO: add uri;
                return Created("", insertedInvoiceline);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        // Read
        [HttpGet("{invoiceid}")]
        public async Task<IActionResult> GetAllInvoiceLines(long invoiceid)
        {
            try
            {
                List<Domain.DataModels.InvoiceLine> invoicelines = await _invoicelineService.GetAllInvoiceLines(invoiceid);
                if (invoicelines == null || invoicelines.Count == 0)
                {
                    return NoContent();
                }

                return Ok(invoicelines);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        // Update
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ViewModels.InvoiceLine invoiceline)
        {
            try
            {
                await _invoicelineService.UpdateInvoiceLine(_mapper.Map<Domain.DataModels.InvoiceLine>(invoiceline));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        // Delete
        [HttpDelete]
        public async Task<IActionResult> DeleteInvoiceLine([FromBody] ViewModels.InvoiceLine invoiceline)
        {
            try
            {
                await _invoicelineService.DeleteInvoiceLine(_mapper.Map<Domain.DataModels.InvoiceLine>(invoiceline));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
