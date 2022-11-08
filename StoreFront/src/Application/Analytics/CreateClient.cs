using MediatR;
using YourBrand.Analytics;

namespace YourBrand.StoreFront.Application.Analytics;

public sealed record CreateClient : IRequest<string>
{
    sealed class Handler : IRequestHandler<CreateClient, string>
    {
        private IClientClient clientClient;
        private readonly ICurrentUserService currentUserService;

        public Handler(
            YourBrand.Analytics.IClientClient clientClient,
            ICurrentUserService currentUserService)
        {
            this.clientClient = clientClient;
            this.currentUserService = currentUserService;
        }

        public async Task<string> Handle(CreateClient request, CancellationToken cancellationToken)
        {
            var userAgent = currentUserService.UserAgent!.ToString();

            return await clientClient.InitClientAsync(new YourBrand.Analytics.ClientData()
            {
                UserAgent = userAgent!
            }, cancellationToken);
        }
    }
}
