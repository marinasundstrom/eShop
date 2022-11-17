namespace YourBrand.Sales.Contracts;

public record UpdateStatus(string Id, OrderStatus Status);

public record ProductPriceChanged(string Id, decimal OldPrice, decimal NewPrice, bool IsDiscount);
