using MediatR;

using Microsoft.EntityFrameworkCore;
using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Attributes;

public record CreateAttributeCommand(string Name) : IRequest<AttributeDto>
{
    public class CreateAttributeCommandHandler : IRequestHandler<CreateAttributeCommand, AttributeDto>
    {
        private readonly IApplicationDbContext context;

        public CreateAttributeCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<AttributeDto> Handle(CreateAttributeCommand request, CancellationToken cancellationToken)
        {
            var attribute = await context.Attributes.FirstOrDefaultAsync(i => i.Name == request.Name, cancellationToken);

            if (attribute is not null) throw new Exception();

            attribute = new Domain.Entities.Attribute(request.Name);

            context.Attributes.Add(attribute);

            await context.SaveChangesAsync(cancellationToken);

            return attribute.ToDto();
        }
    }
}
