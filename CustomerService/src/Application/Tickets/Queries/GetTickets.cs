using MediatR;
using Microsoft.EntityFrameworkCore;
using YourBrand.CustomerService.Application.Common;
using YourBrand.CustomerService.Application.Tickets.Dtos;
using YourBrand.CustomerService.Domain.Enums;

namespace YourBrand.CustomerService.Application.Tickets.Queries;

public record GetTickets(TicketStatusDto? Status, string? AssigneeId, int Page = 1, int PageSize = 10, string? SortBy = null, SortDirection? SortDirection = null) : IRequest<ItemsResult<TicketDto>>
{
    public class Handler : IRequestHandler<GetTickets, ItemsResult<TicketDto>>
    {
        private readonly ITicketRepository ticketRepository;

        public Handler(ITicketRepository ticketRepository)
        {
            this.ticketRepository = ticketRepository;
        }

        public async Task<ItemsResult<TicketDto>> Handle(GetTickets request, CancellationToken cancellationToken)
        {
            var query = ticketRepository.GetAll();

            if (request.Status is not null)
            {
                query = query.Where(x => x.Status == (TicketStatus)request.Status);
            }

            if (request.AssigneeId is not null)
            {
                query = query.Where(x => x.AssigneeId == request.AssigneeId);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            if (request.SortBy is not null)
            {
                query = query.OrderBy(request.SortBy, request.SortDirection);
            }
            else
            {
                query = query.OrderByDescending(x => x.Created);
            }

            var tickets = await query
                .Include(i => i.Assignee)
                .Include(i => i.CreatedBy)
                .Include(i => i.LastModifiedBy)
                .AsSplitQuery()
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize).AsQueryable()
                .ToArrayAsync(cancellationToken);

            return new ItemsResult<TicketDto>(tickets.Select(x => x.ToDto()), totalCount);
        }
    }
}
