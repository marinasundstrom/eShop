using MediatR;

using Microsoft.EntityFrameworkCore;
using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Attributes;

public record UpdateAttributeCommand(string Id, string Name) : IRequest
{
    public class UpdateAttributeCommandHandler : IRequestHandler<UpdateAttributeCommand>
    {
        private readonly IApplicationDbContext context;

        public UpdateAttributeCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(UpdateAttributeCommand request, CancellationToken cancellationToken)
        {
            var attribute = await context.Attributes.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (attribute is null) throw new Exception();

            attribute.Name = request.Name;

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
