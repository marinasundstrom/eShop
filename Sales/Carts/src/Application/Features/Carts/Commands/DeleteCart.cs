using FluentValidation;
using MediatR;

namespace YourBrand.Carts.Application.Features.Carts.Commands;

public sealed record DeleteCart(string Id) : IRequest<Result>
{
    public sealed class Validator : AbstractValidator<DeleteCart>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public sealed class Handler : IRequestHandler<DeleteCart, Result>
    {
        private readonly ICartRepository cartRepository;
        private readonly IUnitOfWork unitOfWork;

        public Handler(ICartRepository cartRepository, IUnitOfWork unitOfWork)
        {
            this.cartRepository = cartRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteCart request, CancellationToken cancellationToken)
        {
            var cart = await cartRepository.FindByIdAsync(request.Id, cancellationToken);

            if (cart is null)
            {
                return Result.Failure(Errors.Carts.CartNotFound);
            }

            cartRepository.Remove(cart);

            cart.AddDomainEvent(new CartDeleted(cart.Id));

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
