using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Application.Attributes;
using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Domain.Entities;

namespace YourBrand.Catalog.Application.Items.Attributes;

public record UpdateItemAttribute(string ItemId, string AttributeId, ApiUpdateItemAttribute Data) : IRequest<AttributeDto>
{
    public class Handler : IRequestHandler<UpdateItemAttribute, AttributeDto>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AttributeDto> Handle(UpdateItemAttribute request, CancellationToken cancellationToken)
        {
            var item = await _context.Items
            .AsNoTracking()
            .FirstAsync(x => x.Id == request.ItemId);

            var attribute = await _context.Attributes
                .Include(x => x.Values)
                .Include(x => x.Group)
                .FirstAsync(x => x.Id == request.AttributeId);

            var group = await _context.AttributeGroups
                .FirstOrDefaultAsync(x => x.Id == request.Data.GroupId);

            attribute.Name = request.Data.Name;
            attribute.Description = request.Data.Description;
            attribute.Group = group;
            attribute.ForVariant = request.Data.ForVariant;
            attribute.IsMainAttribute = request.Data.IsMainAttribute;

            foreach (var v in request.Data.Values)
            {
                if (v.Id == null)
                {
                    var value = new AttributeValue(Guid.NewGuid().ToString())
                    {
                        Name = v.Name
                    };

                    attribute.Values.Add(value);
                    _context.AttributeValues.Add(value);
                }
                else
                {
                    var value = attribute.Values.First(x => x.Id == v.Id);

                    value.Name = v.Name;
                }
            }

            foreach (var v in attribute.Values.ToList())
            {
                if (_context.Entry(v).State == EntityState.Added)
                    continue;

                var value = request.Data.Values.FirstOrDefault(x => x.Id == v.Id);

                if (value is null)
                {
                    attribute.Values.Remove(v);
                }
            }

            await _context.SaveChangesAsync();

            return attribute.ToDto();
        }
    }
}
