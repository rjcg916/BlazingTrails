using Ardalis.ApiEndpoints;
using BlazingTrails.API.Persistence;
using BlazingTrails.API.Persistence.Entities;
using BlazingTrails.Shared.Features.ManageTrails.AddTrail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazingTrails.API.Features.ManageTrails.AddTrail
{
    public class AddTrailEndpoint : EndpointBaseAsync
        .WithRequest<AddTrailRequest>
        .WithActionResult<int>
    {
        BlazingTrailsContext _context;
        public AddTrailEndpoint(BlazingTrailsContext context)
        {
            _context = context;
        }


        [Authorize]
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

            await _context.Trails.AddAsync(trail, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Ok(trail.Id);
        }
    }
}