using MediatR;
using Microsoft.EntityFrameworkCore;
using YourBrand.Sales.Application.Common;
using YourBrand.Sales.Application.Carts.Dtos;
using YourBrand.Sales.Domain.Enums;

namespace YourBrand.Sales.Application.Carts.Queries;

public record GetCarts(int Page = 1, int PageSize = 10, string? SortBy = null, SortDirection? SortDirection = null) : IRequest<ItemsResult<CartDto>>
{
    public class Handler : IRequestHandler<GetCarts, ItemsResult<CartDto>>
    {
        private readonly ICartRepository cartRepository;

        public Handler(ICartRepository cartRepository)
        {
            this.cartRepository = cartRepository;
        }

        public async Task<ItemsResult<CartDto>> Handle(GetCarts request, CancellationToken cancellationToken)
        {
            var query = cartRepository.GetAll();

            var totalCount = await query.CountAsync(cancellationToken);

            if (request.SortBy is not null)
            {
                query = query.OrderBy(request.SortBy, request.SortDirection);
            }
            else
            {
                query = query.OrderByDescending(x => x.Created);
            }

            var carts = await query
                .Include(i => i.Items)
                .Include(i => i.CreatedBy)
                .Include(i => i.LastModifiedBy)
                .AsSplitQuery()
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize).AsQueryable()
                .ToArrayAsync(cancellationToken);

            return new ItemsResult<CartDto>(carts.Select(x => x.ToDto()), totalCount);
        }
    }
}
