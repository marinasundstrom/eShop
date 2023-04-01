using MediatR;
using Microsoft.EntityFrameworkCore;
using YourBrand.Portal.Common;

namespace YourBrand.Portal.Features.Widgets;

public record GetWidgets(int Page = 1, int PageSize = 10, string? SortBy = null, SortDirection? SortDirection = null) : IRequest<ItemsResult<WidgetDto>>
{
    public class Handler : IRequestHandler<GetWidgets, ItemsResult<WidgetDto>>
    {
        private readonly IWidgetRepository todoRepository;

        public Handler(IWidgetRepository todoRepository)
        {
            this.todoRepository = todoRepository;
        }

        public async Task<ItemsResult<WidgetDto>> Handle(GetWidgets request, CancellationToken cancellationToken)
        {
            var query = todoRepository.GetAll();

            /*
            if (request.Status is not null)
            {
                query = query.Where(x => x.Status == (WidgetStatus)request.Status);
            }

            if (request.AssigneeId is not null)
            {
                query = query.Where(x => x.AssigneeId == request.AssigneeId);
            }
            */

            var totalCount = await query.CountAsync(cancellationToken);

            if (request.SortBy is not null)
            {
                query = query.OrderBy(request.SortBy, request.SortDirection);
            }
            /*
            else
            {
                query = query.OrderByDescending(x => x.Created);
            }*/

            var widgets = await query
                //.Include(i => i.Assignee)
                //.Include(i => i.CreatedBy)
                //.Include(i => i.LastModifiedBy)
                .AsSplitQuery()
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize).AsQueryable()
                .ToArrayAsync(cancellationToken);

            return new ItemsResult<WidgetDto>(widgets.Select(x => x.ToDto()), totalCount);
        }
    }
}
