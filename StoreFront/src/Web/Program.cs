using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.RateLimiting;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using YourBrand.StoreFront.Application.Services;
using YourBrand.StoreFront.Infrastructure.Persistence;
using YourBrand.StoreFront.Presentation;
using YourBrand.StoreFront.Web;
using YourBrand.StoreFront.Web.Middleware;
using YourBrand.StoreFront.Web.Services;
using Microsoft.Extensions.Azure;
using Azure.Storage.Blobs;
using Azure.Identity;
using Microsoft.Extensions.Caching.Memory;

using YourBrand.Catalog.Client;
using YourBrand.Sales.Client;
using YourBrand.Inventory.Client;
using YourBrand.Carts.Client;
using YourBrand.Customers.Client;
using YourBrand.Marketing.Client;
using YourBrand.Analytics.Client;
using System.Text;
using YourBrand.StoreFront.Authentication;
using YourBrand.StoreFront.Authentication.Endpoints;
using YourBrand.StoreFront.Authentication.Data;

Activity.DefaultIdFormat = ActivityIdFormat.W3C;
Activity.ForceDefaultIdFormat = true;

// Define some important constants to initialize tracing with
var serviceName = "YourBrand.StoreFront";
var serviceVersion = "1.0.0";

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:6001", "https://localhost:6021")
                          .AllowAnyHeader().AllowAnyMethod();
                      });
});

// Add services to the container.

builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddCatalogClients((sp, httpClient) =>
{
    httpClient.BaseAddress = configuration.GetServiceUri("catalog-web", "https");
}, builder =>
{
    //builder.AddHttpMessageHandler<CustomAuthorizationMessageHandler>();
});

builder.Services.AddSalesClients((sp, httpClient) =>
{
    httpClient.BaseAddress = configuration.GetServiceUri("sales-web", "https");
}, builder =>
{
    //builder.AddHttpMessageHandler<CustomAuthorizationMessageHandler>();
});

builder.Services.AddInventoryClients((sp, httpClient) =>
{
    httpClient.BaseAddress = configuration.GetServiceUri("inventory-web", "https");
}, builder =>
{
    //builder.AddHttpMessageHandler<CustomAuthorizationMessageHandler>();
});

builder.Services.AddCustomersClients((sp, httpClient) =>
{
    httpClient.BaseAddress = configuration.GetServiceUri("customers-web", "https");
}, builder =>
{
    //builder.AddHttpMessageHandler<CustomAuthorizationMessageHandler>();
});

builder.Services.AddMarketingClients((sp, httpClient) =>
{
    httpClient.BaseAddress = configuration.GetServiceUri("marketing-web", "https");
}, builder =>
{
    //builder.AddHttpMessageHandler<CustomAuthorizationMessageHandler>();
});

builder.Services.AddAnalyticsClients((sp, httpClient) =>
{
    httpClient.BaseAddress = configuration.GetServiceUri("analytics-web", "https");
}, builder =>
{
    //builder.AddHttpMessageHandler<CustomAuthorizationMessageHandler>();
});

builder.Services.AddCartsClients((sp, httpClient) =>
{
    httpClient.BaseAddress = configuration.GetServiceUri("carts-web", "https");
}, builder =>
{
    //builder.AddHttpMessageHandler<CustomAuthorizationMessageHandler>();
});

builder.Services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        });

builder.Services.AddVersionedApiExplorer(option =>
        {
            option.GroupNameFormat = "VVV";
            option.SubstituteApiVersionInUrl = true;
        });

// Register the Swagger services

#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
var provider = builder.Services
    .BuildServiceProvider()
    .GetRequiredService<IApiVersionDescriptionProvider>();
#pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'

foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
{
    builder.Services.AddOpenApiDocument(config =>
    {
        config.DocumentName = $"v{description.ApiVersion}";
        config.PostProcess = document =>
        {
            document.Info.Title = "StoreFront API";
            document.Info.Version = $"v{description.ApiVersion.ToString()}";
        };
        config.ApiGroupNames = new[] { description.ApiVersion.ToString() };

        config.AddSecurity("JWT", new OpenApiSecurityScheme
        {
            Type = OpenApiSecuritySchemeType.ApiKey,
            Name = "Authorization",
            In = OpenApiSecurityApiKeyLocation.Header,
            Description = "Type into the textbox: Bearer {your JWT token}."
        });

        config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
    });
}

builder.Services.AddAuthServices(builder.Configuration);

builder.Services.AddSignalR();

builder.Services.AddMemoryCache();

builder.Services
    .AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>();

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
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
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

builder.Services.AddStackExchangeRedisCache(o =>
        {
            o.Configuration = builder.Configuration.GetConnectionString("redis");
        });


builder.Services.AddAuthorization();

builder.Services.AddUniverse(builder.Configuration);

builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddAzureClients(builder =>
{
    // Add a KeyVault client
    //builder.AddSecretClient(keyVaultUrl);

    // Add a Storage account client
    builder.AddBlobServiceClient(configuration.GetConnectionString("Azure:Storage"))
                    .WithVersion(BlobClientOptions.ServiceVersion.V2019_07_07);

    // Use DefaultAzureCredential by default
    builder.UseCredential(new DefaultAzureCredential());
});

builder.Services.AddMassTransitForApp();

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
        .AddMassTransitInstrumentation()
        .AddRedisInstrumentation();
});

builder.Services.AddRateLimiter(options =>
{
    options.OnRejected = (context, cancellationToken) =>
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;

            // context.Lease.GetAllMetadata().ToList()
            //    .ForEach(m => app.Logger.LogWarning($"Rate limit exceeded: {m.Key} {m.Value}"));

            return new ValueTask();
        };

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddFixedWindowLimiter("fixed", options =>
    {
        options.PermitLimit = 5;
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 2;
        options.Window = TimeSpan.FromSeconds(2);
        options.AutoReplenishment = false;
    });
});

var app = builder.Build();

app.UseRouting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseOpenApi();
    app.UseSwaggerUi3();
}

app.UseCors(MyAllowSpecificOrigins);

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.AddAuthEndpoints();

app.MapControllers();

app.MapHealthChecks("/healthz", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});


app.MapHubsForApp();

app.UseRateLimiter();

using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    var dbProviderName = context.Database.ProviderName;

    if (dbProviderName!.Contains("SqlServer"))
    {
        //await context.Database.EnsureDeletedAsync();
        //await context.Database.EnsureCreatedAsync(); 

        try
        {
            await ApplyMigrations(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred when applying migrations to the " +
                "database. Error: {Message}", ex.Message);
        }
    }

    if (args.Contains("--seed"))
    {
        try
        {
            await Seed.SeedData(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred seeding the " +
                "database. Error: {Message}", ex.Message);
        }

        return;
    }
}

using var scope2 = app.Services.CreateScope();
var context2 = scope2.ServiceProvider.GetRequiredService<UsersContext>();
//context2.Database.EnsureDeleted();
//context2.Database.EnsureCreated();

app.Run();

static async Task ApplyMigrations(ApplicationDbContext context)
{
    var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
    if (pendingMigrations.Count() > 0)
    {
        await context.Database.MigrateAsync();
    }
}

// INFO: Makes Program class visible to IntegrationTests.
public partial class Program { }