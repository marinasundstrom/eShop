using System;
using YourBrand.Payments.Domain.Entities;

namespace YourBrand.Payments.Domain.Specifications;

public class ReceiptsWithStatus : BaseSpecification<Receipt>
{
    public ReceiptsWithStatus(ReceiptStatus status)
    {
        Criteria = order => order.Status == status;
    }
}

