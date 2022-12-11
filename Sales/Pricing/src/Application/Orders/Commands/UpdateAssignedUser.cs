using FluentValidation;
using MediatR;
using YourBrand.Pricing.Application.Orders.Dtos;

namespace YourBrand.Pricing.Application.Orders.Commands;

public sealed record UpdateAssignedUser(string Id, string? UserId) : IRequest<Result>
{
    public sealed class Validator : AbstractValidator<UpdateAssignedUser>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public sealed class Handler : IRequestHandler<UpdateAssignedUser, Result>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;

        public Handler(IOrderRepository orderRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            this.orderRepository = orderRepository;
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateAssignedUser request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.FindByIdAsync(request.Id, cancellationToken);

            if (order is null)
            {
                return Result.Failure(Errors.Orders.OrderNotFound);
            }

            if (request.UserId is not null)
            {
                var user = await userRepository.FindByIdAsync(request.UserId, cancellationToken);

                if (user is null)
                {
                    return Result.Failure(Errors.Users.UserNotFound);
                }
            }

            order.UpdateAssigneeId(request.UserId);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
