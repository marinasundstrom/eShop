using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.ResponseCompression;
using System.Net.Http;
using System.Globalization;
using Blazor.Analytics;
using Site.Client;
using Site.Server.Authentication;
using Site.Server.Authentication.Endpoints;
using YourBrand.Catalog.Client;
using YourBrand.Sales.Client;
using YourBrand.Inventory.Client;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using YourBrand.Customers.Client;
using Site.Client.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Site.Server.Authentication.Data;
using Site.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddSwaggerDocument(c =>
{
    c.Title = "Site API";
    c.Version = "0.1";
});

builder.Services.AddSignalR();

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
});

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddAuthServices(builder.Configuration);
builder.Services.AddScoped<IAuthenticationService, MockAuthenticationService>();
builder.Services.AddScoped<AuthenticationStateProvider, MockAuthenticationStateProvider>();

const string CatalogServiceUrl = $"https://localhost:5011";

builder.Services.AddCatalogClients((sp, httpClient) => {
            httpClient.BaseAddress = new Uri($"{CatalogServiceUrl}/");
        }, builder => {
            //builder.AddHttpMessageHandler<CustomAuthorizationMessageHandler>();
        });

const string SalesServiceUrl = $"https://localhost:5041";

builder.Services.AddSalesClients((sp, httpClient) => {
    httpClient.BaseAddress = new Uri($"{SalesServiceUrl}/");
}, builder => {
    //builder.AddHttpMessageHandler<CustomAuthorizationMessageHandler>();
});

builder.Services.AddHttpClient("Site", (sp, http) => {
    http.BaseAddress = new Uri("https://localhost:6001/");
});

const string InventoryServiceUrl = $"https://localhost:5051";

builder.Services.AddInventoryClients((sp, httpClient) => {
    httpClient.BaseAddress = new Uri($"{InventoryServiceUrl}/");
}, builder => {
    //builder.AddHttpMessageHandler<CustomAuthorizationMessageHandler>();
});

builder.Services.AddHttpClient("Site", (sp, http) => {
    http.BaseAddress = new Uri("https://localhost:6001/");
});

const string CustomerServiceUrl = $"https://localhost:5071";

builder.Services.AddCustomersClients((sp, httpClient) => {
    httpClient.BaseAddress = new Uri($"{CustomerServiceUrl}/");
}, builder => {
    //builder.AddHttpMessageHandler<CustomAuthorizationMessageHandler>();
});

builder.Services.AddHttpClient("Site", (sp, http) => {
    http.BaseAddress = new Uri("https://localhost:6001/");
});

builder.Services.AddServices(builder.Configuration);

var descriptorDbContext = builder.Services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(Site.Client.IItemsClient));
builder.Services.Remove(descriptorDbContext);

builder.Services.AddHttpClient("Site")
            .AddTypedClient<Site.Client.IItemsClient>((http, sp) => new Site.Client.ItemsClient(http));

builder.Services.AddGoogleAnalytics("YOUR_GTAG_ID");

CultureInfo? culture = new("sv-SE");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

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

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<Site.Server.Hubs.CartHub>("/hubs/cart");

app.MapRazorPages();
app.MapControllers();


app.MapPost("/security/createToken",
[AllowAnonymous] (User user) =>
{
    if (user.UserName == "joydip" && user.Password == "joydip123")
    {
        var issuer = builder.Configuration["Jwt:Issuer"];
        var audience = builder.Configuration["Jwt:Audience"];
        var key = Encoding.ASCII.GetBytes
        (builder.Configuration["Jwt:Key"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
             }),
            Expires = DateTime.UtcNow.AddMinutes(5),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        var stringToken = tokenHandler.WriteToken(token);
        return Results.Ok(stringToken);
    }
    return Results.Unauthorized();
});

app.AddAuthEndpoints();
app.MapFallbackToPage("/_Host");

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<UsersContext>();
//context.Database.EnsureCreated();

var renderingContext = scope.ServiceProvider.GetRequiredService<RenderingContext>();
renderingContext.IsPrerendering = true;

app.Run();

public class User
{
    public string UserName { get; set; }
    public string Password { get; set; }
}