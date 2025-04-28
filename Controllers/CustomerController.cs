using BXTecnologia.API.Models.Customer.DTO;
using BXTecnologia.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BXTecnologia.API.Controllers;

[ApiController]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPost("customers")]
    public async Task<IActionResult> Create([FromBody] CreateCustomerDTO request)
    {
        var response = await _customerService.CreateAsync(request);
        return Ok(response);
    }

    [HttpGet("customers/{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var customer = await _customerService.GetAsync(id);
        if (customer is null) return NotFound();

        return Ok(customer);
    }
    
    [HttpGet("customers")]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _customerService.GetAllAsync();
        return Ok(customers);
    }
    
    /*
    [HttpPut("customers/{id:guid}")]
    public async Task<IActionResult> Update(
        [FromMultiSource] UpdateCustomerDTO request)
    {
        var requestStarted = DateTime.UtcNow;
        var existingCustomer = await _customerService.GetAsync(request.Id);

        if (existingCustomer is null)
        {
            return NotFound();
        }

        var customer = request.ToCustomer();
        await _customerService.UpdateAsync(customer, requestStarted);

        var customerResponse = customer.ToCustomerResponse();
        return Ok(customerResponse);
    }
    */
    
    [HttpDelete("customers/{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var deleted = await _customerService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return Ok();
    }
}