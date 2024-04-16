using Microsoft.EntityFrameworkCore;
using StockMarketApi.Data;
using StockMarketApi.Interfaces;
using StockMarketApi.Models;

namespace StockMarketApi.Repositories
{
    public class PortfolioRepository(ApplicationDbContext context) : IPortfolioRepository
    {

        private readonly ApplicationDbContext _context = context;

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;
        }

        public async Task<Portfolio?> DeleteAsync(Stock stock, AppUser user)
        {

            var portfolio =await  _context.Portfolios.FirstOrDefaultAsync(e => e.StockId == stock.Id && e.AppUserId == user.Id);
            if (portfolio == null)
            {
                return null;
            }
            _context.Portfolios.Remove(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;
        }

        public async Task<List<Stock>> GetUserPortfolioAsync(AppUser user)
        {
         
            return await _context.Portfolios.Where(s => s.AppUserId.ToLower() == user.Id.ToLower())
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
