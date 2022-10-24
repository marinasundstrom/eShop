using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Site.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddHttpClient("Site", (sp, http) => {
    http.BaseAddress =  new Uri(builder.HostEnvironment.BaseAddress);
});

builder.Services.AddServices();

await builder.Build().RunAsync();
