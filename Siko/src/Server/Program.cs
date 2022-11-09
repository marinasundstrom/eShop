using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.ResponseCompression;
using System.Net.Http;
using System.Globalization;
using Blazor.Analytics;
using Site.Client;
using System.Text;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Components.Authorization;
using Site.Server.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.Extensions.Caching.Memory;
using Site.Client.Authentication;

// Define some important constants to initialize tracing with
var serviceName = "YourBrand.Site";
var serviceVersion = "1.0.0";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerDocument(c =>
{
    c.Title = "Site API";
    c.Version = "0.1";
});

builder.Services.AddSignalR();

builder.Services.AddMemoryCache();

// Add the reverse proxy capability to the server
var proxyBuilder = builder.Services.AddReverseProxy();
// Initialize the reverse proxy from the "ReverseProxy" section of configuration
proxyBuilder.LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services
    .AddHealthChecks();

// Configure important OpenTelemetry settings, the console exporter, and instrumentation library
builder.Services.AddOpenTelemetryTracing(tracerProviderBuilder =>
{
    tracerProviderBuilder
        .AddConsoleExporter()
        .AddZipkinExporter(o =>
        {
            o.Endpoint = new Uri("http://localhost:9411/api/v2/spans");
            o.ExportProcessorType = OpenTelemetry.ExportProcessorType.Simple;
        })
        .AddSource(serviceName)
        .AddSource("MassTransit")
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService(serviceName: serviceName, serviceVersion: serviceVersion))
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation()
        .AddSqlClientInstrumentation()
        .AddMassTransitInstrumentation();
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };

    o.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            // If the request is for our hub...
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                (path.StartsWithSegments("/hubs")))
            {
                // Read the token out of the query string
                 context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddScoped<IAuthenticationService, MockAuthenticationService>();
builder.Services.AddScoped<AuthenticationStateProvider, MockAuthenticationStateProvider>();

builder.Services.AddServices(builder.Configuration);

builder.Services.AddGoogleAnalytics("YOUR_GTAG_ID");

CultureInfo? culture = new("sv-SE");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

builder.Services.AddHttpClient("Site", (sp, http) => {
    http.BaseAddress = new Uri("https://siko.yourbrand.local:5151/");
    //http.EnableIntercept(sp);
});
//.AddHttpMessageHandler<CustomMessageHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseOpenApi();
    app.UseSwaggerUi3();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for itemion scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapReverseProxy();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.MapHealthChecks("/healthz", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapFallbackToPage("/_Host");

using var scope = app.Services.CreateScope();

var renderingContext = scope.ServiceProvider.GetRequiredService<RenderingContext>();
renderingContext.IsPrerendering = true;

app.Run();