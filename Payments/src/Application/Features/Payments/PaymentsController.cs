using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using YourBrand.Payments.Application;
using YourBrand.Payments.Application.Common;
using System.Text.Json;

namespace YourBrand.Payments.Application.Features.Payments;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
public sealed partial class PaymentsController : ControllerBase
{
    private readonly IMediator mediator;

    public PaymentsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> CreatePayment(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreatePayment(), cancellationToken);
        return result.Handle(
            onSuccess: data => Ok(data),
            onError: error => Problem(detail: error.Detail, title: error.Title, type: error.Id));
    }

    [HttpPost("Callback")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Callback(JsonElement value, CancellationToken cancellationToken) 
    {
        try 
        {
            CallbackData json = System.Text.Json.JsonSerializer.Deserialize<CallbackData>(value.ToString())!;

            await mediator.Send(new QuickPayCallback(json!), cancellationToken);
        }
        catch(Exception e) 
        {
            Console.WriteLine(e);
        }

        return Ok();
    }
}
