using Microsoft.EntityFrameworkCore;
using StockMarketApi.Data;
using StockMarketApi.DTOs.Stock;
using StockMarketApi.Helpers;
using StockMarketApi.Interfaces;
using StockMarketApi.Models;

namespace StockMarketApi.Repositories
{
    public class StockRepository(ApplicationDbContext context) : IStockRepository
    {

        private readonly ApplicationDbContext _context = context;

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {

            var stocks =  _context.Stocks.Include(c => c.Comments).AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
            }
            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
                } else if (query.SortBy.Equals("CompanyName", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending ? stocks.OrderByDescending(s => s.CompanyName) : stocks.OrderBy(s => s.CompanyName);
                }
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;



            return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();

        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Stock> CreateAsync(Stock stock)
        {
            await _context.Stocks.AddAsync(stock);
            await _context.SaveChangesAsync();
            return stock;
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockDto updatedStock)
        {

            var currentStock = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);

            if (currentStock == null)
            {
                return null;
            }

            currentStock.Symbol = updatedStock.Symbol;
            currentStock.CompanyName = updatedStock.CompanyName;
            currentStock.Purchase = updatedStock.Purchase;
            currentStock.LastDiv = updatedStock.LastDiv;
            currentStock.Industry = updatedStock.Industry;
            currentStock.MarketCap = updatedStock.MarketCap;
            await _context.SaveChangesAsync();

            return currentStock;

        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
            if (stock == null)
            {
                return null;
            }

            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();
            return stock;
        }

        public async Task<bool> StockExistsAsync(int id)
        {
            return await _context.Stocks.AnyAsync(s => s.Id == id);
        }
    }
}
