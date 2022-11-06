using MediatR;

using Microsoft.EntityFrameworkCore;
using YourBrand.Marketing.Domain;

namespace YourBrand.Marketing.Application.Analytics.Commands;

public record RegisterEventCommand(string ClientId, string SessionId, Domain.Enums.EventType EventType, string Data) : IRequest<string?>
{
    public class Handler : IRequestHandler<RegisterEventCommand, string?>
    {
        private readonly IApplicationDbContext context;

        public Handler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<string?> Handle(RegisterEventCommand request, CancellationToken cancellationToken)
        {
            var session = await context.Sessions
                .FirstAsync(x => x.Id == request.SessionId && x.ClientId == request.ClientId, cancellationToken);

            if(DateTimeOffset.UtcNow > session.Expires) 
            {
                session = new Session(request.ClientId, DateTimeOffset.UtcNow);

                context.Sessions.Add(session);
                await context.SaveChangesAsync(cancellationToken);

                return session.Id;
            }

            if((session.Expires - DateTimeOffset.UtcNow).TotalMinutes <= 10) 
            {
                session.Expires = DateTimeOffset.UtcNow.AddMinutes(30);

                await context.SaveChangesAsync(cancellationToken);
            }

            context.Events.Add(new Event(request.ClientId, request.SessionId, request.EventType, request.Data));
            await context.SaveChangesAsync(cancellationToken);

            return null;
        }
    }
}
