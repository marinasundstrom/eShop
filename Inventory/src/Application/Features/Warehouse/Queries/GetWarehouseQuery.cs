using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Inventory.Domain;

namespace YourBrand.Inventory.Application.Features.Warehouses.Queries;

public record GetWarehouseQuery(string Id) : IRequest<WarehouseDto?>
{
    class GetWarehouseQueryHandler : IRequestHandler<GetWarehouseQuery, WarehouseDto?>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService currentUserService;

        public GetWarehouseQueryHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            this.currentUserService = currentUserService;
        }

        public async Task<WarehouseDto?> Handle(GetWarehouseQuery request, CancellationToken cancellationToken)
        {
            var warehouse = await _context.Warehouses
                .Include(x => x.Site)
               .AsNoTracking()
               .FirstAsync(c => c.Id == request.Id);

            if (warehouse is null)
            {
                return null;
            }

            return warehouse.ToDto();
        }
    }
}
