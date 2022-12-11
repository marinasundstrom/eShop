using FluentValidation;
using MediatR;

namespace YourBrand.Subscriptions.Application.ProductPriceLists.Commands;

public sealed record DeleteProductPriceList(string Id) : IRequest<Result>
{
    public sealed class Validator : AbstractValidator<DeleteProductPriceList>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public sealed class Handler : IRequestHandler<DeleteProductPriceList, Result>
    {
        private readonly IProductPriceListRepository productPriceListRepository;
        private readonly IUnitOfWork unitOfWork;

        public Handler(IProductPriceListRepository productPriceListRepository, IUnitOfWork unitOfWork)
        {
            this.productPriceListRepository = productPriceListRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteProductPriceList request, CancellationToken cancellationToken)
        {
            var productPriceList = await productPriceListRepository.FindByIdAsync(request.Id, cancellationToken);

            if (productPriceList is null)
            {
                return Result.Failure(Errors.ProductPriceLists.ProductPriceListNotFound);
            }

            productPriceListRepository.Remove(productPriceList);

            //productPriceList.AddDomainEvent(new ProductPriceListDeleted(productPriceList.Id));

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
