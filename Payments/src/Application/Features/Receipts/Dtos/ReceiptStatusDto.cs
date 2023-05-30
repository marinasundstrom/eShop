using System.ComponentModel.DataAnnotations;

using YourBrand.Payments.Application;

namespace YourBrand.Payments.Application.Features.Receipts.Dtos;

public record ReceiptStatusDto
(
    int Id,
    string Name
);
