using MediatR;
using System.Net.Http.Json;

namespace BlazingTrails.Shared.Features.ManageTrails.EditTrail
{
    public class EditTrailHandler(HttpClient httpClient) : IRequestHandler<EditTrailRequest, EditTrailRequest.Response>
    {
        public async Task<EditTrailRequest.Response> Handle(EditTrailRequest request, CancellationToken cancellationToken)
        {
            var response = await httpClient.PutAsJsonAsync(EditTrailRequest.RouteTemplate,
                                                            request, cancellationToken);

            return new EditTrailRequest.Response(response.IsSuccessStatusCode);
            
        }
    }
}