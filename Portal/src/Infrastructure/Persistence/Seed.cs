using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YourBrand.Portal.Domain.Entities;

namespace YourBrand.Portal.Infrastructure.Persistence;

public static class Seed
{
    public static async Task SeedData(ApplicationDbContext context)
    {
        var widgetArea = new WidgetArea("dashboard", "Dashboard");
        widgetArea.AddWidget(new Widget("analytics.engagements", null, null));
        widgetArea.AddWidget(new Widget("sample-widget2", null, null));

        context.Set<WidgetArea>().Add(widgetArea);

        await context.SaveChangesAsync();
    }
}