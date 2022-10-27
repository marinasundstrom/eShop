﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YourBrand.Sales.Domain.Entities;

namespace YourBrand.Sales.Infrastructure.Persistence;

public static class Seed
{
    public static async Task SeedData(ApplicationDbContext context)
    {
        context.Carts.Add(new Cart("test"));

        context.OrderStatuses.Add(new OrderStatus("Draft"));
        context.OrderStatuses.Add(new OrderStatus("Pending Payment"));
        context.OrderStatuses.Add(new OrderStatus("Processing"));
        context.OrderStatuses.Add(new OrderStatus("On Hold"));
        context.OrderStatuses.Add(new OrderStatus("Shipped"));
        context.OrderStatuses.Add(new OrderStatus("Completed"));
        context.OrderStatuses.Add(new OrderStatus("Canceled"));

        await context.SaveChangesAsync();
    }
}