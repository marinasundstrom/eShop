using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YourBrand.Customers.Domain.Entities;
using YourBrand.Customers.Domain.Enums;

namespace YourBrand.Customers.Infrastructure.Persistence;

public static class Seed
{
    public static async Task SeedData(ApplicationDbContext context)
    {
        if (!context.Persons.Any())
        {
            var person = new Person("John", "Doe", "3234234234")
            {
                Name = "John Doe",
                Phone = null,
                PhoneMobile = "072423123",
                Email = "john.d@email.com",
            };

            person.AddAddress(new Address("foo")
            {
                Type = Domain.Enums.AddressType.Billing,
                Thoroughfare = "Baker Street",
                SubPremises = null,
                Premises = "42",
                PostalCode = "4534 23",
                Locality = "Testville",
                SubAdministrativeArea = "Sub",
                AdministrativeArea = "Area",
                Country = "Testland"
            });

            context.Persons.Add(person);

            await context.SaveChangesAsync();
        }

        if (!context.Organizations.Any())
        {
            var organization = new Organization("John", "Doe", "3234234234")
            {
                Name = "ACME Inc.",
                OrganizationNo = "2323434",
                VatNo = "SE-2323434",
                Phone = null,
                PhoneMobile = "072423123",
                Email = "acme@email.com",
            };

            organization.AddAddress(new Address("foo")
            {
                Type = Domain.Enums.AddressType.Billing,
                Thoroughfare = "Baker Street",
                SubPremises = null,
                Premises = "42",
                PostalCode = "4534 23",
                Locality = "Testville",
                SubAdministrativeArea = "Sub",
                AdministrativeArea = "Area",
                Country = "Testland"
            });

            context.Organizations.Add(organization);

            await context.SaveChangesAsync();
        }
    }
}