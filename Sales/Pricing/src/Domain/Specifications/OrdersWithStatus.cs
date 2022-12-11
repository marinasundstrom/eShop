using System;
using YourBrand.Pricing.Domain.Entities;

namespace YourBrand.Pricing.Domain.Specifications;

public class OrdersWithStatus : BaseSpecification<Order>
{
    public OrdersWithStatus(OrderStatus status)
    {
        Criteria = order => order.Status == status;
    }
}

