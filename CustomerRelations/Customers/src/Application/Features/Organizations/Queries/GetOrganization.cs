using YourBrand.Customers.Domain;

using MediatR;

using Microsoft.EntityFrameworkCore;
using YourBrand.Customers.Application.Features.Organizations;

namespace YourBrand.Customers.Application.Features.Organizations.Queries;

public record GetOrganization(int Id) : IRequest<OrganizationDto?>
{
    public class Handler : IRequestHandler<GetOrganization, OrganizationDto?>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OrganizationDto?> Handle(GetOrganization request, CancellationToken cancellationToken)
        {

            var organization = await _context.Organizations
                .Include(i => i.Addresses)
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            return organization is null
                ? null
                : organization.ToDto();
        }
    }
}