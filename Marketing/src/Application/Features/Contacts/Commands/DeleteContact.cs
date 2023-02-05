using YourBrand.Marketing.Domain;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Marketing.Application.Features.Contacts.Commands;

public record DeleteContact(string Id) : IRequest
{
    public class Handler : IRequestHandler<DeleteContact>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(DeleteContact request, CancellationToken cancellationToken)
        {
            var contact = await _context.Contacts
                .Include(i => i.Campaign)
                .FirstAsync(x => x.Id == request.Id, cancellationToken);

            _context.Contacts.Remove(contact);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}