using Microsoft.EntityFrameworkCore;
using StockMarketApi.Data;
using StockMarketApi.DTOs.Stock;
using StockMarketApi.Interfaces;
using StockMarketApi.Models;

namespace StockMarketApi.Repositories
{
    public class StockRepository(ApplicationDbContext context) : IStockRepository
    {

        private readonly ApplicationDbContext _context = context;

        public async Task<List<Stock>> GetAllAsync()
        {

            return await _context.Stocks.ToListAsync();

        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.FindAsync(id);
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


       
    }
}
