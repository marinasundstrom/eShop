using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YourBrand.CustomerService.Application.Tickets.Dtos;

namespace YourBrand.CustomerService.Application.Tickets.Commands;

public sealed record CreateTicket(string Title, string? Description, TicketStatusDto Status, string? AssigneeId, double? EstimatedHours, double? RemainingHours) : IRequest<Result<TicketDto>>
{
    public sealed class Validator : AbstractValidator<CreateTicket>
    {
        public Validator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(60);

            RuleFor(x => x.Description).MaximumLength(240);
        }
    }

    public sealed class Handler : IRequestHandler<CreateTicket, Result<TicketDto>>
    {
        private readonly ITicketRepository ticketRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IDomainEventDispatcher domainEventDispatcher;

        public Handler(ITicketRepository ticketRepository, IUnitOfWork unitOfWork, IDomainEventDispatcher domainEventDispatcher)
        {
            this.ticketRepository = ticketRepository;
            this.unitOfWork = unitOfWork;
            this.domainEventDispatcher = domainEventDispatcher;
        }

        public async Task<Result<TicketDto>> Handle(CreateTicket request, CancellationToken cancellationToken)
        {
            var ticket = new Ticket(request.Title, request.Description, (Domain.Enums.TicketStatus)request.Status);

            ticket.UpdateEstimatedHours(request.EstimatedHours);
            ticket.UpdateRemainingHours(request.RemainingHours);

            ticketRepository.Add(ticket);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            if (request.AssigneeId is not null)
            {
                ticket.UpdateAssigneeId(request.AssigneeId);

                await unitOfWork.SaveChangesAsync(cancellationToken);

                ticket.ClearDomainEvents();
            }

            await domainEventDispatcher.Dispatch(new TicketCreated(ticket.Id), cancellationToken);

            ticket = await ticketRepository.GetAll()
                .OrderBy(i => i.Id)
                .Include(i => i.CreatedBy)
                .Include(i => i.LastModifiedBy)
                .LastAsync(cancellationToken);

            return Result.Success(ticket!.ToDto());
        }
    }
}
