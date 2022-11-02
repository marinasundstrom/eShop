using FluentValidation;
using MediatR;
using YourBrand.Sales.Application.Carts.Dtos;

namespace YourBrand.Sales.Application.Carts.Queries;

public record GetCartByTag(string Tag) : IRequest<Result<CartDto>>
{
    public class Validator : AbstractValidator<GetCartById>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<GetCartByTag, Result<CartDto>>
    {
        private readonly ICartRepository cartRepository;

        public Handler(ICartRepository cartRepository)
        {
            this.cartRepository = cartRepository;
        }

        public async Task<Result<CartDto>> Handle(GetCartByTag request, CancellationToken cancellationToken)
        {
            var cart = await cartRepository.FindByTagAsync(request.Tag, cancellationToken);

            if (cart is null)
            {
                return Result.Failure<CartDto>(Errors.Carts.CartNotFound);
            }

            return Result.Success(cart.ToDto());
        }
    }
}