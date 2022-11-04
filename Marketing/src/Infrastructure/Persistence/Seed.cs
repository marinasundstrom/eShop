using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YourBrand.Marketing.Domain.Entities;
using YourBrand.Marketing.Domain.Enums;

namespace YourBrand.Marketing.Infrastructure.Persistence;

public static class Seed
{
    public static async Task SeedData(ApplicationDbContext context)
    {
        if (!context.Contacts.Any())
        {
            var person = new Contact("John", "Doe", "3234234234")
            {
                Phone = null,
                PhoneMobile = "072423123",
                Email = "test@test.com",
                Address = new Domain.ValueObjects.Address(
                    Thoroughfare: "Baker Street",
                    SubPremises: null,
                    Premises: "42",
                    PostalCode: "4534 23",
                    Locality: "Testville",
                    SubAdministrativeArea: "Sub",
                    AdministrativeArea: "Area",
                    Country: "Testland"
                )
            };

            context.Contacts.Add(person);

            await context.SaveChangesAsync();
        }
    }
}