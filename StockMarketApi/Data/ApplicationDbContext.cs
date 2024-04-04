using Microsoft.EntityFrameworkCore;
using StockMarketApi.Models;

namespace StockMarketApi.Data
{
    public class ApplicationDbContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions)
    {
        public DbSet<Stock> Stocks { get; set; }

        public DbSet<Comment> Comments { get; set; }
    }
}
