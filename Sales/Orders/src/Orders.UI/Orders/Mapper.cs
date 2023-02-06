using Microsoft.VisualBasic;

using YourBrand.Orders.Client;

using static YourBrand.Orders.Orders.OrderItemViewModel;

namespace YourBrand.Orders.Orders;

public static class Mapper
{
    public static OrderViewModel ToModel(this OrderDto dto)
    {
        var model = new OrderViewModel
        {
            Id = dto.OrderNo,
            Date = dto.Date.Date,
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
            Notes = dto.Notes,
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
            Notes = vm.Notes,
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
            Notes = dto.Notes,
            UnitPrice = dto.UnitPrice,
            Unit = dto.Unit,
            Quantity = dto.Quantity,
            VatRate = dto.VatRate,
        };
    }
}
