using System;
using YourBrand.Orders.Domain.Entities;

namespace YourBrand.Orders.Domain.Specifications;

public class OrdersWithStatus : BaseSpecification<Order>
{
    public OrdersWithStatus(OrderStatus status)
    {
        Criteria = order => order.Status == status;
    }
}

