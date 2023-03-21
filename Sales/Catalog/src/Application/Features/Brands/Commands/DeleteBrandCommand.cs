using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Brands.Commands;

public sealed record DeleteBrandCommand(int Id) : IRequest
{
    public sealed class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand>
    {
        private readonly IApplicationDbContext context;

        public DeleteBrandCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = await context.Brands
                .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (brand is null) throw new Exception();

            context.Brands.Remove(brand);
           
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}