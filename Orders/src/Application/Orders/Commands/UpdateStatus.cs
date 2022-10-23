using FluentValidation;
using MediatR;
using YourBrand.Orders.Application.Orders.Dtos;

namespace YourBrand.Orders.Application.Orders.Commands;

public sealed record UpdateStatus(int Id, OrderStatusDto Status) : IRequest<Result>
{
    public sealed class Validator : AbstractValidator<UpdateStatus>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public sealed class Handler : IRequestHandler<UpdateStatus, Result>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IUnitOfWork unitOfWork;

        public Handler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            this.orderRepository = orderRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateStatus request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.FindByIdAsync(request.Id, cancellationToken);

            if (order is null)
            {
                return Result.Failure(Errors.Orders.OrderNotFound);
            }

            order.UpdateStatus((Domain.Enums.OrderStatus)request.Status);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
