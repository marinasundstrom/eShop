using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using YourBrand.Subscriptions.Application;
using YourBrand.Subscriptions.Application.Common;
using YourBrand.Subscriptions.Application.ProductPriceLists.Commands;
using YourBrand.Subscriptions.Application.ProductPriceLists.Dtos;
using YourBrand.Subscriptions.Application.ProductPriceLists.Queries;
using YourBrand.Subscriptions.Application.ProductPriceLists.ProductPrices.Commands;

namespace YourBrand.Subscriptions.Presentation.Controllers;

partial class ProductPriceListsController
{
    [HttpPost("{id}/ProductPrices")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductPriceDto))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<ProductPriceDto>> CreateProductPrice(string id, CreateProductPriceRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new AddProductPrice(id, request.ItemId, request.Price), cancellationToken);
        return result.Handle(
            onSuccess: data => Ok(data),
            onError: error => Problem(detail: error.Detail, title: error.Title, type: error.Id));
    }

    [HttpPut("{id}/ProductPrices/{priceId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductPriceDto))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> UpdateProductPrice(string id, string priceId, UpdateProductPriceRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateProductPrice(id, priceId, request.Price), cancellationToken);
        return result.Handle(
            onSuccess: () => Ok(),
            onError: error => Problem(detail: error.Detail, title: error.Title, type: error.Id));
    }

    [HttpDelete("{id}/ProductPrices/{priceId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> RemoveProductPrice(string id, string priceId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new RemoveProductPrice(id, priceId), cancellationToken);
        return this.HandleResult(result);
    }
}
