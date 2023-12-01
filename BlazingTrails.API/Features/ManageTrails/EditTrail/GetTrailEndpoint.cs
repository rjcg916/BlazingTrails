using Ardalis.ApiEndpoints;
using BlazingTrails.API.Persistence;
using BlazingTrails.Shared.Features.ManageTrails.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazingTrails.API.Features.ManageTrails.EditTrail
{
    public class GetTrailEndpoint(BlazingTrailsContext context) : 
        EndpointBaseAsync.WithRequest<int>.WithActionResult<GetTrailRequest.Response>
    {
        [HttpGet(GetTrailRequest.RouteTemplate)]
        public override async Task<ActionResult<GetTrailRequest.Response>>
        HandleAsync(int trailId, CancellationToken cancellationToken = default)
        {
            var trail = await context.Trails
            .Include(x => x.Route)
            .SingleOrDefaultAsync(x => x.Id == trailId,
            cancellationToken: cancellationToken);

            if (trail is null)
            {
                return BadRequest("Trail could not be found.");
            }

            var response = new GetTrailRequest.Response(
            new GetTrailRequest.Trail(trail.Id,
                trail.Name,
                trail.Location,
                trail.Image,
                trail.TimeInMinutes,
                trail.Length,
                trail.Description,
                trail.Route.Select(ri => 
                new GetTrailRequest.RouteInstruction(ri.Id, ri.Stage, ri.Description))));

            return Ok(response);
        }
    }
}