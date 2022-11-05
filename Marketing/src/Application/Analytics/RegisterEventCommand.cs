using MediatR;

using Microsoft.EntityFrameworkCore;
using YourBrand.Marketing.Domain;

namespace YourBrand.Marketing.Application.Analytics.Commands;

public record RegisterEventCommand(Domain.Enums.EventType EventType, string Data) : IRequest
{
    public class Handler : IRequestHandler<RegisterEventCommand>
    {
        private readonly IApplicationDbContext context;

        public Handler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(RegisterEventCommand request, CancellationToken cancellationToken)
        {
            context.Events.Add(new Event(request.EventType, request.Data));
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}

public record class Data(string[] Labels, IEnumerable<Series> Series);

public record class Series(string Name, IEnumerable<decimal> Data);