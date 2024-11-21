using Microsoft.EntityFrameworkCore;
using ProniaProject.Models;

namespace ProniaProject.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
     public DbSet<Slider> Slides { get; set; }


    }
}
