using YourBrand.Customers.Application;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using YourBrand.Customers.Application.Customers.Queries;
using YourBrand.Customers.Application.Common.Models;

namespace YourBrand.Customers.Presentation.Controllers;

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
    public async Task<ActionResult<ItemsResult<Application.Customers.CustomerDto>>> GetCustomers(int page, int pageSize, string? searchString, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetCustomers(page, pageSize, searchString), cancellationToken);
        return Ok(result);
    }
}