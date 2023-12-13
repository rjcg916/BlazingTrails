using MediatR;

namespace BlazingTrails.Shared.Features.ManageTrails.Shared
{
    public class UploadTrailImageHandler : IRequestHandler<UploadTrailImageRequest, UploadTrailImageRequest.Response>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public UploadTrailImageHandler(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

        }

        public async Task<UploadTrailImageRequest.Response> Handle
                (UploadTrailImageRequest request, CancellationToken cancellationToken)
        {
            var fileContent = request.File
            .OpenReadStream(request.File.Size, cancellationToken);

            using var content = new MultipartFormDataContent
            {
                {
                    new StreamContent(fileContent),
                    "image",
                    request.File.Name
                }
            };

            var response = await _httpClientFactory.CreateClient(HttpService.SecureAPIClient)
            .PostAsync(UploadTrailImageRequest.RouteTemplate
            .Replace("{trailId}", request.TrailId.ToString()),
            content, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var fileName = await
                response.Content.ReadAsStringAsync(
                cancellationToken: cancellationToken);
                return new UploadTrailImageRequest
                .Response(fileName);
            }
            else
            {
                return new UploadTrailImageRequest
                        .Response("");
            }
        }
    }
}
