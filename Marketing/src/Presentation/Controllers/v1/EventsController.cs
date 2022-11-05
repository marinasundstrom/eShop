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
    public async Task RegisterEvent(EventData dto, CancellationToken cancellationToken)
    {
        await _mediator.Send(new RegisterEventCommand(dto.EventType, dto.Data), cancellationToken);
    }

    [HttpGet("MostViewedItems")]
    public async Task<Data> GetMostViewedItems(DateTime? From = null, DateTime? To = null, bool DistinctByClient = false, CancellationToken cancellationToken = default)
    {
       return await _mediator.Send(new GetMostViewedItems(From, To, DistinctByClient), cancellationToken);
    }
}

public record EventData(EventType EventType, string Data);