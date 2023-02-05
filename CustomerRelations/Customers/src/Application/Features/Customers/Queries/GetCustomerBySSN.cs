using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Customers.Application.Features.Customers.Queries;

public record GetCustomerBySSN(string SSN) : IRequest<CustomerDto?>
{
    public class Handler : IRequestHandler<GetCustomerBySSN, CustomerDto?>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerDto?> Handle(GetCustomerBySSN request, CancellationToken cancellationToken)
        {
            var person = await _context.Customers
                .Include(i => ((Person)i).Addresses)
                .Include(i => ((Organization)i).Addresses)
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => ((Person)x).Ssn == request.SSN, cancellationToken);

            return person is null
                ? null
                : person.ToDto();
        }
    }
}