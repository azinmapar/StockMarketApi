using Microsoft.EntityFrameworkCore;
using StockMarketApi.Data;
using StockMarketApi.Interfaces;
using StockMarketApi.Models;

namespace StockMarketApi.Repositories
{
    public class PortfolioRepository(ApplicationDbContext context) : IPortfolioRepository
    {

        private readonly ApplicationDbContext _context = context;
        public async Task<List<Stock>> GetUserPortfolioAsync(AppUser user)
        {
         
            return await _context.Portfolios.Where(s => s.AppUserId == user.Id)
                .Select(Stock => new Stock
                {
                    Id = Stock.Stock.Id,
                    Symbol = Stock.Stock.Symbol,
                    CompanyName = Stock.Stock.CompanyName,
                    Purchase = Stock.Stock.Purchase,
                    LastDiv = Stock.Stock.LastDiv,
                    Industry = Stock.Stock.Industry,
                    MarketCap = Stock.Stock.MarketCap,
                }).ToListAsync();

        }
    }
}
