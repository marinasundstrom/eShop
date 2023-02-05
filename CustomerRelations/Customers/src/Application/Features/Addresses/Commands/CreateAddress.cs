
using YourBrand.Customers.Domain;

using MediatR;
using YourBrand.Customers.Application.Features.Persons;

namespace YourBrand.Customers.Application.Features.Addresses.Commands;

public record CreateAddress(string FirstName, string LastName, string SSN) : IRequest<AddressDto>
{
    public class Handler : IRequestHandler<CreateAddress, AddressDto>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AddressDto> Handle(CreateAddress request, CancellationToken cancellationToken)
        {
            var person = new Domain.Entities.Address();

            _context.Addresses.Add(person);

            await _context.SaveChangesAsync(cancellationToken);

            return person.ToDto();
        }
    }
}
