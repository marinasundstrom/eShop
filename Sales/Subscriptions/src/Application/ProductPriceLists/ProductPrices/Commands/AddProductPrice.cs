using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YourBrand.Subscriptions.Application.ProductPriceLists.Dtos;
using YourBrand.Subscriptions.Domain.ValueObjects;

namespace YourBrand.Subscriptions.Application.ProductPriceLists.ProductPrices.Commands;

public sealed record AddProductPrice(string ProductPriceListId, string? ItemId, decimal Price) : IRequest<Result<ProductPriceDto>>
{
    public sealed class Validator : AbstractValidator<AddProductPrice>
    {
        public Validator()
        {
            RuleFor(x => x. ProductPriceListId).NotEmpty().MaximumLength(60);

            RuleFor(x => x.ItemId).NotEmpty().MaximumLength(60);

            RuleFor(x => x.Price);
        }
    }

    public sealed class Handler : IRequestHandler<AddProductPrice, Result<ProductPriceDto>>
    {
        private readonly IProductPriceListRepository productPriceListRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IDomainEventDispatcher domainEventDispatcher;

        public Handler( IProductPriceListRepository productPriceListRepository, IUnitOfWork unitOfWork, IDomainEventDispatcher domainEventDispatcher)
        {
            this.productPriceListRepository = productPriceListRepository;
            this.unitOfWork = unitOfWork;
            this.domainEventDispatcher = domainEventDispatcher;
        }

        public async Task<Result<ProductPriceDto>> Handle(AddProductPrice request, CancellationToken cancellationToken)
        {
            var productPriceList = await productPriceListRepository.FindByIdAsync(request. ProductPriceListId, cancellationToken);

            if (productPriceList is null)
            {
                return Result.Failure<ProductPriceDto>(Errors. ProductPriceLists. ProductPriceListNotFound);
            }

            var productPrice = new ProductPrice(request.ProductPriceListId, request.ItemId!, new CurrencyAmount("SEK", request.Price));

            productPriceList.AddProductPrice(productPrice);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(productPrice!.ToDto());
        }
    }
}
