using Microsoft.AspNetCore.Mvc;
using YourBrand.Catalog;
using YourBrand.Marketing;
using YourBrand.Marketing.Client;

namespace Site.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly ILogger<AnalyticsController> _logger;
    private readonly IClientClient clientClient;
    private readonly ISessionClient sessionClient;
    private readonly YourBrand.Marketing.IEventsClient eventsClient;
    private readonly IHttpContextAccessor httpContextAccessor;

    public AnalyticsController(
        ILogger<AnalyticsController> logger,
        YourBrand.Marketing.IClientClient clientClient,
        YourBrand.Marketing.ISessionClient sessionClient,
        YourBrand.Marketing.IEventsClient eventsClient,
        IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        this.clientClient = clientClient;
        this.sessionClient = sessionClient;
        this.eventsClient = eventsClient;
        this.httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    [HttpPost("Event")]
    public async Task<string> RegisterEventAsync([FromHeader(Name = "X-Client-Id")] string clientId, [FromHeader(Name = "X-Session-Id")] string sessionId, YourBrand.Marketing.EventData data, CancellationToken cancellationToken = default)
    {
        try 
        {
            return await eventsClient.RegisterEventAsync(clientId, sessionId, data, cancellationToken);
        }
        catch(YourBrand.Marketing.ApiException exc) when (exc.StatusCode == 204) 
        {
            // This is OK!
        }

        return null!;
    }

    [HttpPost("Client")]
    public async Task<string> CreateClient(CancellationToken cancellationToken = default)
    {
        var context = httpContextAccessor.HttpContext;

        var userAgent = context!.Request.Headers.UserAgent.ToString();

        return await clientClient.InitClientAsync(new ClientData() {
            UserAgent = userAgent!
        }, cancellationToken);
    }

    [HttpPost("Session")]
    public async Task<string> StartSession([FromHeader(Name = "X-Client-Id")] string clientId, CancellationToken cancellationToken = default)
    {
        return await sessionClient.InitSessionAsync(clientId, cancellationToken);
    }

    [HttpPost("Session/Coordinates")]
    public async Task RegisterCoordinatesAsync([FromHeader(Name = "X-Client-Id")] string clientId, [FromHeader(Name = "X-Session-Id")] string sessionId, [FromBody] Coordinates coordinates, CancellationToken cancellationToken = default)
    {
        await sessionClient.RegisterCoordinatesAsync(clientId, sessionId, coordinates, cancellationToken);
    }
}