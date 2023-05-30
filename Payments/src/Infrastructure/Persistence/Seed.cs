using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YourBrand.Payments.Domain.Entities;

namespace YourBrand.Payments.Infrastructure.Persistence;

public static class Seed
{
    public static async Task SeedData(ApplicationDbContext context)
    {
        context.ReceiptStatuses.Add(new ReceiptStatus("Draft"));
        context.ReceiptStatuses.Add(new ReceiptStatus("Pending Payment"));
        context.ReceiptStatuses.Add(new ReceiptStatus("Processing"));
        context.ReceiptStatuses.Add(new ReceiptStatus("On Hold"));
        context.ReceiptStatuses.Add(new ReceiptStatus("Shipped"));
        context.ReceiptStatuses.Add(new ReceiptStatus("Completed"));
        context.ReceiptStatuses.Add(new ReceiptStatus("Canceled"));

        await context.SaveChangesAsync();
    }
}