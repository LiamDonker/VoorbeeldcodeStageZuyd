using AutoMapper;
using Etesian.WebApi.Api.Controllers.v1.ViewModels;
using Etesian.WebApi.Domain.Interfaces.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etesian.WebApi.Api.Controllers.v1
{
    //[Authorize("AdminOnly")]
    [ApiController]
    [Route("api/v1/customers")]
    public class CustomerController : Controller
    {

        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CustomerController(IMapper mapper, ICustomerService customerService)
        {
            _mapper = mapper;
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            try
            {
                List<Domain.Models.Customer> customers = await _customerService.GetCustomers();
                if (customers == null || customers.Count == 0)
                {
                    return NoContent();
                }

                return Ok(customers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(long id)
        {
            try
            {
                Domain.Models.Customer customer = await _customerService.GetCustomer(id);
                if (customer == null)
                {
                    return NotFound($"Customer with id {id} has not been found.");
                }
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] Customer customer)
        {
            try
            {
                Domain.Models.Customer insertedCustomer = await _customerService.Insert(_mapper.Map<Domain.Models.Customer>(customer));
                // TODO: add uri;
                return Created("", insertedCustomer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Customer customer)
        {
            try
            {
                await _customerService.Update(_mapper.Map<Domain.Models.Customer>(customer));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] Customer customer)
        {
            try
            {
                await _customerService.Delete(_mapper.Map<Domain.Models.Customer>(customer));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
