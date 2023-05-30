using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

using YourBrand.Payments.Application;

using YourBrand.Payments.Application.Common;
using YourBrand.Payments.Application.Features.Receipts.Dtos;
using YourBrand.Payments.Application.Features.Receipts.Statuses.Commands;
using YourBrand.Payments.Application.Features.Receipts.Statuses.Queries;

namespace YourBrand.ReceiptStatuses.Application.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
public class ReceiptStatusesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReceiptStatusesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ItemsResult<ReceiptStatusDto>> GetReceiptStatuses(int page = 1, int pageSize = 10, string? searchString = null, string? sortBy = null, SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new GetReceiptStatusesQuery(page - 1, pageSize, searchString, sortBy, sortDirection), cancellationToken);
    }

    [HttpGet("{id}")]
    public async Task<ReceiptStatusDto?> GetReceiptStatus(int id, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetReceiptStatusQuery(id), cancellationToken);
    }

    [HttpPost]
    public async Task<ReceiptStatusDto> CreateReceiptStatus(CreateReceiptStatusDto dto, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new CreateReceiptStatusCommand(dto.Name, dto.CreateWarehouse), cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task UpdateReceiptStatus(int id, UpdateReceiptStatusDto dto, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateReceiptStatusCommand(id, dto.Name), cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task DeleteReceiptStatus(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteReceiptStatusCommand(id), cancellationToken);
    }
}

public record CreateReceiptStatusDto(string Name, bool CreateWarehouse);

public record UpdateReceiptStatusDto(string Name);

