using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourStore.Catalog.Features.Stores.Commands;

public sealed record DeleteStoreCommand(string Id) : IRequest
{
    public sealed class DeleteStoreCommandHandler : IRequestHandler<DeleteStoreCommand>
    {
        private readonly IApplicationDbContext context;

        public DeleteStoreCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(DeleteStoreCommand request, CancellationToken cancellationToken)
        {
            var store = await context.Stores
                .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (store is null) throw new Exception();

            context.Stores.Remove(store);
           
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}