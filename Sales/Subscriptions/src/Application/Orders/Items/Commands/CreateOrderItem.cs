using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YourBrand.Subscriptions.Application.Orders.Dtos;

namespace YourBrand.Subscriptions.Application.Orders.Items.Commands;

public sealed record CreateOrderItem(string OrderId, string Description, string? ItemId, string? Unit, decimal UnitPrice, double VatRate, double Quantity, string? Notes) : IRequest<Result<OrderItemDto>>
{
    public sealed class Validator : AbstractValidator<CreateOrderItem>
    {
        public Validator()
        {
            RuleFor(x => x.OrderId);

            RuleFor(x => x.Description).NotEmpty().MaximumLength(240);
        }
    }

    public sealed class Handler : IRequestHandler<CreateOrderItem, Result<OrderItemDto>>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IDomainEventDispatcher domainEventDispatcher;

        public Handler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IDomainEventDispatcher domainEventDispatcher)
        {
            this.orderRepository = orderRepository;
            this.unitOfWork = unitOfWork;
            this.domainEventDispatcher = domainEventDispatcher;
        }

        public async Task<Result<OrderItemDto>> Handle(CreateOrderItem request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.FindByIdAsync(request.OrderId, cancellationToken);

            if (order is null)
            {
                return Result.Failure<OrderItemDto>(Errors.Orders.OrderNotFound);
            }

            var orderItem = order.AddOrderItem(request.Description, request.ItemId, request.Unit, request.UnitPrice, request.VatRate, request.Quantity, request.Notes);

            order.Calculate();

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(orderItem!.ToDto());
        }
    }
}
