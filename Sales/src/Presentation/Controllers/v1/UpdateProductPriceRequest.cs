namespace YourBrand.Sales.Presentation.Controllers;

public record UpdateProductPriceRequest(string Description, string? ItemId, decimal Price);