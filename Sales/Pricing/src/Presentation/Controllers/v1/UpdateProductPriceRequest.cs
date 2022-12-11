namespace YourBrand.Pricing.Presentation.Controllers;

public record UpdateProductPriceRequest(string Description, string? ItemId, decimal Price);