using Ardalis.ApiEndpoints;
using BlazingTrails.API.Persistence;
using BlazingTrails.API.Persistence.Entities;
using BlazingTrails.Shared.Features.ManageTrails.EditTrail;
using BlazingTrails.Shared.Features.ManageTrails.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazingTrails.API.Features.ManageTrails.EditTrail
{
    public class EditTrailEndpoint(BlazingTrailsContext database) :
                EndpointBaseAsync.WithRequest<EditTrailRequest>.WithActionResult<bool>
    {
        [HttpPut(EditTrailRequest.RouteTemplate)]
        public override async Task<ActionResult<bool>> HandleAsync(EditTrailRequest request,
                                CancellationToken cancellationToken = default)
        {
            var trail = await database.Trails
                                        .Include(x => x.Route)
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
            trail.Route = request.Trail.Route.Select(
                ri => new RouteInstruction
                {
                    Stage = ri.Stage,
                    Description = ri.Description,
                    Trail = trail
                }).ToList();
 
            if (request.Trail.ImageAction == ImageAction.Remove)
            {
                System.IO.File.Delete(Path.Combine(
                Directory.GetCurrentDirectory(), "Images",
                trail.Image!));
                trail.Image = null;
            }

            await database.SaveChangesAsync(cancellationToken);

            return Ok(true);
        }
    }
}