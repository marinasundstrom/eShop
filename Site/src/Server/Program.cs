using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.ResponseCompression;
using System.Net.Http;
using System.Globalization;
using Blazor.Analytics;
using Site.Client;
using YourBrand.Catalog.Client;
using YourBrand.Sales.Client;
using YourBrand.Inventory.Client;

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

builder.Services.AddHttpContextAccessor();

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

builder.Services.AddServices();

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

app.MapHub<Site.Server.Hubs.CartHub>("/hubs/cart");

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToPage("/_Host");

app.Run();
