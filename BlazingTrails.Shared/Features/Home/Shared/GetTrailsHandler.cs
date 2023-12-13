using BlazingTrails.Shared.Features.Home.Shared;
using BlazingTrails.Shared.Features.ManageTrails;
using MediatR;
using System.Net.Http.Json;

namespace BlazingTrails.Client.Features.Home.Shared
{
    public class GetTrailsHandler : IRequestHandler<GetTrailsRequest, GetTrailsRequest.Response?>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public GetTrailsHandler(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

        }

        public async Task<GetTrailsRequest.Response?> Handle(GetTrailsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var client = _httpClientFactory.CreateClient(HttpService.UnsecuredAPIClient);

                return await client
                .GetFromJsonAsync<GetTrailsRequest.Response>(GetTrailsRequest.RouteTemplate, cancellationToken);
            }
            catch (HttpRequestException)
            {
                return default!;
            }
        }
    }
}