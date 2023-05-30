namespace YourBrand.Payments.Application.Features.Receipts.Dtos;

using YourBrand.Payments.Application.Features.Users;

public sealed record ReceiptDto(string Id, int ReceiptNo, DateTime Date, ReceiptStatusDto Status, UserDto? AssigneeId, string? CustomerId, string Currency, BillingDetailsDto? BillingDetails, ShippingDetailsDto? ShippingDetails, IEnumerable<ReceiptItemDto> Items, decimal SubTotal, decimal Vat, decimal Total, DateTimeOffset Created, UserDto? CreatedBy, DateTimeOffset? LastModified, UserDto? LastModifiedBy);

public sealed record ReceiptItemDto(string Id, string Description, string? ItemId, string? Unit, decimal UnitPrice, double Quantity, double VatRate, decimal Total, string? Notes, DateTimeOffset Created, UserDto? CreatedBy, DateTimeOffset? LastModified, UserDto? LastModifiedBy);
