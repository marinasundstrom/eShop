using Microsoft.VisualBasic;

using YourBrand.Sales.Client;

using static YourBrand.Sales.Orders.OrderItemViewModel;

namespace YourBrand.Sales.Orders;

public static class Mapper
{
    public static OrderViewModel ToModel(this OrderDto dto)
    {
        var model = new OrderViewModel
        {
            Id = dto.Id,
            Status = dto.Status,
        };

        foreach (var item in dto.Items)
        {
            model.Items.Add(item.ToModel());
        }

        return model;
    }

    public static OrderItemViewModel ToModel(this OrderItemDto dto)
    {
        return new OrderItemViewModel
        {
            Id = dto.Id,
            Description = dto.Description,
            ItemId = dto.ItemId,
            UnitPrice = dto.UnitPrice,
            Unit = dto.Unit,
            Quantity = dto.Quantity,
            VatRate = dto.VatRate
        };
    }

    public static CreateOrderItemRequest ToCreateOrderItemRequest(this OrderItemViewModel vm)
    {
        return new CreateOrderItemRequest
        {
            Description = vm.Description,
            ItemId = vm.ItemId,
            UnitPrice = vm.UnitPrice,
            Unit = vm.Unit,
            Quantity = vm.Quantity,
            VatRate = vm.VatRate
        };
    }

    public static UpdateOrderItemRequest ToUpdateOrderItemRequest(this OrderItemViewModel dto)
    {
        return new UpdateOrderItemRequest
        {
            Description = dto.Description,
            ItemId = dto.ItemId,
            UnitPrice = dto.UnitPrice,
            Unit = dto.Unit,
            Quantity = dto.Quantity,
            VatRate = dto.VatRate,
        };
    }
}
