using FluentValidation;
using MediatR;

namespace YourBrand.Pricing.Application.Orders.Commands;

public sealed record DeleteOrder(string Id) : IRequest<Result>
{
    public sealed class Validator : AbstractValidator<DeleteOrder>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public sealed class Handler : IRequestHandler<DeleteOrder, Result>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IUnitOfWork unitOfWork;

        public Handler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            this.orderRepository = orderRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteOrder request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.FindByIdAsync(request.Id, cancellationToken);

            if (order is null)
            {
                return Result.Failure(Errors.Orders.OrderNotFound);
            }

            orderRepository.Remove(order);

            order.AddDomainEvent(new OrderDeleted(order.OrderNo));

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
