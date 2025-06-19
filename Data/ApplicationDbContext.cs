using Microsoft.EntityFrameworkCore;
using VolApp.Models;
using NetTopologySuite.Geometries;

namespace VolApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Vol> Vols { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<VolLigne> VolLignes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VolLigne>(entity =>
            {
                entity.HasKey(v => v.Id);
                entity.Property(v => v.Geom).HasColumnType("geometry");
            });
        }
    }
}
