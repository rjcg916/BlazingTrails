using BlazingTrails.API.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazingTrails.API.Persistence
{
    public class BlazingTrailsContext : DbContext
    {
        public DbSet<Trail> Trails {  get; set; }

        public DbSet<RouteInstruction> RouteInstructions =>
            Set<RouteInstruction>();

        public BlazingTrailsContext(DbContextOptions<BlazingTrailsContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);          
            modelBuilder.ApplyConfiguration(new TrailConfig());
            modelBuilder.ApplyConfiguration(new RouteInstructionConfig());
        }
    }
}
