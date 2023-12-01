using Ardalis.ApiEndpoints;
using BlazingTrails.API.Persistence;
using BlazingTrails.API.Persistence.Entities;
using BlazingTrails.Shared.Features.ManageTrails.AddTrail;
using Microsoft.AspNetCore.Mvc;

namespace BlazingTrails.API.Features.ManageTrails.AddTrail
{
    public class AddTrailEndpoint(BlazingTrailsContext database) : EndpointBaseAsync
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
                Length = request.Trail.Length
            };

            await database.Trails.AddAsync(trail, cancellationToken);

            var routeInstructions = request.Trail.Route
                                            .Select(x => new RouteInstruction
                                            {
                                                Stage = x.Stage,
                                                Description = x.Description,
                                                Trail = trail
                                            });

            await database.RouteInstructions.AddRangeAsync(routeInstructions, cancellationToken);
            await database.SaveChangesAsync(cancellationToken);

            return Ok(trail.Id);
        }
    }
}