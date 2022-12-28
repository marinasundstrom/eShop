using MediatR;

using Microsoft.AspNetCore.Mvc;

using YourBrand.Catalog.Application;
using YourBrand.Catalog.Application.Attributes;
using YourBrand.Catalog.Application.Common.Models;
using YourBrand.Catalog.Application.Options;

namespace YourBrand.Catalog.Presentation.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
public class AttributesController : Controller
{
    private readonly IMediator _mediator;

    public AttributesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ItemsResult<AttributeDto>>> GetAttributes(
        int page = 0, int pageSize = 10, string? searchString = null, string? sortBy = null, Application.Common.Models.SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new GetAttributes(page, pageSize, searchString, sortBy, sortDirection), cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<AttributeDto> GetAttribute(string id, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetAttribute(id));
    }

    [HttpGet("{attributeId}/Values")]
    public async Task<ActionResult<OptionValueDto>> GetAttributesValues(string attributeId, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetAttributeValues(attributeId), cancellationToken));
    }
    [HttpPost]
    public async Task<AttributeDto> CreateAttribute(CreateAttributeDto dto, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new CreateAttributeCommand(dto.Name), cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task UpdateAttribute(string id, UpdateAttributeDto dto, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateAttributeCommand(id, dto.Name), cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAttribute(string id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteAttributeCommand(id), cancellationToken);
    }
}

public record CreateAttributeDto(string Name);

public record UpdateAttributeDto(string Name);