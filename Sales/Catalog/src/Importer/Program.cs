
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Nodes;

using YourBrand.Catalog.Domain.Entities;
using YourBrand.Catalog.Infrastructure;
using YourBrand.Catalog.Infrastructure.Persistence;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<HostApplicationLifetimeEventsHostedService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
{
    options.UseSqlServer(connectionString!, o => o.EnableRetryOnFailure());

    //options.AddInterceptors(
    //    sp.GetRequiredService<OutboxSaveChangesInterceptor>(),
    //    sp.GetRequiredService<AuditableEntitySaveChangesInterceptor>());

#if DEBUG
                options
                    .LogTo(Console.WriteLine)
                    .EnableSensitiveDataLogging();
#endif
});


var app = builder.Build();

await app.RunAsync();

public class HostApplicationLifetimeEventsHostedService : IHostedService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly ILogger<HostApplicationLifetimeEventsHostedService> _logger;
    private readonly ApplicationDbContext _context;

    public HostApplicationLifetimeEventsHostedService(
        IHostApplicationLifetime hostApplicationLifetime, ILogger<HostApplicationLifetimeEventsHostedService> logger, ApplicationDbContext context)
        => (_hostApplicationLifetime, _logger, _context) = (hostApplicationLifetime, logger, context);

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _hostApplicationLifetime.ApplicationStarted.Register(OnStarted);
        _hostApplicationLifetime.ApplicationStopping.Register(OnStopping);
        _hostApplicationLifetime.ApplicationStopped.Register(OnStopped);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;

    private async void OnStarted()
    {
        var countries = _context.Set<Country>();
        var currencies = _context.Set<Currency>();
        var languages = _context.Set<Language>();
        var regions = _context.Set<Region>();
        var continents = _context.Set<Continent>();

        foreach(var c in await countries.Take(20)
            .Include(x => x.Continent)
            .Include(x => x.Currencies)
            .Include(x => x.Languages)
            .ToArrayAsync()) {
                JsonSerializerOptions options = new()
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true
                };
                Console.WriteLine(JsonSerializer.Serialize(c, options));
        }

        return;

        countries.ExecuteDelete();
        currencies.ExecuteDelete();
        languages.ExecuteDelete();
        continents.ExecuteDelete();

        var continentsJ = JsonSerializer.Deserialize<IDictionary<string, string>>(
            await File.ReadAllTextAsync("continents.json"));

        foreach (var continentJ in continentsJ)
        {
            var continent = new Continent(continentJ.Key, continentJ.Value);
            continents.Add(continent);
        }

        var languagesJs = JsonSerializer.Deserialize<IDictionary<string, JsonElement>>(
            await File.ReadAllTextAsync("languages.json"));

        foreach (var languageJ in languagesJs)
        {
            var language = new Language(languageJ.Key, languageJ.Value.GetProperty("name").GetString()) {
                NativeName = languageJ.Value.GetProperty("native").GetString()
            };
            languages.Add(language);
        }

        await _context.SaveChangesAsync();

        var countriesJ = JsonSerializer.Deserialize<IEnumerable<CountryJ>>(
            await File.ReadAllTextAsync("countries.json"));

        foreach (var countryJ in countriesJ)
        {
            CurrencyJ currencyJ = countryJ.Currency;
            Currency? currency = await currencies.FirstOrDefaultAsync(x => x.Code == currencyJ.Code.Trim());
            if (currency is null)
            {
                currency = new Currency(currencyJ.Code.Trim(), currencyJ.Name, currencyJ.Symbol);
                currencies.Add(currency);
            }

            LanguageJ languageJ = countryJ.Language;
            Language? language = await languages.FirstOrDefaultAsync(x => x.Code == languageJ.Code.Trim());
            if (language is null)
            {
                language = new Language(languageJ.Code.Trim(), languageJ.Name);
                languages.Add(language);

            }
            
            await _context.SaveChangesAsync();
        }

        int i = 1;
        foreach (var countryJ in countriesJ)
        {
            var country = new Country(countryJ.Code, countryJ.Name)
            {
                Capital = countryJ.Capital,
            };

            country.Continent  = await continents.FirstOrDefaultAsync(x => x.Code == countryJ.Region);

            countries.Add(country);

            await _context.SaveChangesAsync();
            
            /*
            if(countryJ.Currency?.Code is not null) 
            {
                country.Currency = await currencies.FirstOrDefaultAsync(x => x.Code == countryJ.Currency.Code.Trim());
            }
            if(countryJ.Language?.Code is not null) 
            {
                country.Language = await languages.FirstOrDefaultAsync(x => x.Code == countryJ.Language.Code.Trim());
            }
            */

            await _context.SaveChangesAsync();

            i++;
        }

        var countriesJ2 = JsonSerializer.Deserialize<IEnumerable<CountryJ2>>(
            await File.ReadAllTextAsync("regions.json"));

        foreach (var countryJ2 in countriesJ2)
        {
            Country? country = await countries.FirstOrDefaultAsync(x => x.Code == countryJ2.CountryShortCode);
            if (country is not null)
            {
                foreach (var regionJ in countryJ2.Regions)
                {
                    var region = new Region(regionJ.ShortCode, regionJ.Name);
                    region.Country = country;

                    regions.Add(region);
                }

                await _context.SaveChangesAsync();
            }
        }

        var countriesJs = JsonSerializer.Deserialize<IDictionary<string, JsonElement>>(
            await File.ReadAllTextAsync("countries2.json"));

        foreach (var countryJ in countriesJs)
        {
            var code = countryJ.Key;
            var c = await countries.FirstOrDefaultAsync(x => x.Code == code);
            if(c is not null) 
            {
                c.NativeName = countryJ.Value.GetProperty("native").GetString();

                var languages0 = countryJ.Value.GetProperty("languages").EnumerateArray();

                foreach(var languageO in languages0) 
                {
                    var l = languageO.GetString();
                    c.Languages.Add(
                        await languages.FirstOrDefaultAsync(x => x.Code == l));
                }

                var currencies0 = countryJ.Value.GetProperty("currency").EnumerateArray();

                foreach(var currency0 in currencies0) 
                {
                    var cu = currency0.GetString();
                    var currency = await currencies.FirstOrDefaultAsync(x => x.Code == cu);
                    if(currency is null) continue;
                    c.Currencies.Add(
                        currency);
                }
            }
        }

        await _context.SaveChangesAsync();

        Console.WriteLine("Num: {0}", i);
    }

    private void OnStopping()
    {
        // ...
    }

    private void OnStopped()
    {
        // ...
    }
}

public partial class CountryJ
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;

    [JsonPropertyName("code")]
    public string Code { get; set; } = default!;

    [JsonPropertyName("capital")]
    public string Capital { get; set; } = default!;

    [JsonPropertyName("region")]
    public string Region { get; set; } = default!;

    [JsonPropertyName("currency")]
    public CurrencyJ Currency { get; set; } = default!;

    [JsonPropertyName("language")]
    public LanguageJ Language { get; set; } = default!;

    [JsonPropertyName("flag")]
    public Uri? Flag { get; set; }
}

public partial class CurrencyJ
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = default!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;

    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = default!;
}

public partial class LanguageJ
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = default!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;
}

public partial class CountryJ2
{
    [JsonPropertyName("countryName")]
    public string CountryName { get; set; }

    [JsonPropertyName("countryShortCode")]
    public string CountryShortCode { get; set; }

    [JsonPropertyName("regions")]
    public RegionJ[] Regions { get; set; }
}

public partial class RegionJ
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("shortCode")]
    public string ShortCode { get; set; }
}