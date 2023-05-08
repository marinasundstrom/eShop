using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog;
using YourBrand.Catalog.Features.Stores;

namespace YourStore.Catalog.Features.Stores.Queries;

public sealed record GetStoreQuery(string IdOrHandle) : IRequest<StoreDto?>
{
    sealed class GetStoreQueryHandler : IRequestHandler<GetStoreQuery, StoreDto?>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService currentUserService;

        public GetStoreQueryHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            this.currentUserService = currentUserService;
        }

        public async Task<StoreDto?> Handle(GetStoreQuery request, CancellationToken cancellationToken)
        {
            var store = await _context
               .Stores
               .AsNoTracking()
               .Include(x => x.Currency)
               .FirstAsync(c => c.Id == request.IdOrHandle || c.Handle == request.IdOrHandle);

            return store?.ToDto();
        }
    }
}
