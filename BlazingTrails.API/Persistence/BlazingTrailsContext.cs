using BlazingTrails.API.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazingTrails.API.Persistence
{
    public class BlazingTrailsContext : DbContext
    {
        public BlazingTrailsContext(DbContextOptions<BlazingTrailsContext> options) : base(options) { }

        public DbSet<Trail> Trails => Set<Trail>();
        public DbSet<Waypoint> Waypoints => Set<Waypoint>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new TrailConfig());
            modelBuilder.ApplyConfiguration(new WaypointConfig());
        }
    }
}