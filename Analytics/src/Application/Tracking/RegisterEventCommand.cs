using MediatR;

using Microsoft.EntityFrameworkCore;
using YourBrand.Analytics.Domain;

using Microsoft.Extensions.Caching.Memory;
namespace YourBrand.Analytics.Application.Tracking.Commands;

public record RegisterEventCommand(string ClientId, string SessionId, Domain.Enums.EventType EventType, string Data) : IRequest<string?>
{
    public class Handler : IRequestHandler<RegisterEventCommand, string?>
    {
        private readonly IApplicationDbContext context;
        private readonly IMemoryCache memoryCache;

        public Handler(IApplicationDbContext context, IMemoryCache memoryCache)
        {
            this.context = context;
            this.memoryCache = memoryCache;
        }

        public async Task<string?> Handle(RegisterEventCommand request, CancellationToken cancellationToken)
        {
            var session = await memoryCache.GetOrCreate(
                $"session-{request.SessionId}",
                async cacheEntry =>
                {
                    cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(20);
                    return await context.Sessions.FirstAsync(x => x.Id == request.SessionId && x.ClientId == request.ClientId, cancellationToken);
                })!;

            //var session = await context.Sessions
            //    .FirstAsync(x => x.Id == request.SessionId && x.ClientId == request.ClientId, cancellationToken);

            if(DateTimeOffset.UtcNow > session.Expires) 
            {
                session = new Session(request.ClientId, DateTimeOffset.UtcNow);

                context.Sessions.Add(session);
                await context.SaveChangesAsync(cancellationToken);

                var cacheEntryOptions = new MemoryCacheEntryOptions();
                cacheEntryOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(20);

                //memoryCache.Remove($"session-{request.SessionId}");
                memoryCache.Set($"session-{request.SessionId}", session, cacheEntryOptions);

                return session.Id;
            }

            if((session.Expires - DateTimeOffset.UtcNow).TotalMinutes <= 10) 
            {
                session.Expires = DateTimeOffset.UtcNow.AddMinutes(30);

                await context.SaveChangesAsync(cancellationToken);

                var cacheEntryOptions = new MemoryCacheEntryOptions();
                cacheEntryOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(20);

                //memoryCache.Remove($"session-{request.SessionId}");
                memoryCache.Set($"session-{request.SessionId}", session, cacheEntryOptions);
            }

            context.Events.Add(new Event(request.ClientId, request.SessionId, request.EventType, request.Data));
            await context.SaveChangesAsync(cancellationToken);

            return null;
        }
    }
}
