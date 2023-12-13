using Ardalis.ApiEndpoints;
using BlazingTrails.API.Persistence;
using BlazingTrails.Shared.Features.ManageTrails.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazingTrails.API.Features.ManageTrails.EditTrail
{
    public class GetTrailEndpoint :
        EndpointBaseAsync.WithRequest<int>.WithActionResult<GetTrailRequest.Response>
    {
        BlazingTrailsContext _context;
        public GetTrailEndpoint(BlazingTrailsContext context)
        {
            _context = context;
        }


        [Authorize]
        [HttpGet(GetTrailRequest.RouteTemplate)]
        public override async Task<ActionResult<GetTrailRequest.Response>>
        HandleAsync(int trailId, CancellationToken cancellationToken = default)
        {
            var trail = await _context.Trails.Include(x => x.Waypoints)
            .SingleOrDefaultAsync(x => x.Id == trailId,
            cancellationToken: cancellationToken);

            if (trail is null)
            {
                return BadRequest("Trail could not be found.");
            }

            if (!trail.Owner.Equals(HttpContext.User.Identity!.Name, StringComparison.OrdinalIgnoreCase))
                return Unauthorized();

            var response = new GetTrailRequest.Response(
                new GetTrailRequest.Trail(trail.Id,
                    trail.Name,
                    trail.Location,
                    trail.Image,
                    trail.TimeInMinutes,
                    trail.Length,
                    trail.Description,
                    trail.Waypoints.Select(w =>
                    new GetTrailRequest.Waypoint(w.Latitude, w.Longitude))));

            return Ok(response);
        }
    }
}