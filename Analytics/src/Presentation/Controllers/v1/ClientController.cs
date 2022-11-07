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
public class ClientController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClientController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<string> InitClient(ClientData data, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new InitClientCommand(data.UserAgent), cancellationToken);
    }
}
