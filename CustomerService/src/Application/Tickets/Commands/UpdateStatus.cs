using FluentValidation;
using MediatR;
using YourBrand.CustomerService.Application.Tickets.Dtos;

namespace YourBrand.CustomerService.Application.Tickets.Commands;

public sealed record UpdateStatus(int Id, TicketStatusDto Status) : IRequest<Result>
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
        private readonly ITicketRepository ticketRepository;
        private readonly IUnitOfWork unitOfWork;

        public Handler(ITicketRepository ticketRepository, IUnitOfWork unitOfWork)
        {
            this.ticketRepository = ticketRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateStatus request, CancellationToken cancellationToken)
        {
            var ticket = await ticketRepository.FindByIdAsync(request.Id, cancellationToken);

            if (ticket is null)
            {
                return Result.Failure(Errors.Tickets.TicketNotFound);
            }

            //ticket.UpdateStatus((Domain.Enums.TicketStatus)request.Status);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
