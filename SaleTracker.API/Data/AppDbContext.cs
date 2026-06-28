using Microsoft.EntityFrameworkCore;
using SaleTracker.API.Models;

namespace SaleTracker.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Deal> Deals { get; set; }
    }
}
