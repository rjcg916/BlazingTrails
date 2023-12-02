using Ardalis.ApiEndpoints;
using BlazingTrails.API.Persistence;
using BlazingTrails.Shared.Features.Home.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazingTrails.API.Features.Home.Shared
{
    public class GetTrailsEndpoint(BlazingTrailsContext context) : EndpointBaseAsync
    .WithRequest<int>
    .WithActionResult<GetTrailsRequest.Response>
    {
        [HttpGet(GetTrailsRequest.RouteTemplate)]
        public override async Task<ActionResult<GetTrailsRequest.Response>>
            HandleAsync(int trailId, CancellationToken cancellationToken = default)
        {
            var trails = await context.Trails
                                        .Include(x => x.Route)
                                        .ToListAsync(cancellationToken);

            var response = new GetTrailsRequest
            .Response(trails.Select(trail => new GetTrailsRequest.Trail(
                trail.Id,
                trail.Name,
                trail.Image,
                trail.Location,
                trail.TimeInMinutes,
                trail.Length,
                trail.Description
            )));

            return Ok(response);
        }
    }
}