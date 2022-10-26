using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YourBrand.Sales.Application.Orders.Dtos;

namespace YourBrand.Sales.Application.Orders.Items.Commands;

public sealed record CreateOrderItem(string OrderId, string Description, string? ItemId, decimal Price, double VatRate, double Quantity) : IRequest<Result<OrderItemDto>>
{
    public sealed class Validator : AbstractValidator<CreateOrderItem>
    {
        public Validator()
        {
            RuleFor(x => x.OrderId).NotEmpty().MaximumLength(60);

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

            var orderItem = order.AddOrderItem(request.Description, request.ItemId, request.Price, request.VatRate, request.Quantity);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(orderItem!.ToDto());
        }
    }
}
