using FluentValidation;
using MediatR;

namespace YourBrand.Subscriptions.Application.ProductPriceLists.ProductPrices.Commands;

public sealed record UpdateProductPrice(string ProductPriceListId, string ProductPriceId, decimal Price) : IRequest<Result>
{
    public sealed class Validator : AbstractValidator<RemoveProductPrice>
    {
        public Validator()
        {
            RuleFor(x => x. ProductPriceListId).NotEmpty();

            RuleFor(x => x.ProductPriceId).NotEmpty();
        }
    }

    public sealed class Handler : IRequestHandler<UpdateProductPrice, Result>
    {
        private readonly IProductPriceListRepository productPriceListRepository;
        private readonly IUnitOfWork unitOfWork;

        public Handler( IProductPriceListRepository productPriceListRepository, IUnitOfWork unitOfWork)
        {
            this.productPriceListRepository = productPriceListRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateProductPrice request, CancellationToken cancellationToken)
        {
            var productPriceList = await productPriceListRepository.FindByIdAsync(request. ProductPriceListId, cancellationToken);

            if (productPriceList is null)
            {
                return Result.Failure(Errors.ProductPriceLists.ProductPriceListNotFound);
            }

            var productPriceListItem = productPriceList.ProductPrices.FirstOrDefault(x => x.Id == request.ProductPriceId);

            if (productPriceListItem is null)
            {
                throw new System.Exception();
            }

            //productPriceListItem.UpdateQuantity(request.Quantity);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
