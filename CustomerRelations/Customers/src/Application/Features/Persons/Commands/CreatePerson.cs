
using YourBrand.Customers.Domain;

using MediatR;
using YourBrand.Customers.Application.Features.Persons;
using YourBrand.Customers.Application.Features.Addresses;

namespace YourBrand.Customers.Application.Features.Persons.Commands;

public record CreatePerson(string FirstName, string LastName, string SSN, string? Phone, string? PhoneMobile, string? Email, Address2Dto Address) : IRequest<PersonDto>
{
    public class Handler : IRequestHandler<CreatePerson, PersonDto>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PersonDto> Handle(CreatePerson request, CancellationToken cancellationToken)
        {
            var person = new Domain.Entities.Person(request.FirstName, request.LastName, request.SSN);
            person.Phone = request.Phone;
            person.PhoneMobile = request.PhoneMobile!;
            person.Email = request.Email!;

            person.AddAddress(new Address
            {
                Thoroughfare = request.Address.Thoroughfare,
                Premises = request.Address.Premises,
                SubPremises = request.Address.SubPremises,
                PostalCode = request.Address.PostalCode,
                Locality = request.Address.Locality,
                SubAdministrativeArea = request.Address.SubAdministrativeArea,
                AdministrativeArea = request.Address.AdministrativeArea,
                Country = request.Address.Country
            });

            _context.Persons.Add(person);

            await _context.SaveChangesAsync(cancellationToken);

            return person.ToDto();
        }
    }
}
