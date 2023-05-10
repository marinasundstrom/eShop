using MediatR;

using Microsoft.AspNetCore.Mvc;

using YourBrand.Catalog.Common.Models;

using YourStore.Catalog.Features.Currencies;

namespace YourBrand.Catalog.Features.Currencies;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
public class CurrenciesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CurrenciesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ItemsResult<CurrencyDto>> GetCurrencies(int page = 1, int pageSize = 10, string? searchString = null, string? sortBy = null, YourBrand.Catalog.Common.Models.SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new GetCurrenciesQuery(page, pageSize, searchString, sortBy, sortDirection), cancellationToken);
    }
}

