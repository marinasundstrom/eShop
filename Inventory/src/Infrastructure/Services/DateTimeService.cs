namespace YourBrand.Inventory.Infrastructure.Services;

sealed class DateTimeService : IDateTime
{
    public DateTimeOffset Now => DateTimeOffset.UtcNow;
}

