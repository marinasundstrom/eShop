using FluentValidation;
using MediatR;
using YourBrand.CustomerService.Application.Tickets.Dtos;

namespace YourBrand.CustomerService.Application.Tickets.Commands;

public sealed record UpdateAssignedUser(int Id, string? UserId) : IRequest<Result>
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
        private readonly ITicketRepository ticketRepository;
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;

        public Handler(ITicketRepository ticketRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            this.ticketRepository = ticketRepository;
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateAssignedUser request, CancellationToken cancellationToken)
        {
            var ticket = await ticketRepository.FindByIdAsync(request.Id, cancellationToken);

            if (ticket is null)
            {
                return Result.Failure(Errors.Tickets.TicketNotFound);
            }

            if (request.UserId is not null)
            {
                var user = await userRepository.FindByIdAsync(request.UserId, cancellationToken);

                if (user is null)
                {
                    return Result.Failure(Errors.Users.UserNotFound);
                }
            }

            ticket.UpdateAssigneeId(request.UserId);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
