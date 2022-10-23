using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.ResponseCompression;
using System.Net.Http;
using System.Globalization;
using Blazor.Analytics;
using YourBrand.Catalog.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddSwaggerDocument(c =>
{
    c.Title = "Site API";
    c.Version = "0.1";
});

const string CatalogServiceUrl = $"https://localhost:5011";

builder.Services.AddCatalogClients((sp, httpClient) => {
            httpClient.BaseAddress = new Uri($"{CatalogServiceUrl}/");
        }, builder => {
            //builder.AddHttpMessageHandler<CustomAuthorizationMessageHandler>();
        });

var descriptorDbContext = builder.Services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(Site.Client.IProductsClient));
builder.Services.Remove(descriptorDbContext);

builder.Services.AddHttpClient(nameof(Site.Client.ProductsClient) + "C", (sp, http) => {
                http.BaseAddress = new Uri("https://localhost:6001/");
            })
            .AddTypedClient<Site.Client.IProductsClient>((http, sp) => new Site.Client.ProductsClient(http));

CultureInfo? culture = new("sv-SE");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

builder.Services.AddGoogleAnalytics("YOUR_GTAG_ID");

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
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToPage("/_Host");

app.Run();