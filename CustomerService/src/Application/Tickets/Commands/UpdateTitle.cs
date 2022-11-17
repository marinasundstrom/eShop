using FluentValidation;
using MediatR;

namespace YourBrand.CustomerService.Application.Tickets.Commands;

public sealed record UpdateTitle(int Id, string Title) : IRequest<Result>
{
    public sealed class Validator : AbstractValidator<UpdateTitle>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Title).NotEmpty().MaximumLength(60);
        }
    }

    public sealed class Handler : IRequestHandler<UpdateTitle, Result>
    {
        private readonly ITicketRepository ticketRepository;
        private readonly IUnitOfWork unitOfWork;

        public Handler(ITicketRepository ticketRepository, IUnitOfWork unitOfWork)
        {
            this.ticketRepository = ticketRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateTitle request, CancellationToken cancellationToken)
        {
            var ticket = await ticketRepository.FindByIdAsync(request.Id, cancellationToken);

            if (ticket is null)
            {
                return Result.Failure(Errors.Tickets.TicketNotFound);
            }

            ticket.UpdateTitle(request.Title);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
