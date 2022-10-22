using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YourBrand.Catalog.Infrastructure.Persistence.Interceptors;

namespace YourBrand.Catalog.Infrastructure.Persistence
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            const string ConnectionStringKey = "mssql";

            var connectionString = Infrastructure.ConfigurationExtensions.GetConnectionString(configuration, ConnectionStringKey, "Catalog")
                ?? configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                options.UseSqlServer(connectionString!, o => o.EnableRetryOnFailure());

                options.AddInterceptors(
                    sp.GetRequiredService<OutboxSaveChangesInterceptor>(),
                    sp.GetRequiredService<AuditableEntitySaveChangesInterceptor>());

#if DEBUG
                options
                    .LogTo(Console.WriteLine)
                    .EnableSensitiveDataLogging();
#endif
            });

            services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

            services.AddScoped<AuditableEntitySaveChangesInterceptor>();
            services.AddScoped<OutboxSaveChangesInterceptor>();

            return services;
        }
    }
}
