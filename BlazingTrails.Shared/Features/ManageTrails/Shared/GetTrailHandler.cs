﻿using MediatR;
using System.Net.Http.Json;

namespace BlazingTrails.Shared.Features.ManageTrails.Shared
{
    public class GetTrailHandler(IHttpClientFactory httpClientFactory) : IRequestHandler<GetTrailRequest, GetTrailRequest.Response?>
    {
        public async Task<GetTrailRequest.Response?> Handle(GetTrailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var client = httpClientFactory.CreateClient(HttpService.SecureAPIClient);

                return await client
                .GetFromJsonAsync<GetTrailRequest.Response>(
                GetTrailRequest.RouteTemplate.Replace("{trailId}", request.TrailId.ToString()));
            }
            catch (HttpRequestException)
            {
                return new GetTrailRequest.Response(null);
            }
        }
    }
}
