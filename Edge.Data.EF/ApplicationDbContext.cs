using Edge.DomainModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Edge.Data.EF
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SeedRoles(modelBuilder);
        }

        public DbSet<Artwork> Artworks { get; set; }
        public DbSet<Cycle> Cycles { get; set; }

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData
            (
                new IdentityRole()
                {
                    Name = "Admin",
                    ConcurrencyStamp = "1",
                    NormalizedName = "Admin"
                },
                new IdentityRole()
                {
                    Name = "User",
                    ConcurrencyStamp = "2",
                    NormalizedName = "User"
                }
            );
        }
    }
}
