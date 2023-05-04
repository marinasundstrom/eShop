using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourStore.Catalog.Features.Stores.Commands;

public sealed record UpdateStoreCommand(string Id, string Name, string Handle, string Currency) : IRequest
{
    public sealed class UpdateStoreCommandHandler : IRequestHandler<UpdateStoreCommand>
    {
        private readonly IApplicationDbContext context;

        public UpdateStoreCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(UpdateStoreCommand request, CancellationToken cancellationToken)
        {
            var store = await context.Stores.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (store is null) throw new Exception();

            store.Name = request.Name;
            store.Handle = request.Handle;
            store.Currency = request.Currency;

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
