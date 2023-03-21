using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Brands.Commands;

public sealed record UpdateBrandCommand(int Id, string Name) : IRequest
{
    public sealed class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand>
    {
        private readonly IApplicationDbContext context;

        public UpdateBrandCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = await context.Brands.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (brand is null) throw new Exception();

            brand.Name = request.Name;

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
