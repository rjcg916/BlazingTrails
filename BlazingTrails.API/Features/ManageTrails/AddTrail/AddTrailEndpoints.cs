using Ardalis.ApiEndpoints;
using BlazingTrails.API.Persistence;
using BlazingTrails.API.Persistence.Entities;
using BlazingTrails.Shared.Features.ManageTrails.AddTrail;
using Microsoft.AspNetCore.Mvc;

namespace BlazingTrails.API.Features.ManageTrails.AddTrail
{
    public class AddTrailEndpoint(BlazingTrailsContext context) : EndpointBaseAsync
        .WithRequest<AddTrailRequest>
        .WithActionResult<int>
    {

        [HttpPost(AddTrailRequest.RouteTemplate)]
        public override async Task<ActionResult<int>> HandleAsync(
            AddTrailRequest request,
            CancellationToken cancellationToken = default)
        {
            var trail = new Trail
            {
                Name = request.Trail.Name,
                Description = request.Trail.Description,
                Location = request.Trail.Location,
                TimeInMinutes = request.Trail.TimeInMinutes,
                Length = request.Trail.Length,
                Waypoints = request.Trail.Waypoints.Select(
                    wp => new Waypoint
                    {
                        Latitude = wp.Latitude,
                        Longitude = wp.Longitude
                    }).ToList()
            };

            await context.Trails.AddAsync(trail, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return Ok(trail.Id);
        }
    }
}