using FluentValidation;
using MediatR;
using YourBrand.CustomerService;

namespace YourBrand.CustomerService.Application.Tickets.Commands;

public sealed record UpdateDescription(int Id, string? Description) : IRequest<Result>
{
    public sealed class Validator : AbstractValidator<UpdateDescription>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Description).MaximumLength(240);
        }
    }

    public sealed class Handler : IRequestHandler<UpdateDescription, Result>
    {
        private readonly ITicketRepository ticketRepository;
        private readonly IUnitOfWork unitOfWork;

        public Handler(ITicketRepository ticketRepository, IUnitOfWork unitOfWork)
        {
            this.ticketRepository = ticketRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateDescription request, CancellationToken cancellationToken)
        {
            var ticket = await ticketRepository.FindByIdAsync(request.Id, cancellationToken);

            if (ticket is null)
            {
                return Result.Failure(Errors.Tickets.TicketNotFound);
            }

            ticket.UpdateDescription(request.Description);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
