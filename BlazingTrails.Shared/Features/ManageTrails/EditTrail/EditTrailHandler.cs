using MediatR;
using System.Net.Http.Json;

namespace BlazingTrails.Shared.Features.ManageTrails.EditTrail
{
    public class EditTrailHandler : IRequestHandler<EditTrailRequest, EditTrailRequest.Response>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public EditTrailHandler(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

        }
        public async Task<EditTrailRequest.Response> Handle(EditTrailRequest request, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient(HttpService.SecureAPIClient);
            var response = await client.PutAsJsonAsync(EditTrailRequest.RouteTemplate, request, cancellationToken);
            return new EditTrailRequest.Response(response.IsSuccessStatusCode);            
        }
    }
}