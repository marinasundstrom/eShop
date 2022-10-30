using YourBrand.Inventory.Domain;
using YourBrand.Inventory.Domain.Events;
using YourBrand.Inventory.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using YourBrand.Inventory.Application.Common;
using YourBrand.Inventory.Contracts;
using MassTransit;

namespace YourBrand.Inventory.Application.Warehouses.Items.Events;

public class WarehouseItemEventHandler 
: IDomainEventHandler<WarehouseItemCreated>, 
  IDomainEventHandler<WarehouseItemQuantityOnHandUpdated>, 
  IDomainEventHandler<WarehouseItemsPicked>,
  IDomainEventHandler<WarehouseItemsReserved>,
  IDomainEventHandler<WarehouseItemQuantityAvailableUpdated>
{
    private readonly IApplicationDbContext _context;
    private readonly IPublishEndpoint publishEndpoint;

    public WarehouseItemEventHandler(IApplicationDbContext context, IPublishEndpoint publishEndpoint)
    {
        _context = context;
        this.publishEndpoint = publishEndpoint;
    }

    public async Task Handle(WarehouseItemCreated notification, CancellationToken cancellationToken)
    {
    }

    public async Task Handle(WarehouseItemQuantityOnHandUpdated notification, CancellationToken cancellationToken)
    {
    }

    public async Task Handle(WarehouseItemsPicked notification, CancellationToken cancellationToken)
    {
    }

    public async Task Handle(WarehouseItemsReserved notification, CancellationToken cancellationToken)
    {
    }

    public async Task Handle(WarehouseItemQuantityAvailableUpdated notification, CancellationToken cancellationToken)
    {
        await publishEndpoint.Publish(new QuantityAvailableChanged(notification.ItemId, notification.WarehouseId, notification.Quantity));
    }
}