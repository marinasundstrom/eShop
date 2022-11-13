using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YourBrand.Carts.Application.Carts.Dtos;

namespace YourBrand.Carts.Application.Carts.Items.Commands;

public sealed record AddCartItem(string CartId, string? ItemId, double Quantity, string? Data) : IRequest<Result<CartItemDto>>
{
    public sealed class Validator : AbstractValidator<AddCartItem>
    {
        public Validator()
        {
            RuleFor(x => x.CartId).NotEmpty().MaximumLength(60);

            RuleFor(x => x.ItemId).NotEmpty().MaximumLength(60);

            RuleFor(x => x.Quantity);
        }
    }

    public sealed class Handler : IRequestHandler<AddCartItem, Result<CartItemDto>>
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

        public async Task<Result<CartItemDto>> Handle(AddCartItem request, CancellationToken cancellationToken)
        {
            var cart = await cartRepository.FindByIdAsync(request.CartId, cancellationToken);

            if (cart is null)
            {
                return Result.Failure<CartItemDto>(Errors.Carts.CartNotFound);
            }

            var cartItem = cart.AddCartItem(request.ItemId, request.Quantity, request.Data);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(cartItem!.ToDto());
        }
    }
}
