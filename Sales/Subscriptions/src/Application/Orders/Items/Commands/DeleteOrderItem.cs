using FluentValidation;
using MediatR;

namespace YourBrand.Subscriptions.Application.Orders.Items.Commands;

public sealed record DeleteOrderItem(string OrderId, string OrdeItemId) : IRequest<Result>
{
    public sealed class Validator : AbstractValidator<DeleteOrderItem>
    {
        public Validator()
        {
            RuleFor(x => x.OrderId).NotEmpty();

            RuleFor(x => x.OrdeItemId).NotEmpty();
        }
    }

    public sealed class Handler : IRequestHandler<DeleteOrderItem, Result>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IUnitOfWork unitOfWork;

        public Handler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            this.orderRepository = orderRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteOrderItem request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.FindByIdAsync(request.OrderId, cancellationToken);

            if (order is null)
            {
                return Result.Failure(Errors.Orders.OrderNotFound);
            }

            var orderItem = order.Items.FirstOrDefault(x => x.Id == request.OrdeItemId);

            if (orderItem is null)
            {
                throw new System.Exception();
            }

            order.RemoveOrderItem(orderItem);

            order.Calculate();

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
