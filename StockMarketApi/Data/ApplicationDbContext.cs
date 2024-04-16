using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StockMarketApi.Models;

namespace StockMarketApi.Data
{
    public class ApplicationDbContext(DbContextOptions dbContextOptions) : IdentityDbContext<AppUser>(dbContextOptions)
    {
        public DbSet<Stock> Stocks { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Portfolio> Portfolios { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Portfolio>(s => s.HasKey(p => new { p.AppUserId, p.StockId }));

            builder.Entity<Portfolio>()
                .HasOne(u => u.AppUser)
                .WithMany(w => w.Portfolios)
                .HasForeignKey(p => p.AppUserId);

            builder.Entity<Portfolio>()
                .HasOne(u => u.Stock)
                .WithMany(w => w.Portfolios)
                .HasForeignKey(p => p.StockId);

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole 
                {
                    Name = "User",
                    NormalizedName = "USER"
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
