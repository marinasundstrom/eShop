using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using YourBrand.Analytics.Application.Tracking;
using YourBrand.Analytics.Application.Tracking.Commands;
using YourBrand.Analytics.Domain.Enums;

namespace YourBrand.Analytics.Presentation.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
public class SessionController : ControllerBase
{
    private readonly IMediator _mediator;

    public SessionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<string> InitSession([FromHeader(Name = "X-Client-Id")] string clientId, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new InitSessionCommand(clientId), cancellationToken);
    }

    [HttpPost("Coordinates")]
    public async Task RegisterCoordinates([FromHeader(Name = "X-Client-Id")] string clientId, [FromHeader(Name = "X-Session-Id")] string sessionId, [FromBody] Domain.ValueObjects.Coordinates coordinates, CancellationToken cancellationToken)
    {
        await _mediator.Send(new RegisterGeoLocation(clientId, sessionId, coordinates), cancellationToken);
    }
}

public record EventData(EventType EventType, string Data);

public record ClientData(string UserAgent);