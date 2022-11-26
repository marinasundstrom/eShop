using FluentValidation;
using MediatR;
using YourBrand.Sales.Application.ProductPriceLists.Dtos;

namespace YourBrand.Sales.Application.ProductPriceLists.Queries;

public record GetProductPriceListById(string Id) : IRequest<Result<ProductPriceListDto>>
{
    public class Validator : AbstractValidator<GetProductPriceListById>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<GetProductPriceListById, Result<ProductPriceListDto>>
    {
        private readonly IProductPriceListRepository productPriceListRepository;

        public Handler(IProductPriceListRepository productPriceListRepository)
        {
            this.productPriceListRepository = productPriceListRepository;
        }

        public async Task<Result<ProductPriceListDto>> Handle(GetProductPriceListById request, CancellationToken cancellationToken)
        {
            var productPriceList = await productPriceListRepository.FindByIdAsync(request.Id, cancellationToken);

            if (productPriceList is null)
            {
                return Result.Failure<ProductPriceListDto>(Errors.ProductPriceLists.ProductPriceListNotFound);
            }

            return Result.Success(productPriceList.ToDto());
        }
    }
}
