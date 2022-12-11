using FluentValidation;
using MediatR;
using YourBrand.Subscriptions.Application.Orders.Dtos;

namespace YourBrand.Subscriptions.Application.Orders.Queries;

public record GetOrderByNo(int OrderNo) : IRequest<Result<OrderDto>>
{
    public class Validator : AbstractValidator<GetOrderById>
    {
        public Validator()
        {
            RuleFor(x => x.Id);
        }
    }

    public class Handler : IRequestHandler<GetOrderByNo, Result<OrderDto>>
    {
        private readonly IOrderRepository orderRepository;

        public Handler(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task<Result<OrderDto>> Handle(GetOrderByNo request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.FindByNoAsync(request.OrderNo, cancellationToken);

            if (order is null)
            {
                return Result.Failure<OrderDto>(Errors.Orders.OrderNotFound);
            }

            return Result.Success(order.ToDto());
        }
    }
}