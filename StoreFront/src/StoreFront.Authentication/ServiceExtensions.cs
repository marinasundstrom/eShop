using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YourBrand.StoreFront.Authentication.Data;
using YourBrand.StoreFront.Authentication.Services;
using Microsoft.EntityFrameworkCore;

namespace YourBrand.StoreFront.Authentication;

public static class ServiceExtensions
{
    public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
    {
        const string ConnectionStringKey = "mssql";

        var connectionString = ConfigurationExtensions.GetConnectionString(configuration, ConnectionStringKey, "Users")
            ?? configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<UsersContext>((sp, options) =>
        {
            options.UseSqlServer(connectionString!, o => o.EnableRetryOnFailure());
#if DEBUG
            options
                .LogTo(Console.WriteLine)
                .EnableSensitiveDataLogging();
#endif
        });

        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<ITokenService, TokenService>();
        return services;
    }
}

