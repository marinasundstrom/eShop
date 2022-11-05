using Microsoft.AspNetCore.Mvc;
using YourBrand.Catalog;
using YourBrand.Marketing.Client;

namespace Site.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly ILogger<AnalyticsController> _logger;
    private readonly YourBrand.Marketing.IEventsClient eventsClient;

    public AnalyticsController(
        ILogger<AnalyticsController> logger,
        YourBrand.Marketing.IEventsClient eventsClient)
    {
        _logger = logger;
        this.eventsClient = eventsClient;
    }

    [HttpPost]
    public async Task RegisterEventAsync(YourBrand.Marketing.EventData data, CancellationToken cancellationToken = default)
    {
        await eventsClient.RegisterEventAsync(data, cancellationToken);
    }
}