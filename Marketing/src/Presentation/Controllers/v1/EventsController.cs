using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using YourBrand.Marketing.Application.Analytics;
using YourBrand.Marketing.Application.Analytics.Commands;
using YourBrand.Marketing.Domain.Enums;

namespace YourBrand.Marketing.Presentation.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IMediator _mediator;

    public EventsController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost]
    public async Task RegisterEvent([FromHeader(Name = "X-Client-Id")] string clientId, [FromHeader(Name = "X-Session-Id")] string sessionId, EventData dto, CancellationToken cancellationToken)
    {
        await _mediator.Send(new RegisterEventCommand(clientId, sessionId, dto.EventType, dto.Data), cancellationToken);
    }

    [HttpGet("MostViewedItems")]
    public async Task<Data> GetMostViewedItems(DateTime? From = null, DateTime? To = null, bool DistinctByClient = false, CancellationToken cancellationToken = default)
    {
       return await _mediator.Send(new GetMostViewedItems(From, To, DistinctByClient), cancellationToken);
    }
}

public record EventData(EventType EventType, string Data);

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
}

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
public class ClientController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClientController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost]
    public async Task<string> InitClient(CancellationToken cancellationToken)
    {
        return await _mediator.Send(new InitClientCommand(), cancellationToken);
    }
}