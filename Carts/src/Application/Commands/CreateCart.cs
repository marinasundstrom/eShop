using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YourBrand.Carts.Application.Carts.Dtos;

namespace YourBrand.Carts.Application.Carts.Commands;

public sealed record CreateCart(string? Tag) : IRequest<Result<CartDto>>
{
    public sealed class Validator : AbstractValidator<CreateCart>
    {
        public Validator()
        {

        }
    }

    public sealed class Handler : IRequestHandler<CreateCart, Result<CartDto>>
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

        public async Task<Result<CartDto>> Handle(CreateCart request, CancellationToken cancellationToken)
        {
            var cart = new Cart(request.Tag);

            cartRepository.Add(cart);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await domainEventDispatcher.Dispatch(new CartCreated(cart.Id), cancellationToken);

            cart = await cartRepository.GetAll()
                .Include(i => i.CreatedBy)
                .Include(i => i.LastModifiedBy)
                .FirstAsync(x => x.Id == cart.Id, cancellationToken);

            return Result.Success(cart!.ToDto());
        }
    }
}
