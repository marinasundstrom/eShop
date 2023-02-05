using YourBrand.Customers.Domain;

using MediatR;

using Microsoft.EntityFrameworkCore;
using YourBrand.Customers.Application.Features.Customers;

namespace YourBrand.Customers.Application.Features.Customers.Queries;

public record GetCustomer(int CustomerId) : IRequest<CustomerDto?>
{
    public class Handler : IRequestHandler<GetCustomer, CustomerDto?>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerDto?> Handle(GetCustomer request, CancellationToken cancellationToken)
        {
            var person = await _context.Customers
                .Include(i => ((Person)i).Addresses)
                .Include(i => ((Organization)i).Addresses)
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.CustomerId, cancellationToken);

            return person is null
                ? null
                : person.ToDto();
        }
    }
}
