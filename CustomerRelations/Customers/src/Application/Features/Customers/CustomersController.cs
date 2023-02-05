using YourBrand.Customers.Application;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using YourBrand.Customers.Application.Features.Customers.Queries;
using YourBrand.Customers.Application.Common.Models;
using YourBrand.Customers.Application.Features.Organizations;
using YourBrand.Customers.Application.Features.Organizations.Queries;
using YourBrand.Customers.Application.Features.Customers;

namespace YourBrand.Customers.Application.Features.Customers;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
public class CustomersController
 : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ItemsResult<Customers.CustomerDto>>> GetCustomers(int page, int pageSize, string? searchString, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetCustomers(page, pageSize, searchString), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<CustomerDto?> GetCustomer(int id, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetCustomer(id), cancellationToken);
    }


    [HttpGet("GetCustomerBySsn/{ssn}")]
    public async Task<CustomerDto?> GetCustomerBySSN(string ssn, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetCustomerBySSN(ssn), cancellationToken);
    }
}