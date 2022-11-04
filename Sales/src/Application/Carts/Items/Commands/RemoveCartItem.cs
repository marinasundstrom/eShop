using FluentValidation;
using MediatR;

namespace YourBrand.Sales.Application.Carts.Items.Commands;

public sealed record RemoveCartItem(string CartId, string CartItemId) : IRequest<Result>
{
    public sealed class Validator : AbstractValidator<RemoveCartItem>
    {
        public Validator()
        {
            RuleFor(x => x.CartId).NotEmpty();

            RuleFor(x => x.CartItemId).NotEmpty();
        }
    }

    public sealed class Handler : IRequestHandler<RemoveCartItem, Result>
    {
        private readonly ICartRepository cartRepository;
        private readonly IUnitOfWork unitOfWork;

        public Handler(ICartRepository cartRepository, IUnitOfWork unitOfWork)
        {
            this.cartRepository = cartRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(RemoveCartItem request, CancellationToken cancellationToken)
        {
            var cart = await cartRepository.FindByIdAsync(request.CartId, cancellationToken);

            if (cart is null)
            {
                return Result.Failure(Errors.Carts.CartNotFound);
            }

            var cartItem = cart.Items.FirstOrDefault(x => x.Id == request.CartItemId);

            if (cartItem is null)
            {
                throw new System.Exception();
            }
            
            await cartRepository.DeleteCartItem(request.CartId, request.CartItemId);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
