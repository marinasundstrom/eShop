using MediatR;
using YourBrand.Analytics;

namespace YourBrand.StoreFront.Application.Analytics;

public sealed record RegisterEvent(EventType EventType, string Data) : IRequest<string>
{
    sealed class Handler : IRequestHandler<RegisterEvent, string>
    {
        private IEventsClient eventsClient;
        private readonly ICurrentUserService currentUserService;

        public Handler(
            YourBrand.Analytics.IEventsClient eventsClient,
            ICurrentUserService currentUserService)
        {
            this.eventsClient = eventsClient;
            this.currentUserService = currentUserService;
        }

        public async Task<string> Handle(RegisterEvent request, CancellationToken cancellationToken)
        {
            try
            {
                return await eventsClient.RegisterEventAsync(currentUserService.ClientId, currentUserService.SessionId,
                    new EventData { EventType = request.EventType, Data = request.Data }, cancellationToken);
            }
            catch (YourBrand.Analytics.ApiException exc) when (exc.StatusCode == 204)
            {

            }

            return null!;
        }
    }
}
