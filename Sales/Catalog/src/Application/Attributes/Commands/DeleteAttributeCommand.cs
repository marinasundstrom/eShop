using MediatR;

using Microsoft.EntityFrameworkCore;
using YourBrand.Catalog.Application.Common.Interfaces;
using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Attributes;

public record DeleteAttributeCommand(string Id) : IRequest
{
    public class DeleteAttributeCommandHandler : IRequestHandler<DeleteAttributeCommand>
    {
        private readonly IApplicationDbContext context;

        public DeleteAttributeCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(DeleteAttributeCommand request, CancellationToken cancellationToken)
        {
            var attribute = await context.Attributes
                .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (attribute is null) throw new Exception();

            context.Attributes.Remove(attribute);

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}