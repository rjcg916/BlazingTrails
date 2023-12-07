using Ardalis.ApiEndpoints;
using BlazingTrails.API.Persistence;
using BlazingTrails.API.Persistence.Entities;
using BlazingTrails.Shared.Features.ManageTrails.EditTrail;
using BlazingTrails.Shared.Features.ManageTrails.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazingTrails.API.Features.ManageTrails.EditTrail
{
    public class EditTrailEndpoint(BlazingTrailsContext context) :
                EndpointBaseAsync.WithRequest<EditTrailRequest>.WithActionResult<bool>
    {
        [HttpPut(EditTrailRequest.RouteTemplate)]
        public override async Task<ActionResult<bool>> HandleAsync(EditTrailRequest request,
                                CancellationToken cancellationToken = default)
        {
            var trail = await context.Trails
                                        .Include(x => x.Waypoints)
                                        .SingleOrDefaultAsync(x => x.Id == request.Trail.Id,
                                        cancellationToken: cancellationToken);
            if (trail is null)
            {
                return BadRequest("Trail could not be found.");
            }

            trail.Name = request.Trail.Name;
            trail.Description = request.Trail.Description;
            trail.Location = request.Trail.Location;
            trail.TimeInMinutes = request.Trail.TimeInMinutes;
            trail.Length = request.Trail.Length;
            trail.Waypoints = request.Trail.Waypoints.Select(
                                wp => new Waypoint
                                       {
                                        Latitude = wp.Latitude,
                                        Longitude = wp.Longitude
                                        }).ToList();

            if (request.Trail.ImageAction == ImageAction.Remove)
            {
                System.IO.File.Delete(Path.Combine(
                Directory.GetCurrentDirectory(), "Images",
                trail.Image!));
                trail.Image = null;
            }

            await context.SaveChangesAsync(cancellationToken);

            return Ok(true);
        }
    }
}
