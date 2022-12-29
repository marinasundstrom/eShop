using MediatR;

using Microsoft.EntityFrameworkCore;
using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Attributes;

public record UpdateAttributeCommand(string Id, string Name, string? Description, string? GroupId, IEnumerable<ApiUpdateProductAttributeValue> Values) : IRequest
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
            var attribute = await context.Attributes
                .Include(x => x.Values)
                .Include(x => x.Group)
                .FirstAsync(x => x.Id == request.Id);

            var group = await context.AttributeGroups
                .FirstOrDefaultAsync(x => x.Id == request.GroupId);

            attribute.Name = request.Name;
            attribute.Description = request.Description;
            attribute.Group = group;

            foreach (var v in request.Values)
            {
                if (v.Id == null)
                {
                    var value = new AttributeValue(Guid.NewGuid().ToString())
                    {
                        Name = v.Name
                    };

                    attribute.Values.Add(value);
                    context.AttributeValues.Add(value);
                }
                else
                {
                    var value = attribute.Values.First(x => x.Id == v.Id);

                    value.Name = v.Name;
                }
            }

            foreach (var v in attribute.Values.ToList())
            {
                if (context.Entry(v).State == EntityState.Added)
                    continue;

                var value = request.Values.FirstOrDefault(x => x.Id == v.Id);

                if (value is null)
                {
                    attribute.Values.Remove(v);
                }
            }

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
