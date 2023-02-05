using YourBrand.Marketing.Application.Common.Models;
using YourBrand.Marketing.Domain;
using YourBrand.Marketing.Domain.Events;

using Microsoft.EntityFrameworkCore;
using YourBrand.Marketing.Application.Common.Interfaces;

using YourBrand.Marketing.Application;

using YourBrand.Marketing.Application.Common;

namespace YourBrand.Marketing.Application.Features.Addresses.Events;

public class AddressCreatedHandler : IDomainEventHandler<AddressCreated>
{
    private readonly IApplicationDbContext _context;

    public AddressCreatedHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(AddressCreated notification, CancellationToken cancellationToken)
    {
        var person = await _context.Addresses
            .FirstOrDefaultAsync(i => i.Id == notification.AddressId);

        if (person is not null)
        {

        }
    }
}