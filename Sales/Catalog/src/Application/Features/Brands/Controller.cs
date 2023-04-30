
using MediatR;

using Microsoft.AspNetCore.Mvc;

using YourBrand.Catalog.Common.Models;
using YourBrand.Catalog.Features.Brands.Commands;
using YourBrand.Catalog.Features.Brands.Queries;

namespace YourBrand.Catalog.Features.Brands;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
public class BrandsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BrandsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ItemsResult<BrandDto>> GetBrands(int page = 1, int pageSize = 10, string? searchString = null, string? sortBy = null, Catalog.Common.Models.SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new GetBrandsQuery(page - 1, pageSize, searchString, sortBy, sortDirection), cancellationToken);
    }

    [HttpGet("{id}")]
    public async Task<BrandDto?> GetBrand(int id, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetBrandQuery(id), cancellationToken);
    }

    [HttpPost]
    public async Task<BrandDto> CreateBrand(CreateBrandDto dto, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new CreateBrandCommand(dto.Name, dto.Handle), cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task UpdateBrand(int id, UpdateBrandDto dto, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateBrandCommand(id, dto.Name, dto.Handle), cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task DeleteBrand(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteBrandCommand(id), cancellationToken);
    }
}

public record CreateBrandDto(string Name, string Handle);

public record UpdateBrandDto(string Name, string Handle);

