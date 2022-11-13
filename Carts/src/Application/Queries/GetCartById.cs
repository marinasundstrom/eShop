using FluentValidation;
using MediatR;
using YourBrand.Carts.Application.Carts.Dtos;

namespace YourBrand.Carts.Application.Carts.Queries;

public record GetCartById(string Id) : IRequest<Result<CartDto>>
{
    public class Validator : AbstractValidator<GetCartById>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<GetCartById, Result<CartDto>>
    {
        private readonly ICartRepository cartRepository;

        public Handler(ICartRepository cartRepository)
        {
            this.cartRepository = cartRepository;
        }

        public async Task<Result<CartDto>> Handle(GetCartById request, CancellationToken cancellationToken)
        {
            var cart = await cartRepository.FindByIdAsync(request.Id, cancellationToken);

            if (cart is null)
            {
                return Result.Failure<CartDto>(Errors.Carts.CartNotFound);
            }

            return Result.Success(cart.ToDto());
        }
    }
}
