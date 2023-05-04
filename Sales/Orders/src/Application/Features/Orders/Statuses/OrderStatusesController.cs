using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

using YourBrand.Orders.Application;

using YourBrand.Orders.Application.Common;
using YourBrand.Orders.Application.Features.Orders.Dtos;
using YourBrand.Orders.Application.Features.Orders.Statuses.Commands;
using YourBrand.Orders.Application.Features.Orders.Statuses.Queries;

namespace YourBrand.OrderStatuses.Application.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
public class OrderStatusesController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderStatusesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ItemsResult<OrderStatusDto>> GetOrderStatuses(int page = 1, int pageSize = 10, string? searchString = null, string? sortBy = null, SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new GetOrderStatusesQuery(page - 1, pageSize, searchString, sortBy, sortDirection), cancellationToken);
    }

    [HttpGet("{id}")]
    public async Task<OrderStatusDto?> GetOrderStatus(int id, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetOrderStatusQuery(id), cancellationToken);
    }

    [HttpPost]
    public async Task<OrderStatusDto> CreateOrderStatus(CreateOrderStatusDto dto, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new CreateOrderStatusCommand(dto.Name, dto.Handle, dto.Description), cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task UpdateOrderStatus(int id, UpdateOrderStatusDto dto, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateOrderStatusCommand(id, dto.Name, dto.Handle, dto.Description), cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task DeleteOrderStatus(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteOrderStatusCommand(id), cancellationToken);
    }
}

public record CreateOrderStatusDto(string Name, string Handle, string? Description);

public record UpdateOrderStatusDto(string Name, string Handle, string? Description);

