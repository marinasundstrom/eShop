using FluentValidation;
using MediatR;

namespace YourBrand.Carts.Application.Features.Carts.Commands;

public sealed record ClearCart(string Id) : IRequest<Result>
{
    public sealed class Validator : AbstractValidator<ClearCart>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public sealed class Handler : IRequestHandler<ClearCart, Result>
    {
        private readonly ICartRepository cartRepository;
        private readonly IUnitOfWork unitOfWork;

        public Handler(ICartRepository cartRepository, IUnitOfWork unitOfWork)
        {
            this.cartRepository = cartRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(ClearCart request, CancellationToken cancellationToken)
        {
            var cart = await cartRepository.FindByIdAsync(request.Id, cancellationToken);

            if (cart is null)
            {
                return Result.Failure(Errors.Carts.CartNotFound);
            }

            foreach (var cartItem in cart.Items)
            {
                await cartRepository.DeleteCartItem(request.Id, cartItem.Id);
            }

            //cart.AddDomainEvent(new CartCleared(cart.Id));

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
