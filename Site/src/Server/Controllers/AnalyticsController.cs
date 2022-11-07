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
    private readonly YourBrand.Analytics.IClientClient clientClient;
    private readonly YourBrand.Analytics.ISessionClient sessionClient;
    private readonly YourBrand.Analytics.IEventsClient eventsClient;
    private readonly IHttpContextAccessor httpContextAccessor;

    public AnalyticsController(
        ILogger<AnalyticsController> logger,
        YourBrand.Analytics.IClientClient clientClient,
        YourBrand.Analytics.ISessionClient sessionClient,
        YourBrand.Analytics.IEventsClient eventsClient,
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
    public async Task<string> RegisterEventAsync([FromHeader(Name = "X-Client-Id")] string clientId, [FromHeader(Name = "X-Session-Id")] string sessionId, YourBrand.Analytics.EventData data, CancellationToken cancellationToken = default)
    {
        try 
        {
            return await eventsClient.RegisterEventAsync(clientId, sessionId, data, cancellationToken);
        }
        catch(YourBrand.Analytics.ApiException exc) when (exc.StatusCode == 204) 
        {

        }
        
        return null!;
    }

    [HttpPost("Client")]
    public async Task<string> CreateClient(CancellationToken cancellationToken = default)
    {
        var context = httpContextAccessor.HttpContext;

        var userAgent = context!.Request.Headers.UserAgent.ToString();

        return await clientClient.InitClientAsync(new YourBrand.Analytics.ClientData() {
            UserAgent = userAgent!
        }, cancellationToken);
    }

    [HttpPost("Session")]
    public async Task<string> StartSession([FromHeader(Name = "X-Client-Id")] string clientId, CancellationToken cancellationToken = default)
    {
        return await sessionClient.InitSessionAsync(clientId, new YourBrand.Analytics.SessionData() { IpAddress = HttpContext?.Connection?.RemoteIpAddress?.ToString() }, cancellationToken);
    }

    [HttpPost("Session/Coordinates")]
    public async Task RegisterCoordinatesAsync([FromHeader(Name = "X-Client-Id")] string clientId, [FromHeader(Name = "X-Session-Id")] string sessionId, [FromBody] YourBrand.Analytics.Coordinates coordinates, CancellationToken cancellationToken = default)
    {
        await sessionClient.RegisterCoordinatesAsync(clientId, sessionId, coordinates, cancellationToken);
    }
}