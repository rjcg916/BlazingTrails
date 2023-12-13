using Ardalis.ApiEndpoints;
using BlazingTrails.API.Persistence;
using BlazingTrails.Shared.Features.Home.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazingTrails.API.Features.Home.Shared
{
    public class GetTrailsEndpoint : EndpointBaseAsync
            .WithRequest<int>
            .WithActionResult<GetTrailsRequest.Response>
    {

        BlazingTrailsContext _context;
        public GetTrailsEndpoint(BlazingTrailsContext context)
        {
            _context = context;
        }

        [HttpGet(GetTrailsRequest.RouteTemplate)]
        public override async Task<ActionResult<GetTrailsRequest.Response>>
            HandleAsync(int trailId, CancellationToken cancellationToken = default)
        {
            var trails = await _context.Trails
                                        .Include(x => x.Waypoints)
                                        .ToListAsync(cancellationToken);

            var response = new GetTrailsRequest
            .Response(trails.Select(trail => new GetTrailsRequest.Trail(
                trail.Id,
                trail.Name,
                trail.Image,
                trail.Location,
                trail.TimeInMinutes,
                trail.Length,
                trail.Description,                
                trail.Waypoints.Select(wp => new GetTrailsRequest.Waypoint(wp.Latitude, wp.Longitude)).ToList(),
                trail.Owner
            )));
            return Ok(response);
        }
    }
}