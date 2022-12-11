using System;
using YourBrand.Subscriptions.Domain.Entities;

namespace YourBrand.Subscriptions.Domain.Specifications;

public class OrdersWithStatus : BaseSpecification<Order>
{
    public OrdersWithStatus(OrderStatus status)
    {
        Criteria = order => order.Status == status;
    }
}

