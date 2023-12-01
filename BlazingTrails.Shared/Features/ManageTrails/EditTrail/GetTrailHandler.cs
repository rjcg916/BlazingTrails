using BlazingTrails.Shared.Features.ManageTrails.Shared;
using MediatR;
using System.Net.Http.Json;

namespace BlazingTrails.Shared.Features.ManageTrails.EditTrail
{
    public class GetTrailHandler(HttpClient httpClient) : IRequestHandler<GetTrailRequest, GetTrailRequest.Response?>
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<GetTrailRequest.Response?> Handle(GetTrailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                return await _httpClient
                .GetFromJsonAsync<GetTrailRequest.Response>(
                                                GetTrailRequest.RouteTemplate.Replace("{trailId}",
                request.TrailId.ToString()));
            }
            catch (HttpRequestException)
            {
                return default!;
            }
        }
    }
}
