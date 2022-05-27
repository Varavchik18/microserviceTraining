using Microsoft.EntityFrameworkCore;
using PlatformService.models;

namespace PlatformService.Data
{
    public class AppDbContext : DbContext
    {
        protected AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Platform> Platforms { get; set; }
    }
}