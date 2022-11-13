using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using YourBrand.Inventory;
using YourBrand.Sales;
using YourBrand.StoreFront.Application.Services;
using Microsoft.Extensions.Logging;
using MediatR;
using YourBrand.StoreFront.Application.Checkout;

namespace YourBrand.StoreFront.Presentation.Controllers;

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
    public YourBrand.Sales.BillingDetailsDto BillingDetails { get; set; } = null!;

    public YourBrand.Sales.ShippingDetailsDto ShippingDetails { get; set; } = null!;
}