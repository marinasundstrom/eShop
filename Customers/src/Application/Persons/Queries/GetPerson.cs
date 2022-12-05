using YourBrand.Customers.Domain;

using MediatR;

using Microsoft.EntityFrameworkCore;
using YourBrand.Customers.Application.Persons;

namespace YourBrand.Customers.Application.Persons.Queries;

public record GetPerson(int Id) : IRequest<PersonDto?>
{
    public class Handler : IRequestHandler<GetPerson, PersonDto?>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PersonDto?> Handle(GetPerson request, CancellationToken cancellationToken)
        {
            var person = await _context.Persons
                .Include(i => i.Addresses)
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            return person is null
                ? null
                : person.ToDto();
        }
    }
}