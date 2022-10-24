using FluentValidation;
using MediatR;

namespace YourBrand.Sales.Application.Carts.Items.Commands;

public sealed record DeleteCartItem(string CartId, string OrdeItemId) : IRequest<Result>
{
    public sealed class Validator : AbstractValidator<DeleteCartItem>
    {
        public Validator()
        {
            RuleFor(x => x.CartId).NotEmpty();

            RuleFor(x => x.OrdeItemId).NotEmpty();
        }
    }

    public sealed class Handler : IRequestHandler<DeleteCartItem, Result>
    {
        private readonly ICartRepository cartRepository;
        private readonly IUnitOfWork unitOfWork;

        public Handler(ICartRepository cartRepository, IUnitOfWork unitOfWork)
        {
            this.cartRepository = cartRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteCartItem request, CancellationToken cancellationToken)
        {
            var cart = await cartRepository.FindByIdAsync(request.CartId, cancellationToken);

            if (cart is null)
            {
                return Result.Failure(Errors.Carts.CartNotFound);
            }

            var cartItem = cart.Items.FirstOrDefault(x => x.Id == request.OrdeItemId);

            if (cartItem is null)
            {
                throw new System.Exception();
            }

            cart.RemoveCartItem(cartItem);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
