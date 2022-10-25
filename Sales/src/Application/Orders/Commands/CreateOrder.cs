using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YourBrand.Sales.Application.Orders.Dtos;

namespace YourBrand.Sales.Application.Orders.Commands;

public sealed record CreateOrder(string Title, string? Description, OrderStatusDto Status, string? AssigneeId, double? EstimatedHours, double? RemainingHours) : IRequest<Result<OrderDto>>
{
    public sealed class Validator : AbstractValidator<CreateOrder>
    {
        public Validator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(60);

            RuleFor(x => x.Description).MaximumLength(240);
        }
    }

    public sealed class Handler : IRequestHandler<CreateOrder, Result<OrderDto>>
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

        public async Task<Result<OrderDto>> Handle(CreateOrder request, CancellationToken cancellationToken)
        {
            var order = new Order((Domain.Enums.OrderStatus)request.Status);

            orderRepository.Add(order);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            if (request.AssigneeId is not null)
            {
                order.UpdateAssigneeId(request.AssigneeId);

                await unitOfWork.SaveChangesAsync(cancellationToken);

                order.ClearDomainEvents();
            }

            await domainEventDispatcher.Dispatch(new OrderCreated(order.Id), cancellationToken);

            order = await orderRepository.GetAll()
                .Include(i => i.CreatedBy)
                .Include(i => i.LastModifiedBy)
                .FirstAsync(x => x.Id == order.Id, cancellationToken);

            return Result.Success(order!.ToDto());
        }
    }
}
