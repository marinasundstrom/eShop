using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YourBrand.Subscriptions.Application.ProductPriceLists.Dtos;

namespace YourBrand.Subscriptions.Application.ProductPriceLists.Commands;

public sealed record CreateProductPriceList(string Name) : IRequest<Result<ProductPriceListDto>>
{
    public sealed class Validator : AbstractValidator<CreateProductPriceList>
    {
        public Validator()
        {

        }
    }

    public sealed class Handler : IRequestHandler<CreateProductPriceList, Result<ProductPriceListDto>>
    {
        private readonly IProductPriceListRepository productPriceListRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IDomainEventDispatcher domainEventDispatcher;

        public Handler(IProductPriceListRepository productPriceListRepository, IUnitOfWork unitOfWork, IDomainEventDispatcher domainEventDispatcher)
        {
            this.productPriceListRepository = productPriceListRepository;
            this.unitOfWork = unitOfWork;
            this.domainEventDispatcher = domainEventDispatcher;
        }

        public async Task<Result<ProductPriceListDto>> Handle(CreateProductPriceList request, CancellationToken cancellationToken)
        {
            var productPriceList = new ProductPriceList(request.Name);

            productPriceListRepository.Add(productPriceList);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await domainEventDispatcher.Dispatch(new ProductPriceListCreated(productPriceList.Id), cancellationToken);

            productPriceList = await productPriceListRepository.GetAll()
                .Include(i => i.CreatedBy)
                .Include(i => i.LastModifiedBy)
                .FirstAsync(x => x.Id == productPriceList.Id, cancellationToken);

            return Result.Success(productPriceList!.ToDto());
        }
    }
}
