using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YourBrand.Portal.Contracts;

namespace YourBrand.Portal.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
public sealed class Test : ControllerBase
{
    private readonly IPublishEndpoint publishEndpoint;

    public Test(IPublishEndpoint publishEndpoint)
    {
        this.publishEndpoint = publishEndpoint;
    }

    [HttpPut("{id}/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task UpdateStatus(int id, [FromBody] Domain.Enums.TodoStatus status, CancellationToken cancellationToken)
    {
        await publishEndpoint.Publish(new UpdateStatus(id, (Contracts.TodoStatus)status), cancellationToken);
    }
}
