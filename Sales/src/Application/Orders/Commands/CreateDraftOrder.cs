using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YourBrand.Sales.Application.Orders.Dtos;

namespace YourBrand.Sales.Application.Orders.Commands;

public sealed record CreateDraftOrder() : IRequest<Result<OrderDto>>
{
    public sealed class Validator : AbstractValidator<CreateDraftOrder>
    {
        public Validator()
        {
            //RuleFor(x => x.Title).NotEmpty().MaximumLength(60);

            //RuleFor(x => x.Description).MaximumLength(240);
        }
    }

    public sealed class Handler : IRequestHandler<CreateDraftOrder, Result<OrderDto>>
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

        public async Task<Result<OrderDto>> Handle(CreateDraftOrder request, CancellationToken cancellationToken)
        {
            var order = new Order();

            orderRepository.Add(order);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await domainEventDispatcher.Dispatch(new OrderCreated(order.Id), cancellationToken);

            order = await orderRepository.GetAll()
                .Include(i => i.CreatedBy)
                .Include(i => i.LastModifiedBy)
                .FirstAsync(x => x.Id == order.Id, cancellationToken);

            return Result.Success(order!.ToDto());
        }
    }
}
