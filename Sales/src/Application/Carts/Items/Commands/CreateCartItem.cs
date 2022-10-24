using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YourBrand.Sales.Application.Carts.Dtos;

namespace YourBrand.Sales.Application.Carts.Items.Commands;

public sealed record CreateCartItem(string CartId, string Description, string? ItemId, decimal Price, double Quantity, decimal Total) : IRequest<Result<CartItemDto>>
{
    public sealed class Validator : AbstractValidator<CreateCartItem>
    {
        public Validator()
        {
            RuleFor(x => x.CartId).NotEmpty().MaximumLength(60);

            RuleFor(x => x.Description).NotEmpty().MaximumLength(240);
        }
    }

    public sealed class Handler : IRequestHandler<CreateCartItem, Result<CartItemDto>>
    {
        private readonly ICartRepository cartRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IDomainEventDispatcher domainEventDispatcher;

        public Handler(ICartRepository cartRepository, IUnitOfWork unitOfWork, IDomainEventDispatcher domainEventDispatcher)
        {
            this.cartRepository = cartRepository;
            this.unitOfWork = unitOfWork;
            this.domainEventDispatcher = domainEventDispatcher;
        }

        public async Task<Result<CartItemDto>> Handle(CreateCartItem request, CancellationToken cancellationToken)
        {
            var cart = await cartRepository.FindByIdAsync(request.CartId, cancellationToken);

            if (cart is null)
            {
                return Result.Failure<CartItemDto>(Errors.Carts.CartNotFound);
            }

            var cartItem = cart.AddCartItem(request.Description, request.ItemId, request.Price, request.Quantity, request.Total);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(cartItem!.ToDto());
        }
    }
}
