using Edge.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace Edge.Data.EF
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Artwork> Artworks { get; set; }
        public DbSet<Cycle> Cycles { get; set; }
    }
}
