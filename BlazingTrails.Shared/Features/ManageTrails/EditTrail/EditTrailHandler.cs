using MediatR;
using System.Net.Http.Json;

namespace BlazingTrails.Shared.Features.ManageTrails.EditTrail
{
    public class EditTrailHandler(IHttpClientFactory httpClientFactory) : IRequestHandler<EditTrailRequest, EditTrailRequest.Response>
    {
        public async Task<EditTrailRequest.Response> Handle(EditTrailRequest request, CancellationToken cancellationToken)
        {
            var client = httpClientFactory.CreateClient(HttpService.SecureAPIClient);
            var response = await client.PutAsJsonAsync(EditTrailRequest.RouteTemplate, request, cancellationToken);
            return new EditTrailRequest.Response(response.IsSuccessStatusCode);            
        }
    }
}