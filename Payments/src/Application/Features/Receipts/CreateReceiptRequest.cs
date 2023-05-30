using YourBrand.Payments.Application.Features.Receipts.Dtos;

namespace YourBrand.Payments.Application.Features.Receipts;

public sealed record CreateReceiptRequest(string? CustomerId, BillingDetailsDto BillingDetails, ShippingDetailsDto? ShippingDetails, IEnumerable<CreateReceiptItemDto> Items);
