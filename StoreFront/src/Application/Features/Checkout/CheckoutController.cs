using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MediatR;

namespace YourBrand.StoreFront.Application.Features.Checkout;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
public class CheckoutController : ControllerBase
{
    private readonly ILogger<CheckoutController> _logger;
    private readonly IMediator mediator;

    public CheckoutController(
        ILogger<CheckoutController> logger,
        IMediator mediator)
    {
        _logger = logger;
        this.mediator = mediator;
    }

    [HttpPost]
    public async Task Checkout(CheckoutDto dto, CancellationToken cancellationToken = default)
    {
        await mediator.Send(new Checkout(dto.BillingDetails, dto.ShippingDetails), cancellationToken);
    }
}

public class CheckoutDto
{
    public BillingDetailsDto BillingDetails { get; set; } = null!;

    public ShippingDetailsDto ShippingDetails { get; set; } = null!;
}