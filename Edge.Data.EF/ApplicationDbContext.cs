using Edge.DomainModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Edge.Data.EF
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cycle>()
                .HasMany(c => c.Artworks)
                .WithOne(a => a.Cycle)
                .HasForeignKey(a => a.CycleId)
                .OnDelete(DeleteBehavior.SetNull);

            // Order configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Status).HasMaxLength(20);
                entity.Property(e => e.PaymentIntentId).HasMaxLength(50);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.PaymentIntentId).IsUnique();

                entity.HasOne(o => o.User)
                    .WithMany()
                    .HasForeignKey(o => o.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.OrderItems)
                    .WithOne(e => e.Order)
                    .HasForeignKey(e => e.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Quantity).HasDefaultValue(1);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.HasIndex(e => new { e.OrderId, e.ArtworkId });

                entity.HasOne(e => e.Artwork)
                    .WithMany()
                    .HasForeignKey(e => e.ArtworkId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            //SeedRoles(modelBuilder);
        }

        public DbSet<Artwork> Artworks { get; set; }
        public DbSet<Cycle> Cycles { get; set; }
        public DbSet<SmtpSettings> SmtpSettings { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

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
