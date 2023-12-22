using HotelsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelsAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }
        public DbSet<Hotel> Hotels { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hotel>().HasData(
                new Hotel()
                {
                    Id = 1,
                    Name = "Royal Villa",
                    Details = "lorem ipsum  mndjkh mwswjbhbshw ydvyv hvbduyg mn gduyb uygy",
                    ImageUrl = "",
                    Occupancy = 5,
                    Rate = 200,
                    Sqft = 550,
                    Amenity = "",
                    CreatedAt = DateTime.Now,
                });
        }
    }
}
