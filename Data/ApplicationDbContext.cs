using Microsoft.EntityFrameworkCore;
using VolApp.Models;

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
    }
}
