
using YourBrand.Marketing.Domain;

using MediatR;
using YourBrand.Marketing.Application.Features.Contacts;

namespace YourBrand.Marketing.Application.Features.Addresses.Commands;

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
            var person = new Domain.Entities.Address(Domain.Enums.AddressType.Delivery);

            _context.Addresses.Add(person);

            await _context.SaveChangesAsync(cancellationToken);

            return person.ToDto();
        }
    }
}
